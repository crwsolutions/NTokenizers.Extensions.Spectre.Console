using NTokenizers.C;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class CWriter(IAnsiConsole ansiConsole, CStyles styles)
    : BaseInlineWriter<CToken, CTokenType>(ansiConsole)
{
    protected override Style GetStyle(CTokenType token) => token switch
    {
        CTokenType.NotDefined => styles.NotDefined,
        CTokenType.Operator => styles.Operator,
        CTokenType.OpenParenthesis => styles.OpenParenthesis,
        CTokenType.CloseParenthesis => styles.CloseParenthesis,
        CTokenType.OpenBrace => styles.OpenBrace,
        CTokenType.CloseBrace => styles.CloseBrace,
        CTokenType.OpenBracket => styles.OpenBracket,
        CTokenType.CloseBracket => styles.CloseBracket,
        CTokenType.Comma => styles.Comma,
        CTokenType.Dot => styles.Dot,
        CTokenType.Arrow => styles.Arrow,
        CTokenType.SequenceTerminator => styles.SequenceTerminator,
        CTokenType.Colon => styles.Colon,
        CTokenType.StringValue => styles.StringValue,
        CTokenType.CharValue => styles.CharValue,
        CTokenType.Number => styles.Number,
        CTokenType.Identifier => styles.Identifier,
        CTokenType.Keyword => styles.Keyword,
        CTokenType.Preprocessor => styles.Preprocessor,
        CTokenType.Comment => styles.Comment,
        CTokenType.Whitespace => styles.Whitespace,
        _ => new Style(Color.White)
    };
}
