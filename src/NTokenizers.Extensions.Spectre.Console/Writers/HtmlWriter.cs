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
                await tsMeta.RegisterInlineTokenHandler(writer.WriteToken);
            }
            else
            {
                await tsMeta.RegisterInlineTokenHandler(writer.WriteTokenInLiveTarget);
            }
        }
        else if (token.Metadata is CssCodeBlockMetadata cssMeta)
        {
            var writer = new CssWriter(_ansiConsole, _styles.CssStyles, _liveParagraph, ctx);
            if (liveTarget is null)
            {
                await cssMeta.RegisterInlineTokenHandler(writer.WriteToken);
            }
            else
            {
                await cssMeta.RegisterInlineTokenHandler(writer.WriteTokenInLiveTarget);
            }
        }
        else
        {
            await base.WriteTokenAsync(liveTarget, token, ctx);
        }
    }

    /// <summary>
    /// Registers the nested style/script handlers on the forward-only paragraph route. The HTML
    /// tokenizer emits a nested <c>CssCodeBlockMetadata</c> (style) or <c>TypeScriptCodeBlockMetadata</c>
    /// (script) token and, before streaming that element's content, waits up to its timeout for
    /// the nested handler to be registered. The registration is synchronous (it signals the
    /// metadata's handler task immediately); the returned processing task is deliberately not
    /// awaited, so the callback returns quickly and the tokenizer continues without a deadlock or
    /// the handler-wait timeout.
    /// </summary>
    /// <param name="paragraph">The paragraph to append the nested style/script tokens to.</param>
    /// <param name="token">The token to append; if it carries nested style/script metadata, the
    /// nested handler is registered instead of appending the (empty) wrapper value.</param>
    internal override void AppendToken(Paragraph paragraph, HtmlToken token)
    {
        if (token.Metadata is TypeScriptCodeBlockMetadata tsMeta)
        {
            var writer = new TypescriptWriter(_ansiConsole, _styles.TypescriptStyles);
            _ = tsMeta.RegisterInlineTokenHandler(t => writer.AppendToken(paragraph, t));
            return;
        }

        if (token.Metadata is CssCodeBlockMetadata cssMeta)
        {
            var writer = new CssWriter(_ansiConsole, _styles.CssStyles);
            _ = cssMeta.RegisterInlineTokenHandler(t => writer.AppendToken(paragraph, t));
            return;
        }

        base.AppendToken(paragraph, token);
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