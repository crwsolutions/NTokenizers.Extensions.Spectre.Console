using NTokenizers.Cpp;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class CppWriter(IAnsiConsole ansiConsole, CppStyles styles)
    : BaseInlineWriter<CppToken, CppTokenType>(ansiConsole)
{
    protected override Style GetStyle(CppTokenType token) => token switch
    {
        CppTokenType.NotDefined => styles.NotDefined,
        CppTokenType.Operator => styles.Operator,
        CppTokenType.OpenParenthesis => styles.OpenParenthesis,
        CppTokenType.CloseParenthesis => styles.CloseParenthesis,
        CppTokenType.OpenBrace => styles.OpenBrace,
        CppTokenType.CloseBrace => styles.CloseBrace,
        CppTokenType.OpenBracket => styles.OpenBracket,
        CppTokenType.CloseBracket => styles.CloseBracket,
        CppTokenType.Comma => styles.Comma,
        CppTokenType.Dot => styles.Dot,
        CppTokenType.Arrow => styles.Arrow,
        CppTokenType.SequenceTerminator => styles.SequenceTerminator,
        CppTokenType.Colon => styles.Colon,
        CppTokenType.DoubleColon => styles.DoubleColon,
        CppTokenType.QuestionMark => styles.QuestionMark,
        CppTokenType.StringValue => styles.StringValue,
        CppTokenType.CharValue => styles.CharValue,
        CppTokenType.Number => styles.Number,
        CppTokenType.Boolean => styles.Boolean,
        CppTokenType.Null => styles.Null,
        CppTokenType.Identifier => styles.Identifier,
        CppTokenType.Keyword => styles.Keyword,
        CppTokenType.Comment => styles.Comment,
        CppTokenType.Whitespace => styles.Whitespace,
        _ => new Style(Color.White)
    };
}
