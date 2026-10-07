using NTokenizers.Core;
using NTokenizers.Markdown;
using NTokenizers.Markdown.Metadata;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

/// <summary>
/// One rendering context per markdown nesting level (mirrors the ToHtml architecture where each
/// <c>BlockquoteHtmlWriter</c> owns a fresh <c>MarkdownBlockTokenDispatcher</c>). A context owns
/// the paragraph state, the pending block separation, the heading state, and the
/// <see cref="LiveBlock"/> that receives its content. It is a stack machine that mirrors the
/// deeply-nested token stream.
/// </summary>
/// <remarks>
/// Whitespace principle (from ToHtml): the writer only ever ADDS a block separation, it never
/// removes whitespace from the stream. A pending separation is written before the next token and
/// dropped when the container's content completes.
///
/// Sub-document blocks (blockquote, fenced/indented code, heading, list item) register a child
/// context as the metadata's inline-token handler: start administration, then
/// <c>RegisterInlineTokenHandler(token =&gt; context.WriteToken(token), commit)</c>, where commit is
/// the end administration. The region closes exactly once, when the region's outermost block
/// completes (a sub-document's commit, a flat list's <c>ListEnd</c>, or a root horizontal rule).
///
/// Lists are flat (their <c>ListStart</c>/<c>ListEnd</c> tokens are not sub-documents): the owning
/// context handles <c>ListStart</c>, items and <c>ListEnd</c> inline.
/// </remarks>
internal sealed class MarkdownBlockContext
{
    private readonly MarkdownWriter _owner;
    private readonly LiveBlock _region;
    private readonly string? _itemPrefix;

    // The region-close signal. Non-null on the root context (region owner) and, after handoff, on
    // the outermost block's child context. Fired exactly once when the outermost block completes.
    private Action? _regionClose;

    // True only on the root context (the region owner). It may hand _regionClose to the
    // outermost sub-document block, or keep it to fire on a flat block (list / horizontal rule).
    private bool _ownsRegion;

    // Per-level paragraph / separation state (mirrors MarkdownBlockTokenDispatcher).
    private bool _inParagraph;
    private bool _pendingBlockBreak;
    private Paragraph? _currentParagraph;

    // Item state (set when this context renders a list item's content).
    private bool _itemFirstLineDone;

    // Heading state (active only on the heading's own context).
    private bool _headingActive;
    private int _headingLevel;
    private Style _headingStyle = new Style();
    private int _headingTextLength;
    private Paragraph? _headingParagraph;

    // Indented-code content paragraph (set when this context renders an indented code block).
    private Paragraph? _codeContentParagraph;

    // The list LiveBlock while dispatching the tokens of a list (list state is flat).
    private LiveBlock? _activeList;

    internal MarkdownBlockContext(
        MarkdownWriter owner,
        LiveBlock region,
        Action? regionClose = null,
        bool ownsRegion = false,
        string? itemPrefix = null)
    {
        _owner = owner;
        _region = region;
        _regionClose = regionClose;
        _ownsRegion = ownsRegion;
        _itemPrefix = itemPrefix;
    }

