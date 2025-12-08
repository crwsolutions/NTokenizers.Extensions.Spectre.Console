using NTokenizers.Typescript;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using System.Text;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render TypeScript code with syntax highlighting.
/// </summary>
public static class AnsiConsoleTypescriptExtensions
{
    /// <summary>
    /// Writes TypeScript code from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The <see cref="Stream"/> containing TypeScript code.</param>
    /// <param name="typescriptStyles">The <see cref="TypescriptStyles"/> to use for styling.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The result is the input stream as a string.</returns>
    public static async Task<string> WriteTypescriptAsync(this IAnsiConsole ansiConsole, Stream stream, TypescriptStyles? typescriptStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var typescriptWriter = new TypescriptWriter(ansiConsole, typescriptStyles ?? TypescriptStyles.Default);
        if (encoding is null)
        {
            // Call overload without encoding to preserve BOM detection
            return await TypescriptTokenizer.Create().ParseAsync(
                stream,
                ct,
                typescriptWriter.WriteToken
            );
        }
        else
        {
            // Call overload with encoding and cancellation token
            return await TypescriptTokenizer.Create().ParseAsync(
                stream,
                encoding,
                ct,
                typescriptWriter.WriteToken
            );
        }
    }

    /// <summary>
    /// Writes TypeScript code from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The <see cref="Stream"/> containing TypeScript code.</param>
    /// <param name="typescriptStyles">The <see cref="TypescriptStyles"/> to use for styling.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The input stream as a string.</returns>
    public static string WriteTypescript(this IAnsiConsole ansiConsole, Stream stream, TypescriptStyles? typescriptStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteTypescriptAsync(ansiConsole, stream, typescriptStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes TypeScript code from a string to the console with default styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The TypeScript code as a string.</param>
    public static void WriteTypescript(this IAnsiConsole ansiConsole, string value) =>
        WriteTypescript(ansiConsole, value, TypescriptStyles.Default);

    /// <summary>
    /// Writes TypeScript code from a string to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The TypeScript code as a string.</param>
    /// <param name="typescriptStyles">The <see cref="TypescriptStyles"/> to use for styling.</param>
    public static void WriteTypescript(this IAnsiConsole ansiConsole, string value, TypescriptStyles typescriptStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteTypescriptAsync(ansiConsole, stream, typescriptStyles, null, default));
        t.GetAwaiter().GetResult();
    }
}