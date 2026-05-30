using NTokenizers.Swift;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class SwiftWriter(IAnsiConsole ansiConsole, SwiftStyles styles)
    : BaseInlineWriter<SwiftToken, SwiftTokenType>(ansiConsole)
{
    protected override Style GetStyle(SwiftTokenType token) => token switch
    {
        SwiftTokenType.NotDefined => styles.NotDefined,
        SwiftTokenType.Operator => styles.Operator,
        SwiftTokenType.OpenParenthesis => styles.OpenParenthesis,
        SwiftTokenType.CloseParenthesis => styles.CloseParenthesis,
        SwiftTokenType.OpenBrace => styles.OpenBrace,
        SwiftTokenType.CloseBrace => styles.CloseBrace,
        SwiftTokenType.OpenBracket => styles.OpenBracket,
        SwiftTokenType.CloseBracket => styles.CloseBracket,
        SwiftTokenType.Comma => styles.Comma,
        SwiftTokenType.Dot => styles.Dot,
        SwiftTokenType.SequenceTerminator => styles.SequenceTerminator,
        SwiftTokenType.Colon => styles.Colon,
        SwiftTokenType.DoubleColon => styles.DoubleColon,
        SwiftTokenType.At => styles.At,
        SwiftTokenType.Pound => styles.Pound,
        SwiftTokenType.QuestionMark => styles.QuestionMark,
        SwiftTokenType.StringValue => styles.StringValue,
        SwiftTokenType.CharValue => styles.CharValue,
        SwiftTokenType.Number => styles.Number,
        SwiftTokenType.Boolean => styles.Boolean,
        SwiftTokenType.Null => styles.Null,
        SwiftTokenType.Identifier => styles.Identifier,
        SwiftTokenType.Keyword => styles.Keyword,
        SwiftTokenType.Comment => styles.Comment,
        SwiftTokenType.Whitespace => styles.Whitespace,
        _ => new Style(Color.White)
    };
}
