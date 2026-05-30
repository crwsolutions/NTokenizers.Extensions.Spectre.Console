using NTokenizers.Java;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class JavaWriter(IAnsiConsole ansiConsole, JavaStyles styles)
    : BaseInlineWriter<JavaToken, JavaTokenType>(ansiConsole)
{
    protected override Style GetStyle(JavaTokenType token) => token switch
    {
        JavaTokenType.NotDefined => styles.NotDefined,
        JavaTokenType.Operator => styles.Operator,
        JavaTokenType.OpenParenthesis => styles.OpenParenthesis,
        JavaTokenType.CloseParenthesis => styles.CloseParenthesis,
        JavaTokenType.OpenBrace => styles.OpenBrace,
        JavaTokenType.CloseBrace => styles.CloseBrace,
        JavaTokenType.OpenBracket => styles.OpenBracket,
        JavaTokenType.CloseBracket => styles.CloseBracket,
        JavaTokenType.Comma => styles.Comma,
        JavaTokenType.Dot => styles.Dot,
        JavaTokenType.SequenceTerminator => styles.SequenceTerminator,
        JavaTokenType.Colon => styles.Colon,
        JavaTokenType.DoubleColon => styles.DoubleColon,
        JavaTokenType.QuestionMark => styles.QuestionMark,
        JavaTokenType.StringValue => styles.StringValue,
        JavaTokenType.CharValue => styles.CharValue,
        JavaTokenType.Number => styles.Number,
        JavaTokenType.Boolean => styles.Boolean,
        JavaTokenType.Null => styles.Null,
        JavaTokenType.Identifier => styles.Identifier,
        JavaTokenType.Keyword => styles.Keyword,
        JavaTokenType.Comment => styles.Comment,
        JavaTokenType.Whitespace => styles.Whitespace,
        _ => new Style(Color.White)
    };
}
