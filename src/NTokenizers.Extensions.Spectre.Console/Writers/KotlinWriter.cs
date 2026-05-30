using NTokenizers.Kotlin;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class KotlinWriter(IAnsiConsole ansiConsole, KotlinStyles styles)
    : BaseInlineWriter<KotlinToken, KotlinTokenType>(ansiConsole)
{
    protected override Style GetStyle(KotlinTokenType token) => token switch
    {
        KotlinTokenType.NotDefined => styles.NotDefined,
        KotlinTokenType.Operator => styles.Operator,
        KotlinTokenType.OpenParenthesis => styles.OpenParenthesis,
        KotlinTokenType.CloseParenthesis => styles.CloseParenthesis,
        KotlinTokenType.OpenBrace => styles.OpenBrace,
        KotlinTokenType.CloseBrace => styles.CloseBrace,
        KotlinTokenType.OpenBracket => styles.OpenBracket,
        KotlinTokenType.CloseBracket => styles.CloseBracket,
        KotlinTokenType.Comma => styles.Comma,
        KotlinTokenType.Dot => styles.Dot,
        KotlinTokenType.SequenceTerminator => styles.SequenceTerminator,
        KotlinTokenType.Colon => styles.Colon,
        KotlinTokenType.DoubleColon => styles.DoubleColon,
        KotlinTokenType.QuestionMark => styles.QuestionMark,
        KotlinTokenType.At => styles.At,
        KotlinTokenType.Pound => styles.Pound,
        KotlinTokenType.StringValue => styles.StringValue,
        KotlinTokenType.CharValue => styles.CharValue,
        KotlinTokenType.Number => styles.Number,
        KotlinTokenType.Boolean => styles.Boolean,
        KotlinTokenType.Null => styles.Null,
        KotlinTokenType.Identifier => styles.Identifier,
        KotlinTokenType.Keyword => styles.Keyword,
        KotlinTokenType.Comment => styles.Comment,
        KotlinTokenType.Whitespace => styles.Whitespace,
        _ => new Style(Color.White)
    };
}
