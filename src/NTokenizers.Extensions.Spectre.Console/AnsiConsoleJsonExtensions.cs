using NTokenizers.Json;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using System.Text;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for writing JSON content to the console with syntax highlighting.
/// This class enables style-rich console output for JSON data using the Spectre.Console library
/// and NTokenizers for tokenization.
/// </summary>
public static class AnsiConsoleJsonExtensions
{
    /// <summary>
    /// Writes JSON content from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The console to write to.</param>
    /// <param name="stream">The stream containing JSON content.</param>
    /// <param name="jsonStyles">The styles to use for JSON token coloring.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The JSON content as a string.</returns>
    public static async Task<string> WriteJsonAsync(this IAnsiConsole ansiConsole, Stream stream, JsonStyles? jsonStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var jsonWriter = new JsonWriter(ansiConsole, jsonStyles ?? JsonStyles.Default);
        if (encoding is null)
        {
            // Call overload without encoding to preserve BOM detection
            return await JsonTokenizer.Create().ParseAsync(
                stream,
                ct,
                jsonWriter.WriteToken
            );
        }
        else
        {
            // Call overload with encoding and cancellation token
            return await JsonTokenizer.Create().ParseAsync(
                stream,
                encoding,
                ct,
                jsonWriter.WriteToken
            );
        }
    }

    /// <summary>
    /// Writes JSON content from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The console to write to.</param>
    /// <param name="stream">The stream containing JSON content.</param>
    /// <param name="jsonStyles">The styles to use for JSON token coloring.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The JSON content as a string.</returns>
    public static string WriteJson(this IAnsiConsole ansiConsole, Stream stream, JsonStyles? jsonStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteJsonAsync(ansiConsole, stream, jsonStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes JSON content from a string to the console with default styling.
    /// </summary>
    /// <param name="ansiConsole">The console to write to.</param>
    /// <param name="value">The JSON string to write.</param>
    public static void WriteJson(this IAnsiConsole ansiConsole, string value) =>
        WriteJson(ansiConsole, value, JsonStyles.Default);

    /// <summary>
    /// Writes JSON content from a string to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The console to write to.</param>
    /// <param name="value">The JSON string to write.</param>
    /// <param name="jsonStyles">The styles to use for JSON token coloring.</param>
    public static void WriteJson(this IAnsiConsole ansiConsole, string value, JsonStyles jsonStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteJsonAsync(ansiConsole, stream, jsonStyles, null, default));
        t.GetAwaiter().GetResult();
    }
}
