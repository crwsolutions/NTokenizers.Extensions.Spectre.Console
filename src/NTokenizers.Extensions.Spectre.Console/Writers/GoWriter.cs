using NTokenizers.Go;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class GoWriter(IAnsiConsole ansiConsole, GoStyles styles)
    : BaseInlineWriter<GoToken, GoTokenType>(ansiConsole)
{
    protected override Style GetStyle(GoTokenType token) => token switch
    {
        GoTokenType.NotDefined => styles.NotDefined,
        GoTokenType.Operator => styles.Operator,
        GoTokenType.OpenParenthesis => styles.OpenParenthesis,
        GoTokenType.CloseParenthesis => styles.CloseParenthesis,
        GoTokenType.OpenBrace => styles.OpenBrace,
        GoTokenType.CloseBrace => styles.CloseBrace,
        GoTokenType.OpenBracket => styles.OpenBracket,
        GoTokenType.CloseBracket => styles.CloseBracket,
        GoTokenType.Comma => styles.Comma,
        GoTokenType.Dot => styles.Dot,
        GoTokenType.SequenceTerminator => styles.SequenceTerminator,
        GoTokenType.Colon => styles.Colon,
        GoTokenType.DoubleColon => styles.DoubleColon,
        GoTokenType.At => styles.At,
        GoTokenType.Pound => styles.Pound,
        GoTokenType.QuestionMark => styles.QuestionMark,
        GoTokenType.StringValue => styles.StringValue,
        GoTokenType.CharValue => styles.CharValue,
        GoTokenType.Number => styles.Number,
        GoTokenType.Boolean => styles.Boolean,
        GoTokenType.Null => styles.Null,
        GoTokenType.Identifier => styles.Identifier,
        GoTokenType.Keyword => styles.Keyword,
        GoTokenType.Comment => styles.Comment,
        GoTokenType.Whitespace => styles.Whitespace,
        _ => new Style(Color.White)
    };
}
