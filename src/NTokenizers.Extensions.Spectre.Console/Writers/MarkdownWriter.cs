using NTokenizers.Markdown;
using NTokenizers.Markdown.Metadata;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;
using NTokenizers.Core;
using NTokenizers.CSharp;
using NTokenizers.C;
using NTokenizers.Cpp;
using NTokenizers.Go;
using NTokenizers.Java;
using NTokenizers.Kotlin;
using NTokenizers.Python;
using NTokenizers.Rust;
using NTokenizers.Swift;
using NTokenizers.Xml;
using NTokenizers.Typescript;
using NTokenizers.Css;
using NTokenizers.Json;
using NTokenizers.Yaml;
using NTokenizers.Sql;
using NTokenizers.Generic;
using NTokenizers.Html;
using NTokenizers.Toml;
using Spectre.Console.Rendering;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

/// <summary>
/// The root markdown renderer. It dispatches root-level tokens: plain paragraphs are written
/// directly to the console (existing behavior), while any other root block (heading, HR,
/// blockquote, list, fenced/indented code) streams to a shared <see cref="MarkdownStream"/> as
/// tokens arrive, so nested blocks render as fast as the tokenizer streams them.
/// </summary>
/// <remarks>
/// Forward-only rendering: the renderer never repositions the cursor and never redraws a line.
/// Every token is written to the console exactly once, in document order, as it arrives. The left
/// side of a block (border, gutter, list marker) is composed from the active nesting levels by the
/// shared stream; a line is broken (and the left side rebuilt) when a token no longer fits. There
/// is no <c>LiveDisplay</c> for blocks (only tables keep a live region), so there is no cursor
/// repositioning, no cursor hide, and no region-lifecycle for blocks.
/// </remarks>
internal class MarkdownWriter(IAnsiConsole ansiConsole)
{
    private readonly IAnsiConsole _ansiConsole = ansiConsole;

    // The in-progress root block (if any): the shared stream its blocks write to, and the context
    // that dispatches its tokens. The stream is finished once, forward-only, when the block
    // completes.
    private MarkdownStream? _rootStream;
    private MarkdownBlockContext? _rootContext;

    // The plain root paragraph is written directly, token by token (no buffering): a root
    // paragraph is the span between ParagraphBlockStart and ParagraphBlockEnd, and its tokens are
    // emitted straight to the console. This flag tracks whether that span is open, so the
    // whitespace Text tokens that separate blocks (outside a paragraph) are not emitted as content.
    private bool _inRootParagraph;

    // True when the last character written for the open root paragraph already ends with a line
    // break. The tokenizer does not emit a block's final newline, so the writer normally adds the
    // paragraph's closing line break; but if the paragraph content itself ends with a newline
    // (whitespace preserved verbatim), that line break is already there and is not added again.
    private bool _rootParagraphEndsNewline;

    // True once any root element (paragraph or block) has been written. A uniform one-blank-line
    // separation is kept between consecutive root elements: each element ends the stream with
    // exactly one line break (a block via its render's trailing line break, a paragraph via the
    // writer's newline), and the element that starts after a previous one is preceded by an
    // explicit blank line. There is no trailing blank line at end of stream.
    private bool _lastRootElementWritten;

    /// <summary>Gets or sets the markdown styles.</summary>
    internal MarkdownStyles MarkdownStyles { get; set; } = MarkdownStyles.Default;

    /// <summary>Gets the console.</summary>
    internal IAnsiConsole Console => _ansiConsole;

    /// <summary>Creates a new markdown writer.</summary>
    internal static MarkdownWriter Create(IAnsiConsole ansiConsole) => new(ansiConsole);

