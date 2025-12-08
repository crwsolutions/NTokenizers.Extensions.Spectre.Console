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
    /// <returns>A task that represents the asynchronous operation. The result is the input stream as a string.</returns>
    public static async Task<string> WriteTypescriptAsync(this IAnsiConsole ansiConsole, Stream stream, TypescriptStyles? typescriptStyles = null)
    {
        var typescriptWriter = new TypescriptWriter(ansiConsole, typescriptStyles ?? TypescriptStyles.Default);
        return await TypescriptTokenizer.Create().ParseAsync(
            stream,
            typescriptWriter.WriteToken
        );
    }

    /// <summary>
    /// Writes TypeScript code from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The <see cref="Stream"/> containing TypeScript code.</param>
    /// <param name="typescriptStyles">The <see cref="TypescriptStyles"/> to use for styling.</param>
    /// <returns>The input stream as a string.</returns>
    public static string WriteTypescript(this IAnsiConsole ansiConsole, Stream stream, TypescriptStyles? typescriptStyles = null)
    {
        var t = Task.Run(() => WriteTypescriptAsync(ansiConsole, stream, typescriptStyles));
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
        var t = Task.Run(() => WriteTypescriptAsync(ansiConsole, stream, typescriptStyles));
        t.GetAwaiter().GetResult();
    }
}