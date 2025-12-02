using NTokenizers.CSharp;
using Spectre.Console.Extensions.NTokenizers.Styles;

namespace Spectre.Console.Extensions.NTokenizers.Writers;

internal sealed class CSharpWriter(IAnsiConsole ansiConsole, CSharpStyles styles) : BaseInlineWriter<CSharpToken, CSharpTokenType>(ansiConsole)
{
    protected override Style GetStyle(CSharpTokenType token) => token switch
    {
        CSharpTokenType.Keyword => styles.Keyword,
        CSharpTokenType.Number => styles.Number,
        CSharpTokenType.StringValue => styles.StringValue,
        CSharpTokenType.Comment => styles.Comment,
        CSharpTokenType.Identifier => styles.Identifier,
        CSharpTokenType.And => styles.And,
        CSharpTokenType.Or => styles.Or,
        CSharpTokenType.Not => styles.Not,
        CSharpTokenType.Equals => styles.EqualsStyle,
        CSharpTokenType.NotEquals => styles.NotEquals,
        CSharpTokenType.GreaterThan => styles.GreaterThan,
        CSharpTokenType.LessThan => styles.LessThan,
        CSharpTokenType.GreaterThanOrEqual => styles.GreaterThanOrEqual,
        CSharpTokenType.LessThanOrEqual => styles.LessThanOrEqual,
        CSharpTokenType.Plus => styles.Plus,
        CSharpTokenType.Minus => styles.Minus,
        CSharpTokenType.Multiply => styles.Multiply,
        CSharpTokenType.Divide => styles.Divide,
        CSharpTokenType.Modulo => styles.Modulo,
        CSharpTokenType.Operator => styles.Operator,
        CSharpTokenType.OpenParenthesis => styles.OpenParenthesis,
        CSharpTokenType.CloseParenthesis => styles.CloseParenthesis,
        CSharpTokenType.Comma => styles.Comma,
        CSharpTokenType.Dot => styles.Dot,
        CSharpTokenType.SequenceTerminator => styles.SequenceTerminator,
        CSharpTokenType.Whitespace => styles.Whitespace,
        _ => new Style(Color.White)
    };
}