    /// <summary>
    /// Dispatches a root-level markdown token. Synchronous and forward-only: plain paragraphs are
    /// written directly to the console; other blocks stream per-token into the shared
    /// <see cref="MarkdownStream"/> as the tokenizer emits them. Runs in document order on the
    /// parse thread.
    /// </summary>
    internal async Task WriteAsync(MarkdownToken token)
    {
        // A list's continuation tokens (items, ListEnd) belong to the block currently being built.
        if (_rootStream is not null && token.TokenType is
            MarkdownTokenType.OrderedListItem or
            MarkdownTokenType.UnorderedListItem or
            MarkdownTokenType.ListEnd)
        {
            _rootContext!.WriteToken(token);
            return;
        }

        switch (token.TokenType)
        {
            case MarkdownTokenType.ParagraphBlockStart:
                // A root element that follows a previous one is separated by an explicit blank
                // line (each element otherwise ends the stream with exactly one line break).
                if (_lastRootElementWritten)
                {
                    _ansiConsole.WriteLine();
                }

                _inRootParagraph = true;
                _rootParagraphEndsNewline = false;
                break;

            case MarkdownTokenType.ParagraphBlockEnd:
                if (_inRootParagraph)
                {
                    // The paragraph ends the stream with exactly one line break. The tokenizer does
                    // not emit a block's final newline, so add it unless the paragraph content
                    // itself already ends with a newline (whitespace preserved verbatim).
                    if (!_rootParagraphEndsNewline)
                    {
                        _ansiConsole.WriteLine();
                    }

                    _inRootParagraph = false;
                    _lastRootElementWritten = true;
                }

                break;

            case MarkdownTokenType.Text:
                // Newline/whitespace text outside a paragraph is block separation, not content.
                if (_inRootParagraph)
                {
                    WriteRootText(token);
                }
                break;

            case MarkdownTokenType.Heading:
            case MarkdownTokenType.HorizontalRule:
            case MarkdownTokenType.Blockquote:
            case MarkdownTokenType.ListStart:
            case MarkdownTokenType.CodeBlock:
            case MarkdownTokenType.IndentedCodeBlock:
                OpenBlock(token);
                break;

            case MarkdownTokenType.Table:
                // A root table renders in its own live region that grows as the tokenizer
                // streams the table content (it is not forward-only: a table restructures as new
                // rows arrive). The callback is awaited by the parser before the table content
                // streams, so it must return immediately; the live region is kicked off as a
                // fire-and-forget task. The region stays open while the tokenizer streams the
                // content and closes itself when the content completes, so the next root element
                // cannot overlap it.
                if (token.Metadata is TableMetadata tableMeta)
                {
                    if (_lastRootElementWritten)
                    {
                        _ansiConsole.WriteLine();
                    }

                    _lastRootElementWritten = true;
                    var writer = new MarkdownTableWriter(_ansiConsole, MarkdownStyles);
                    await writer.WriteAsync(tableMeta);
                }
                break;

            default:
                // Inline tokens (bold, italic, link, ...) inside a root paragraph, written directly.
                if (_inRootParagraph)
                {
                    WriteRootInline(token);
                }
                break;
        }
    }

    /// <summary>
    /// Begins a new root block: creates the shared <see cref="MarkdownStream"/> its blocks write to
    /// and a root context that dispatches its tokens, then feeds the block's start token. The block
    /// streams forward-only; its final line is finished by <see cref="CompleteBlock"/> when the
    /// block completes.
    /// </summary>
    private void OpenBlock(MarkdownToken token)
    {
        // A root block that follows a previous element is separated by an explicit blank line (the
        // previous element ends the stream with exactly one line break; this adds the blank line).
        if (_lastRootElementWritten)
        {
            _ansiConsole.WriteLine();
        }

        var stream = new MarkdownStream(_ansiConsole);
        _rootStream = stream;
        // The root context owns the stream: it hands the completion signal to the outermost
        // sub-document block, or keeps it for a flat block (list / horizontal rule).
        _rootContext = new MarkdownBlockContext(this, stream, CompleteBlock, ownsRegion: true);
        _rootContext.WriteToken(token);
    }

    /// <summary>
    /// Finishes the in-progress root block (ends its final line) and clears the in-progress state.
    /// Invoked exactly when the block's outermost content is complete, in document order.
    /// </summary>
    private void CompleteBlock()
    {
        _rootStream?.Finish();
        _lastRootElementWritten = true;
        _rootStream = null;
        _rootContext = null;
    }

    private void WriteRootInline(MarkdownToken token)
    {
        if (string.IsNullOrEmpty(token.Value))
        {
            return;
        }

        // A link/image with URL metadata is rendered by the dedicated link writer (URL as the
        // display text when the label is absent), matching the previous root behavior.
        if (token.Metadata is LinkMetadata linkMeta)
        {
            new MarkdownLinkWriter(_ansiConsole, MarkdownStyles.GetStyleForToken(token.TokenType)).Write(linkMeta);
            return;
        }

        // Any other root inline (bold, italic, code, ...) is written directly as styled markup.
        _ansiConsole.Write(new Markup(Markup.Escape(token.Value), MarkdownStyles.GetStyleForToken(token.TokenType)));
        _rootParagraphEndsNewline = token.Value[token.Value.Length - 1] is '\n' or '\r';
    }