    /// <summary>
    /// Dispatches a markdown token, routing block tokens to dedicated handling (registering a
    /// child context for sub-document blocks) and inline tokens to the active paragraph. Mirrors
    /// <c>MarkdownBlockTokenDispatcher.WriteTokenAsync</c>.
    /// </summary>
    internal void WriteToken(MarkdownToken token)
    {
        // Write the block separation that follows a closed paragraph, before the next token. It
        // is intentionally omitted when the paragraph is the last token (end of content).
        if (_pendingBlockBreak)
        {
            _pendingBlockBreak = false;
            WriteBlockBreak();
        }

        switch (token.TokenType)
        {
            case MarkdownTokenType.ParagraphBlockStart:
                _inParagraph = true;
                _currentParagraph = new Paragraph();
                AddContentRow(_currentParagraph);
                break;

            case MarkdownTokenType.ParagraphBlockEnd:
                if (!_inParagraph)
                {
                    break;
                }

                _inParagraph = false;
                _currentParagraph = null;
                _pendingBlockBreak = true;
                break;

            case MarkdownTokenType.Text:
                // Newline/whitespace text outside a paragraph is block separation, not content:
                // it must never open a (empty) paragraph row, or a separator line appears before a
                // following nested block (e.g. the '\n' between a list item and its nested list).
                if (!_inParagraph)
                {
                    if (string.IsNullOrWhiteSpace(token.Value))
                    {
                        break;
                    }

                    // A list item in a quote streams inline-only content (no ParagraphBlockStart);
                    // open a paragraph lazily so the content has a row to render into.
                    OpenParagraphIfClosed();
                    if (!_inParagraph)
                    {
                        break;
                    }
                }

                if (_headingActive)
                {
                    _headingTextLength += token.Value.Length;
                    AppendToParagraph(token.Value, _headingStyle);
                }
                else if (_codeContentParagraph is not null)
                {
                    AppendToCodeContent(token.Value);
                }
                else
                {
                    AppendToParagraph(token.Value, _owner.MarkdownStyles.DefaultStyle);
                }

                break;

            case MarkdownTokenType.Heading:
                WriteHeading(token);
                break;

            case MarkdownTokenType.HorizontalRule:
                _region.AddText(new string('─', System.Console.WindowWidth), _owner.MarkdownStyles.HorizontalRule);
                // A root horizontal rule is a complete outermost block: close the region.
                CloseRegionIfOwned();
                break;

            case MarkdownTokenType.Blockquote:
                WriteBlockquote(token);
                break;

            case MarkdownTokenType.ListStart:
                WriteListStart(token);
                break;

            case MarkdownTokenType.ListEnd:
                WriteListEnd();
                break;

            case MarkdownTokenType.UnorderedListItem:
                WriteListItem(token, ordered: false);
                break;

            case MarkdownTokenType.OrderedListItem:
                WriteListItem(token, ordered: true);
                break;

            case MarkdownTokenType.CodeBlock:
                WriteFencedCodeBlock(token);
                break;

            case MarkdownTokenType.IndentedCodeBlock:
                WriteIndentedCodeBlock(token);
                break;

            case MarkdownTokenType.Table:
                WriteTable(token);
                break;

            default:
                WriteInline(token);
                break;
        }
    }

    /// <summary>
    /// Commits the context when its container's inline content is complete: finalizes any heading
    /// and drops the pending block separation (omitted at end of content). Mirrors
    /// <c>BlockquoteHtmlWriter</c>'s <c>onInlinesCompleted</c> callback.
    /// </summary>
    internal void Commit()
    {
        if (_headingActive)
        {
            FinalizeHeading();
        }

        _pendingBlockBreak = false;
        // A sub-document block completes on commit: close the region when this is the outermost.
        _regionClose?.Invoke();
        _regionClose = null;
    }

    // Closes the region when this context owns it as the region owner handling a flat block
    // (list end / horizontal rule). The outermost sub-document instead closes via Commit.
    private void CloseRegionIfOwned()
    {
        if (_ownsRegion)
        {
            _regionClose?.Invoke();
            _regionClose = null;
        }
    }

    // The whitespace principle: the writer only ever ADDS a blank-line separation. For a list
    // item whose first content is a block construct, the first line is a fresh (indented) line.
    private void WriteBlockBreak()
    {
        if (_itemPrefix is not null && !_itemFirstLineDone)
        {
            _itemFirstLineDone = true;
            _region.AddItemRow(_itemPrefix, new Paragraph());
            return;
        }

        _region.AddRow(new Paragraph());
    }

    // Adds a content row to the region. The first content line of a list item carries the
    // item's marker prefix; continuation lines are padded to the gutter by the LiveBlock.
    private void AddContentRow(IRenderable content)
    {
        if (_itemPrefix is not null && !_itemFirstLineDone)
        {
            _itemFirstLineDone = true;
            _region.AddItemRow(_itemPrefix, content);
            return;
        }

        _region.AddRow(content);
    }

    private void AppendToParagraph(string value, Style style)
    {
        if (_currentParagraph is null || string.IsNullOrEmpty(value))
        {
            return;
        }

        _currentParagraph.Append(value, style);
    }

    // A list item in a quote streams inline-only content (no ParagraphBlockStart token): the item
    // text arrives as raw Text/inline tokens. Open a paragraph lazily so that content has a row
    // to render into. Top-level items already receive a ParagraphBlockStart, so this is a no-op
    // for them.
    private void OpenParagraphIfClosed()
    {
        if (_itemPrefix is null || _inParagraph)
        {
            return;
        }

        _inParagraph = true;
        _currentParagraph = new Paragraph();
        AddContentRow(_currentParagraph);
    }

