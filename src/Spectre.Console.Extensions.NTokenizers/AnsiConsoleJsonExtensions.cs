using NTokenizers.Json;
using Spectre.Console.Extensions.NTokenizers.Styles;
using Spectre.Console.Extensions.NTokenizers.Writers;
using System.Text;

namespace Spectre.Console.Extensions.NTokenizers;
public static class AnsiConsoleJsonExtensions
{
    public static async Task WriteJsonAsync(this IAnsiConsole ansiConsole, Stream stream) =>
        await WriteJsonAsync(ansiConsole, stream, JsonStyles.Default);

    public static async Task WriteJsonAsync(this IAnsiConsole ansiConsole, Stream stream, JsonStyles jsonStyles)
    {
        var jsonWriter = new JsonWriter(ansiConsole, jsonStyles);
        await JsonTokenizer.Create().ParseAsync(
            stream,
            jsonWriter.WriteToken
        );
    }

    public static void WriteJson(this IAnsiConsole ansiConsole, Stream stream) =>
        WriteJson(ansiConsole, stream, JsonStyles.Default);

    public static void WriteJson(this IAnsiConsole ansiConsole, Stream stream, JsonStyles jsonStyles)
    {
        var t = Task.Run(() => WriteJsonAsync(ansiConsole, stream, jsonStyles));
        t.GetAwaiter().GetResult();
    }

    public static void WriteJson(this IAnsiConsole ansiConsole, string value) =>
        WriteJson(ansiConsole, value, JsonStyles.Default);

    public static void WriteJson(this IAnsiConsole ansiConsole, string value, JsonStyles jsonStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteJsonAsync(ansiConsole, stream, jsonStyles));
        t.GetAwaiter().GetResult();
    }
}
