using NTokenizers.Typescript;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class TypescriptWriter : BaseInlineWriter<TypescriptToken, TypescriptTokenType>
{
    private readonly TypescriptStyles _styles;

    internal TypescriptWriter(IAnsiConsole ansiConsole, TypescriptStyles styles, Paragraph? liveParagraph, LiveDisplayContext? ctx) 
        : base(ansiConsole, liveParagraph, ctx)
    {
        _styles = styles;
    }

    internal TypescriptWriter(IAnsiConsole ansiConsole, TypescriptStyles styles) : base(ansiConsole)
    {
        _styles = styles;
    }

    protected override Style GetStyle(TypescriptTokenType token) => token switch
    {
        TypescriptTokenType.OpenParenthesis => _styles.OpenParenthesis,
        TypescriptTokenType.CloseParenthesis => _styles.CloseParenthesis,
        TypescriptTokenType.Comma => _styles.Comma,
        TypescriptTokenType.StringValue => _styles.StringValue,
        TypescriptTokenType.Number => _styles.Number,
        TypescriptTokenType.Keyword => _styles.Keyword,
        TypescriptTokenType.Identifier => _styles.Identifier,
        TypescriptTokenType.Comment => _styles.Comment,
        TypescriptTokenType.Operator => _styles.Operator,
        TypescriptTokenType.And => _styles.And,
        TypescriptTokenType.Or => _styles.Or,
        TypescriptTokenType.Equals => _styles.EqualsStyle,
        TypescriptTokenType.NotEquals => _styles.NotEquals,
        TypescriptTokenType.In => _styles.In,
        TypescriptTokenType.NotIn => _styles.NotIn,
        TypescriptTokenType.Like => _styles.Like,
        TypescriptTokenType.NotLike => _styles.NotLike,
        TypescriptTokenType.Limit => _styles.Limit,
        TypescriptTokenType.Match => _styles.Match,
        TypescriptTokenType.SequenceTerminator => _styles.SequenceTerminator,
        TypescriptTokenType.Dot => _styles.Dot,
        TypescriptTokenType.Whitespace => _styles.Whitespace,
        TypescriptTokenType.DateTimeValue => _styles.DateTimeValue,
        TypescriptTokenType.Fingerprint => _styles.Fingerprint,
        TypescriptTokenType.Message => _styles.Message,
        TypescriptTokenType.StackFrame => _styles.StackFrame,
        TypescriptTokenType.ExceptionType => _styles.ExceptionType,
        TypescriptTokenType.Application => _styles.Application,
        TypescriptTokenType.Between => _styles.Between,
        TypescriptTokenType.NotDefined => _styles.NotDefined,
        TypescriptTokenType.Invalid => _styles.Invalid,
        _ => new Style()
    };
}