    // Plain root-paragraph content is written verbatim: every character that arrives is
    // emitted, including a soft-break '\n' (which renders as a line break). Blocks are a
    // different story: the tokenizer strips whitespace and does not emit a block's final
    // newline, so the shared MarkdownStream owns line breaks for block content.
    private void WriteRootText(MarkdownToken token)
    {
        if (string.IsNullOrEmpty(token.Value))
        {
            return;
        }

        _ansiConsole.Write(new Markup(Markup.Escape(token.Value), MarkdownStyles.DefaultStyle));
        _rootParagraphEndsNewline = token.Value[token.Value.Length - 1] is '\n' or '\r';
    }

    /// <summary>
    /// Wires a fenced code block's language-specific tokens into the shared stream, selecting the
    /// matching language writer (the same language set the previous implementation dispatched over).
    /// Each token is written to the stream as it arrives, so the code content streams forward-only
    /// under the code block's border. The block's <c>onInlinesCompleted</c> callback commits the
    /// context.
    /// </summary>
    internal Task WireFencedCode(ICodeBlockMetadata meta, MarkdownStream stream, MarkdownBlockContext context)
    {
        Action commit = () => context.Commit();
        if (meta is CSharpCodeBlockMetadata csharpMeta)
        {
            var writer = new CSharpWriter(_ansiConsole, MarkdownStyles.CSharpStyles);
            return csharpMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }
        if (meta is XmlCodeBlockMetadata xmlMeta)
        {
            var writer = new XmlWriter(_ansiConsole, MarkdownStyles.XmlStyles);
            return xmlMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }
        if (meta is HtmlCodeBlockMetadata htmlMeta)
        {
            var writer = new HtmlWriter(_ansiConsole, MarkdownStyles.HtmlStyles);
            return htmlMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }
        if (meta is TypeScriptCodeBlockMetadata tsMeta)
        {
            var writer = new TypescriptWriter(_ansiConsole, MarkdownStyles.TypescriptStyles);
            return tsMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }
        if (meta is CssCodeBlockMetadata cssMeta)
        {
            var writer = new CssWriter(_ansiConsole, MarkdownStyles.CssStyles);
            return cssMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }
        if (meta is JsonCodeBlockMetadata jsonMeta)
        {
            var writer = new JsonWriter(_ansiConsole, MarkdownStyles.JsonStyles);
            return jsonMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }
        if (meta is YamlCodeBlockMetadata yamlMeta)
        {
            var writer = new YamlWriter(_ansiConsole, MarkdownStyles.YamlStyles);
            return yamlMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }
        if (meta is TomlCodeBlockMetadata tomlMeta)
        {
            var writer = new TomlWriter(_ansiConsole, MarkdownStyles.TomlStyles);
            return tomlMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }
        if (meta is SqlCodeBlockMetadata sqlMeta)
        {
            var writer = new SqlWriter(_ansiConsole, MarkdownStyles.SqlStyles);
            return sqlMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }
        if (meta is CCodeBlockMetadata cMeta)
        {
            var writer = new CWriter(_ansiConsole, MarkdownStyles.CStyles);
            return cMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }
        if (meta is CppCodeBlockMetadata cppMeta)
        {
            var writer = new CppWriter(_ansiConsole, MarkdownStyles.CppStyles);
            return cppMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }
        if (meta is GoCodeBlockMetadata goMeta)
        {
            var writer = new GoWriter(_ansiConsole, MarkdownStyles.GoStyles);
            return goMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }
        if (meta is JavaCodeBlockMetadata javaMeta)
        {
            var writer = new JavaWriter(_ansiConsole, MarkdownStyles.JavaStyles);
            return javaMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }
        if (meta is KotlinCodeBlockMetadata kotlinMeta)
        {
            var writer = new KotlinWriter(_ansiConsole, MarkdownStyles.KotlinStyles);
            return kotlinMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }
        if (meta is PythonCodeBlockMetadata pythonMeta)
        {
            var writer = new PythonWriter(_ansiConsole, MarkdownStyles.PythonStyles);
            return pythonMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }
        if (meta is RustCodeBlockMetadata rustMeta)
        {
            var writer = new RustWriter(_ansiConsole, MarkdownStyles.RustStyles);
            return rustMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }
        if (meta is SwiftCodeBlockMetadata swiftMeta)
        {
            var writer = new SwiftWriter(_ansiConsole, MarkdownStyles.SwiftStyles);
            return swiftMeta.RegisterInlineTokenHandler(t => writer.WriteToStream(stream, t), commit);
        }

        // Generic fallback: plain markdown-token code block.
        var genericWriter = new GenericWriter(_ansiConsole);
        return ((InlineMetadata<MarkdownToken>)meta).RegisterInlineTokenHandler(t => genericWriter.WriteToStream(stream, t), commit);
    }
}
