using NTokenizers.Css;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class CssWriter(IAnsiConsole ansiConsole, CssStyles styles) : BaseInlineWriter<CssToken, CssTokenType>(ansiConsole)
{
    protected override Style GetStyle(CssTokenType token) => token switch
    {
        CssTokenType.StartRuleSet => styles.StartRuleSet,
        CssTokenType.EndRuleSet => styles.EndRuleSet,
        CssTokenType.Selector => styles.Selector,
        CssTokenType.PseudoElement => styles.PseudoElement,
        CssTokenType.PropertyName => styles.PropertyName,
        CssTokenType.StringValue => styles.StringValue,
        CssTokenType.Quote => styles.Quote,
        CssTokenType.Number => styles.Number,
        CssTokenType.Unit => styles.Unit,
        CssTokenType.Function => styles.Function,
        CssTokenType.OpenParen => styles.OpenParen,
        CssTokenType.CloseParen => styles.CloseParen,
        CssTokenType.Comment => styles.Comment,
        CssTokenType.Colon => styles.Colon,
        CssTokenType.Semicolon => styles.Semicolon,
        CssTokenType.Comma => styles.Comma,
        CssTokenType.Whitespace => styles.Whitespace,
        CssTokenType.AtRule => styles.AtRule,
        CssTokenType.Identifier => styles.Identifier,
        CssTokenType.Operator => styles.Operator,
        CssTokenType.DotClass => styles.DotClass,
        CssTokenType.LeftBracket => styles.LeftBracket,
        CssTokenType.RightBracket => styles.RightBracket,
        CssTokenType.Equals => styles.EqualsStyle,
        _ => new Style()
    };
}
