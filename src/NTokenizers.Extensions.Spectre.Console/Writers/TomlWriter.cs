using NTokenizers.Toml;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class TomlWriter(IAnsiConsole ansiConsole, TomlStyles styles) : BaseInlineWriter<TomlToken, TomlTokenType>(ansiConsole)
{
    protected override Style GetStyle(TomlTokenType token) => token switch
    {
        TomlTokenType.Whitespace => styles.Whitespace,
        TomlTokenType.Comment => styles.Comment,
        TomlTokenType.Identifier => styles.Identifier,
        TomlTokenType.Dot => styles.Dot,
        TomlTokenType.Equal => styles.Equal,
        TomlTokenType.Comma => styles.Comma,
        TomlTokenType.OpenBracket => styles.OpenBracket,
        TomlTokenType.CloseBracket => styles.CloseBracket,
        TomlTokenType.OpenBrace => styles.OpenBrace,
        TomlTokenType.CloseBrace => styles.CloseBrace,
        TomlTokenType.StringQuote => styles.StringQuote,
        TomlTokenType.StringValue => styles.StringValue,
        TomlTokenType.Number => styles.Number,
        TomlTokenType.Boolean => styles.Boolean,
        TomlTokenType.DateTime => styles.DateTime,
        _ => new Style()
    };
}