    private void AppendToCodeContent(string value)
    {
        if (_codeContentParagraph is not null && string.IsNullOrEmpty(value) is false)
        {
            _codeContentParagraph.Append(value, _owner.MarkdownStyles.CodeBlock);
        }
    }

    private void WriteInline(MarkdownToken token)
    {
        if (_currentParagraph is null)
        {
            // Inline tokens only appear inside an open paragraph; ignore defensively.
            return;
        }

        var styles = _owner.MarkdownStyles;
        switch (token.TokenType)
        {
            case MarkdownTokenType.Link when token.Metadata is LinkMetadata linkMeta:
                _currentParagraph.Append(token.Value, styles.Link, new Link(linkMeta.Url));
                break;

            case MarkdownTokenType.Image when token.Metadata is LinkMetadata imageMeta:
                _currentParagraph.Append(token.Value, styles.Image, new Link(imageMeta.Url));
                break;

            default:
                _currentParagraph.Append(token.Value, styles.GetStyleForToken(token.TokenType));
                break;
        }
    }

    // A heading is a sub-document: the heading text streams through the registered handler. The
    // heading renders as a row in the current region; its underline is appended on commit.
    private void WriteHeading(MarkdownToken token)
    {
        if (token.Metadata is not HeadingMetadata meta)
        {
            return;
        }

        var styles = _owner.MarkdownStyles.MarkdownHeadingStyles;
        var headingStyle = meta.Level switch
        {
            1 => styles.Level1,
            >= 2 and <= 4 => styles.Level2To4,
            _ => styles.Level5AndAbove
        };

        var headingParagraph = new Paragraph();
        if (meta.Level == 1)
        {
            headingParagraph.Append("** ", headingStyle);
        }

        AddContentRow(headingParagraph);

        var context = new MarkdownBlockContext(_owner, _region, TakeRegionClose());
        context.InitializeHeading(meta.Level, headingStyle, headingParagraph);

        meta.RegisterInlineTokenHandler(
            sub => context.WriteToken(sub),
            () => context.Commit());
    }

    // Internal initializer so the heading state (private fields) can be set by the owning context
    // that just created the heading's child context.
    internal void InitializeHeading(int level, Style style, Paragraph paragraph)
    {
        _headingActive = true;
        _headingLevel = level;
        _headingStyle = style;
        _headingParagraph = paragraph;
        _currentParagraph = paragraph;
        _inParagraph = true;
    }

    private void FinalizeHeading()
    {
        _headingActive = false;
        if (_headingParagraph is null)
        {
            return;
        }

        if (_headingLevel == 1)
        {
            _headingParagraph.Append(" **", _headingStyle);
        }

        // Level 1 is underlined with '=' (spans the text plus the "** " decoration); level 2-4
        // with '-'; level 5+ have no underline (mirrors MarkdownHeadingWriter).
        if (_headingLevel == 1)
        {
            _region.AddText(new string('=', _headingTextLength + 6), _headingStyle);
        }
        else if (_headingLevel is >= 2 and <= 4)
        {
            _region.AddText(new string('-', _headingTextLength), _headingStyle);
        }

    }

    // A blockquote is a sub-document: the quoted content is a full markdown sub-stream. The
    // quote renders as a nested Quote LiveBlock row in the current region.
    private void WriteBlockquote(MarkdownToken token)
    {
        if (token.Metadata is not BlockquoteMetadata meta)
        {
            return;
        }

        var quote = LiveBlock.CreateQuote(_owner.MarkdownStyles.QuoteBorder);
        AddContentRow(quote);
        var context = new MarkdownBlockContext(_owner, quote, TakeRegionClose());
        _pendingBlockBreak = true;

        // Start administration above; content streams through the handler; commit = end
        // administration.
        meta.RegisterInlineTokenHandler(
            sub => context.WriteToken(sub),
            () => context.Commit());
    }

    // A list is flat (not a sub-document): ListStart opens the list LiveBlock, items stream in
    // as sub-document tokens, and ListEnd closes the list.
    private void WriteListStart(MarkdownToken token)
    {
        if (token.Metadata is not ListMetadata meta)
        {
            return;
        }

        var styles = _owner.MarkdownStyles;
        var markerStyle = meta.IsOrdered ? styles.OrderedListItem : styles.UnorderedListItem;
        var list = LiveBlock.CreateList(markerStyle, styles.ListGutter);
        _activeList = list;
        AddContentRow(list);
    }

