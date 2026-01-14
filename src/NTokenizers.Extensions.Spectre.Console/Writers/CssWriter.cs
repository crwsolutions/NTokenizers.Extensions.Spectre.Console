using NTokenizers.Css;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class CssWriter : BaseInlineWriter<CssToken, CssTokenType>
{
    private readonly CssStyles _styles;

    internal CssWriter(IAnsiConsole ansiConsole, CssStyles styles, Paragraph? liveParagraph, LiveDisplayContext? ctx)
        : base(ansiConsole, liveParagraph, ctx)
    {
        _styles = styles;
    }

    internal CssWriter(IAnsiConsole ansiConsole, CssStyles styles) : base(ansiConsole)
    {
        _styles = styles;
    }

    protected override Style GetStyle(CssTokenType token) => token switch
    {
        CssTokenType.StartRuleSet => _styles.StartRuleSet,
        CssTokenType.EndRuleSet => _styles.EndRuleSet,
        CssTokenType.Selector => _styles.Selector,
        CssTokenType.PseudoElement => _styles.PseudoElement,
        CssTokenType.PropertyName => _styles.PropertyName,
        CssTokenType.StringValue => _styles.StringValue,
        CssTokenType.Quote => _styles.Quote,
        CssTokenType.Number => _styles.Number,
        CssTokenType.Unit => _styles.Unit,
        CssTokenType.Function => _styles.Function,
        CssTokenType.OpenParen => _styles.OpenParen,
        CssTokenType.CloseParen => _styles.CloseParen,
        CssTokenType.Comment => _styles.Comment,
        CssTokenType.Colon => _styles.Colon,
        CssTokenType.Semicolon => _styles.Semicolon,
        CssTokenType.Comma => _styles.Comma,
        CssTokenType.Whitespace => _styles.Whitespace,
        CssTokenType.AtRule => _styles.AtRule,
        CssTokenType.Identifier => _styles.Identifier,
        CssTokenType.Operator => _styles.Operator,
        CssTokenType.DotClass => _styles.DotClass,
        CssTokenType.LeftBracket => _styles.LeftBracket,
        CssTokenType.RightBracket => _styles.RightBracket,
        CssTokenType.Equals => _styles.EqualsStyle,
        _ => new Style()
    };
}
