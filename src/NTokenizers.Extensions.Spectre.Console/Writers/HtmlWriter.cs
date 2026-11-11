using NTokenizers.Css;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Html;
using NTokenizers.Typescript;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal class HtmlWriter : BaseInlineWriter<HtmlToken, HtmlTokenType>
{
    private readonly HtmlStyles _styles;

    internal HtmlWriter(IAnsiConsole ansiConsole, HtmlStyles styles) : base(ansiConsole)
    {
        _styles = styles;
    }

    internal async Task WriteAsync(HtmlToken token) => await WriteTokenAsync(null, token, null);

    protected override async Task WriteTokenAsync(Paragraph? liveTarget, HtmlToken token, LiveDisplayContext? ctx)
    {
        if (token.Metadata is TypeScriptCodeBlockMetadata tsMeta)
        {
            var writer = new TypescriptWriter(_ansiConsole, _styles.TypescriptStyles, _liveParagraph, ctx);
            if (liveTarget is null)
            {
                await tsMeta.ParseInlinesAsync(writer.WriteToken);
            }
            else
            {
                await tsMeta.ParseInlinesAsync(writer.WriteTokenInLiveTarget);
            }
        }
        else if (token.Metadata is CssCodeBlockMetadata cssMeta)
        {
            var writer = new CssWriter(_ansiConsole, _styles.CssStyles, _liveParagraph, ctx);
            if (liveTarget is null)
            {
                await cssMeta.ParseInlinesAsync(writer.WriteToken);
            }
            else
            {
                await cssMeta.ParseInlinesAsync(writer.WriteTokenInLiveTarget);
            }
        }
        else
        {
            await base.WriteTokenAsync(liveTarget, token, ctx);
        }
    }

    /// <summary>
    /// Writes an HTML token to the stream. The HTML tokenizer emits a nested
    /// <c>CssCodeBlockMetadata</c> (style) or <c>TypeScriptCodeBlockMetadata</c> (script) token.
    /// The sub-tokenizer runs inline within the container handler via ParseInlinesAsync.
    /// </summary>
    /// <param name="stream">The stream to write the token (or nested style/script tokens) to.</param>
    /// <param name="token">The token to write; if it carries nested style/script metadata, the
    /// nested handler is registered instead of writing the (empty) wrapper value.</param>
    internal override void WriteToStream(MarkdownStream stream, HtmlToken token)
    {
        if (token.Metadata is TypeScriptCodeBlockMetadata tsMeta)
        {
            var writer = new TypescriptWriter(_ansiConsole, _styles.TypescriptStyles);
            var t = tsMeta.ParseInlinesAsync(t => { writer.WriteToStream(stream, t); return Task.CompletedTask; });
            t.GetAwaiter().GetResult();
            return;
        }

        if (token.Metadata is CssCodeBlockMetadata cssMeta)
        {
            var writer = new CssWriter(_ansiConsole, _styles.CssStyles);
            var t = cssMeta.ParseInlinesAsync(t => { writer.WriteToStream(stream, t); return Task.CompletedTask; });
            t.GetAwaiter().GetResult();
            return;
        }

        base.WriteToStream(stream, token);
    }

    protected override Style GetStyle(HtmlTokenType token) => token switch
    {
        HtmlTokenType.ElementName => _styles.ElementName,
        HtmlTokenType.Text => _styles.Text,
        HtmlTokenType.Comment => _styles.Comment,
        HtmlTokenType.DocumentTypeDeclaration => _styles.DocumentTypeDeclaration,
        HtmlTokenType.Whitespace => _styles.Whitespace,
        HtmlTokenType.OpeningAngleBracket => _styles.OpeningAngleBracket,
        HtmlTokenType.ClosingAngleBracket => _styles.ClosingAngleBracket,
        HtmlTokenType.AttributeName => _styles.AttributeName,
        HtmlTokenType.AttributeEquals => _styles.AttributeEquals,
        HtmlTokenType.AttributeValue => _styles.AttributeValue,
        HtmlTokenType.AttributeQuote => _styles.AttributeQuote,
        HtmlTokenType.SelfClosingSlash => _styles.SelfClosingSlash,
        _ => _styles.DefaultStyle,
    };
}