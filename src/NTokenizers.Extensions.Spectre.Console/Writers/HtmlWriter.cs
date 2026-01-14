using NTokenizers.Css;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Html;
using NTokenizers.Typescript;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal class HtmlWriter(IAnsiConsole ansiConsole, HtmlStyles styles) : BaseInlineWriter<HtmlToken, HtmlTokenType>(ansiConsole)
{
    internal async Task WriteAsync(HtmlToken token) => await WriteTokenAsync(null, token, null);

    protected override async Task WriteTokenAsync(Paragraph? liveTarget, HtmlToken token, LiveDisplayContext? ctx)
    {
        if (token.Metadata is TypeScriptCodeBlockMetadata tsMeta)
        {
            var writer = new TypescriptWriter(ansiConsole, styles.TypescriptStyles, _liveParagraph, ctx);
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
            var writer = new CssWriter(ansiConsole, styles.CssStyles, _liveParagraph, ctx);
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

    protected override Style GetStyle(HtmlTokenType token) => token switch
    {
        HtmlTokenType.ElementName => styles.ElementName,
        HtmlTokenType.Text => styles.Text,
        HtmlTokenType.Comment => styles.Comment,
        HtmlTokenType.DocumentTypeDeclaration => styles.DocumentTypeDeclaration,
        HtmlTokenType.Whitespace => styles.Whitespace,
        HtmlTokenType.OpeningAngleBracket => styles.OpeningAngleBracket,
        HtmlTokenType.ClosingAngleBracket => styles.ClosingAngleBracket,
        HtmlTokenType.AttributeName => styles.AttributeName,
        HtmlTokenType.AttributeEquals => styles.AttributeEquals,
        HtmlTokenType.AttributeValue => styles.AttributeValue,
        HtmlTokenType.AttributeQuote => styles.AttributeQuote,
        HtmlTokenType.SelfClosingSlash => styles.SelfClosingSlash,
        _ => styles.DefaultStyle,
    };
}