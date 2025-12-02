using NTokenizers.Json;
using Spectre.Console.Extensions.NTokenizers.Styles;

namespace Spectre.Console.Extensions.NTokenizers.Writers;

internal sealed class JsonWriter(IAnsiConsole ansiConsole, JsonStyles styles) : BaseInlineWriter<JsonToken, JsonTokenType>(ansiConsole)
{
    protected override Style GetStyle(JsonTokenType token) => token switch
    {
        JsonTokenType.StartObject => styles.StartObject,
        JsonTokenType.EndObject => styles.EndObject,
        JsonTokenType.StartArray => styles.StartArray,
        JsonTokenType.EndArray => styles.EndArray,
        JsonTokenType.PropertyName => styles.PropertyName,
        JsonTokenType.Colon => styles.Colon,
        JsonTokenType.Comma => styles.Comma,
        JsonTokenType.StringValue => styles.StringValue,
        JsonTokenType.Number => styles.Number,
        JsonTokenType.True => styles.True,
        JsonTokenType.False => styles.False,
        JsonTokenType.Null => styles.Null,
        _ => new Style()
    };
}