    private void WriteListEnd()
    {
        _activeList = null;
        _pendingBlockBreak = true;
        // A flat list completes at ListEnd: close the region when the list is the outermost block.
        CloseRegionIfOwned();
    }

    // A list item is a sub-document: the item's content streams through the registered handler.
    // The item renders into the list region; the first content line carries the item's marker
    // prefix, continuation lines are padded to the gutter. Items are never the outermost block,
    // so they never receive the region-close.
    private void WriteListItem(MarkdownToken token, bool ordered)
    {
        string? prefix = null;
        if (ordered && token.Metadata is OrderedListItemMetadata orderedMeta)
        {
            prefix = $" {orderedMeta.Number.ToString().PadLeft(2)}{orderedMeta.Marker} ";
        }
        else if (!ordered && token.Metadata is ListItemMetadata unorderedMeta)
        {
            prefix = $" {unorderedMeta.Marker} ";
        }

        if (prefix is null)
        {
            return;
        }

        var list = _activeList ?? _region;
        var context = new MarkdownBlockContext(_owner, list, itemPrefix: prefix);
        (token.Metadata as InlineMetadata<MarkdownToken>)!.RegisterInlineTokenHandler(
            sub => context.WriteToken(sub),
            () => context.Commit());
    }

    // A fenced code block is a sub-document: the code streams through the registered handler.
    // The language label is part of the block model (not a direct console write), so it lands in
    // the correct (nested) context. The code renders as a nested Code LiveBlock row with the
    // label above it.
    private void WriteFencedCodeBlock(MarkdownToken token)
    {
        if (token.Metadata is not ICodeBlockMetadata meta)
        {
            return;
        }

        var language = string.IsNullOrWhiteSpace(meta.Language) ? "code" : meta.Language;

        var container = LiveBlock.CreateBare();
        AddContentRow(container);
        container.AddText($"{language}:", _owner.MarkdownStyles.CodeLabel);

        var code = LiveBlock.CreateCode(_owner.MarkdownStyles.CodeBorder);
        container.AddRow(code);
        var paragraph = new Paragraph();
        code.AddRow(paragraph);

        var context = new MarkdownBlockContext(_owner, code, TakeRegionClose());
        _pendingBlockBreak = true;

        // Start administration above; content streams through the handler; commit = end
        // administration.
        _owner.WireFencedCode(meta, paragraph, context);
    }

    // An indented code block is a sub-document: its (plain text) content streams through the
    // registered handler. It renders like a fenced code block with the label "code:".
    private void WriteIndentedCodeBlock(MarkdownToken token)
    {
        if (token.Metadata is not IndentedCodeBlockMetadata meta)
        {
            return;
        }

        var container = LiveBlock.CreateBare();
        AddContentRow(container);
        container.AddText("code:", _owner.MarkdownStyles.CodeLabel);

        var code = LiveBlock.CreateCode(_owner.MarkdownStyles.CodeBorder);
        container.AddRow(code);
        var paragraph = new Paragraph();
        code.AddRow(paragraph);

        var context = new MarkdownBlockContext(_owner, code, TakeRegionClose());
        context.SetCodeContentParagraph(paragraph);
        _pendingBlockBreak = true;

        meta.RegisterInlineTokenHandler(
            sub => context.WriteToken(sub),
            () => context.Commit());
    }

    // A table (even nested) renders in its own live region that grows as the tokenizer streams
    // the table content. The callback is awaited by the parser before the content streams, so it
    // must return immediately; the live region is kicked off as a fire-and-forget task and closes
    // itself when the content completes.
    private void WriteTable(MarkdownToken token)
    {
        if (token.Metadata is not TableMetadata meta)
        {
            return;
        }

        _pendingBlockBreak = true;
        var writer = new MarkdownTableWriter(_owner.Console, _owner.MarkdownStyles);
        _ = Task.Run(() => writer.WriteAsync(meta));
    }

    /// <summary>Sets the paragraph that receives indented-code content (plain text tokens).</summary>
    internal void SetCodeContentParagraph(Paragraph paragraph) => _codeContentParagraph = paragraph;

    // The region owner hands the region-close to the first sub-document block it dispatches (the
    // region's outermost block) and keeps none for itself. Nested contexts have no signal to hand,
    // so a nested block can never close the region.
    private Action? TakeRegionClose()
    {
        if (!_ownsRegion)
        {
            return null;
        }

        _ownsRegion = false;
        var close = _regionClose;
        _regionClose = null;
        return close;
    }
}
