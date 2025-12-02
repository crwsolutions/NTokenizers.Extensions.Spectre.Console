using NTokenizers.Markup;
using Spectre.Console.Extensions.NTokenizers.Styles;
using Spectre.Console.Extensions.NTokenizers.Writers;
using System.Text;

namespace Spectre.Console.Extensions.NTokenizers;
/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render markup text with syntax highlighting.
/// </summary>
/// <remarks>
/// This library offers Spectre.Console rendering extensions for various markup formats including XML, JSON, 
/// Markup, TypeScript, C#, and SQL with style-rich console syntax highlighting.
/// </remarks>
public static class AnsiConsoleMarkupTextExtensions
{
    /// <summary>
    /// Writes markup text to the console asynchronously using default markup styles.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="stream">The stream containing the markup text.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static async Task WriteMarkupTextAsync(this IAnsiConsole ansiConsole, Stream stream) => 
        await WriteMarkupTextAsync(ansiConsole, stream, MarkupStyles.Default);

    /// <summary>
    /// Writes markup text to the console asynchronously using specified markup styles.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="stream">The stream containing the markup text.</param>
    /// <param name="markupStyles">The markup styles to use for rendering.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static async Task WriteMarkupTextAsync(this IAnsiConsole ansiConsole, Stream stream, MarkupStyles markupStyles)
    {
        var markupWriter = MarkupWriter.Create(ansiConsole);
        MarkupWriter.MarkupStyles = markupStyles;
        await MarkupTokenizer.Create().ParseAsync(
            stream,
            async token => await markupWriter.WriteAsync(token)
        );
    }

    /// <summary>
    /// Writes markup text to the console synchronously using default markup styles.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="stream">The stream containing the markup text.</param>
    public static void WriteMarkupText(this IAnsiConsole ansiConsole, Stream stream) => 
        WriteMarkupText(ansiConsole, stream, MarkupStyles.Default);

    /// <summary>
    /// Writes markup text to the console synchronously using specified markup styles.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="stream">The stream containing the markup text.</param>
    /// <param name="markupStyles">The markup styles to use for rendering.</param>
    public static void WriteMarkupText(this IAnsiConsole ansiConsole, Stream stream, MarkupStyles markupStyles)
    {
        var t = Task.Run(() => WriteMarkupTextAsync(ansiConsole, stream));
        t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes markup text to the console synchronously using default markup styles.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="value">The markup text to write.</param>
    public static void WriteMarkupText(this IAnsiConsole ansiConsole, string value) =>
        WriteMarkupText(ansiConsole, value, MarkupStyles.Default);

    /// <summary>
    /// Writes markup text to the console synchronously using specified markup styles.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="value">The markup text to write.</param>
    /// <param name="markupStyles">The markup styles to use for rendering.</param>
    public static void WriteMarkupText(this IAnsiConsole ansiConsole, string value, MarkupStyles markupStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteMarkupTextAsync(ansiConsole, stream));
        t.GetAwaiter().GetResult();
    }
}