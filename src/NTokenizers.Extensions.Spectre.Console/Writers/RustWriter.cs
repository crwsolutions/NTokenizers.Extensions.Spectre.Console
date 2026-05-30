using NTokenizers.Rust;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class RustWriter(IAnsiConsole ansiConsole, RustStyles styles)
    : BaseInlineWriter<RustToken, RustTokenType>(ansiConsole)
{
    protected override Style GetStyle(RustTokenType token) => token switch
    {
        RustTokenType.NotDefined => styles.NotDefined,
        RustTokenType.Operator => styles.Operator,
        RustTokenType.OpenParenthesis => styles.OpenParenthesis,
        RustTokenType.CloseParenthesis => styles.CloseParenthesis,
        RustTokenType.OpenBrace => styles.OpenBrace,
        RustTokenType.CloseBrace => styles.CloseBrace,
        RustTokenType.OpenBracket => styles.OpenBracket,
        RustTokenType.CloseBracket => styles.CloseBracket,
        RustTokenType.Comma => styles.Comma,
        RustTokenType.Dot => styles.Dot,
        RustTokenType.SequenceTerminator => styles.SequenceTerminator,
        RustTokenType.Colon => styles.Colon,
        RustTokenType.DoubleColon => styles.DoubleColon,
        RustTokenType.FatArrow => styles.FatArrow,
        RustTokenType.Arrow => styles.Arrow,
        RustTokenType.Pound => styles.Pound,
        RustTokenType.At => styles.At,
        RustTokenType.QuestionMark => styles.QuestionMark,
        RustTokenType.StringValue => styles.StringValue,
        RustTokenType.CharValue => styles.CharValue,
        RustTokenType.Number => styles.Number,
        RustTokenType.Boolean => styles.Boolean,
        RustTokenType.Lifetime => styles.Lifetime,
        RustTokenType.Identifier => styles.Identifier,
        RustTokenType.Keyword => styles.Keyword,
        RustTokenType.Comment => styles.Comment,
        RustTokenType.Whitespace => styles.Whitespace,
        _ => new Style(Color.White)
    };
}
