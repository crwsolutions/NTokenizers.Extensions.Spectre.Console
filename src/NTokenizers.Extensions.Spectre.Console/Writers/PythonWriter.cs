using NTokenizers.Python;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class PythonWriter(IAnsiConsole ansiConsole, PythonStyles styles)
    : BaseInlineWriter<PythonToken, PythonTokenType>(ansiConsole)
{
    protected override Style GetStyle(PythonTokenType token) => token switch
    {
        PythonTokenType.NotDefined => styles.NotDefined,
        PythonTokenType.Operator => styles.Operator,
        PythonTokenType.OpenParenthesis => styles.OpenParenthesis,
        PythonTokenType.CloseParenthesis => styles.CloseParenthesis,
        PythonTokenType.OpenBrace => styles.OpenBrace,
        PythonTokenType.CloseBrace => styles.CloseBrace,
        PythonTokenType.OpenBracket => styles.OpenBracket,
        PythonTokenType.CloseBracket => styles.CloseBracket,
        PythonTokenType.Comma => styles.Comma,
        PythonTokenType.Dot => styles.Dot,
        PythonTokenType.Colon => styles.Colon,
        PythonTokenType.Semicolon => styles.Semicolon,
        PythonTokenType.Hash => styles.Hash,
        PythonTokenType.StringValue => styles.StringValue,
        PythonTokenType.Number => styles.Number,
        PythonTokenType.Identifier => styles.Identifier,
        PythonTokenType.Keyword => styles.Keyword,
        PythonTokenType.Comment => styles.Comment,
        PythonTokenType.Whitespace => styles.Whitespace,
        _ => new Style(Color.White)
    };
}
