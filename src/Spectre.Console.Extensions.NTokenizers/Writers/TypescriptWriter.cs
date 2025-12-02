using NTokenizers.Typescript;
using Spectre.Console.Extensions.NTokenizers.Styles;

namespace Spectre.Console.Extensions.NTokenizers.Writers;

internal sealed class TypescriptWriter(IAnsiConsole ansiConsole, TypescriptStyles styles) : BaseInlineWriter<TypescriptToken, TypescriptTokenType>(ansiConsole)
{
    protected override Style GetStyle(TypescriptTokenType token) => token switch
    {
        TypescriptTokenType.OpenParenthesis => styles.OpenParenthesis,
        TypescriptTokenType.CloseParenthesis => styles.CloseParenthesis,
        TypescriptTokenType.Comma => styles.Comma,
        TypescriptTokenType.StringValue => styles.StringValue,
        TypescriptTokenType.Number => styles.Number,
        TypescriptTokenType.Keyword => styles.Keyword,
        TypescriptTokenType.Identifier => styles.Identifier,
        TypescriptTokenType.Comment => styles.Comment,
        TypescriptTokenType.Operator => styles.Operator,
        TypescriptTokenType.And => styles.And,
        TypescriptTokenType.Or => styles.Or,
        TypescriptTokenType.Equals => styles.EqualsStyle,
        TypescriptTokenType.NotEquals => styles.NotEquals,
        TypescriptTokenType.In => styles.In,
        TypescriptTokenType.NotIn => styles.NotIn,
        TypescriptTokenType.Like => styles.Like,
        TypescriptTokenType.NotLike => styles.NotLike,
        TypescriptTokenType.Limit => styles.Limit,
        TypescriptTokenType.Match => styles.Match,
        TypescriptTokenType.SequenceTerminator => styles.SequenceTerminator,
        TypescriptTokenType.Dot => styles.Dot,
        TypescriptTokenType.Whitespace => styles.Whitespace,
        TypescriptTokenType.DateTimeValue => styles.DateTimeValue,
        TypescriptTokenType.Fingerprint => styles.Fingerprint,
        TypescriptTokenType.Message => styles.Message,
        TypescriptTokenType.StackFrame => styles.StackFrame,
        TypescriptTokenType.ExceptionType => styles.ExceptionType,
        TypescriptTokenType.Application => styles.Application,
        TypescriptTokenType.Between => styles.Between,
        TypescriptTokenType.NotDefined => styles.NotDefined,
        TypescriptTokenType.Invalid => styles.Invalid,
        _ => new Style()
    };
}