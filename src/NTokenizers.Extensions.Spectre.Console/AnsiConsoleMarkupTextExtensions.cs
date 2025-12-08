using NTokenizers.Markup;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using System.Text;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render markup text with syntax highlighting.
/// </summary>
public static class AnsiConsoleMarkupTextExtensions
{
    /// <summary>
    /// Writes markup text to the console asynchronously using specified markup styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="stream">The stream containing the markup text.</param>
    /// <param name="markupStyles">The markup styles to use for rendering.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous write operation and contains the parsed string.</returns>
    public static async Task<string> WriteMarkupTextAsync(this IAnsiConsole ansiConsole, Stream stream, MarkupStyles? markupStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var markupWriter = MarkupWriter.Create(ansiConsole);
        MarkupWriter.MarkupStyles = markupStyles ?? MarkupStyles.Default;
        if (encoding is null)
        {
            // Call overload without encoding to preserve BOM detection
            return await MarkupTokenizer.Create().ParseAsync(
                stream,
                ct,
                async token => await markupWriter.WriteAsync(token)
            );
        }
        else
        {
            // Call overload with encoding and cancellation token
            return await MarkupTokenizer.Create().ParseAsync(
                stream,
                encoding,
                ct,
                async token => await markupWriter.WriteAsync(token)
            );
        }
    }

    /// <summary>
    /// Writes markup text to the console synchronously using specified markup styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="stream">The stream containing the markup text.</param>
    /// <param name="markupStyles">The markup styles to use for rendering.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The parsed string from the input stream.</returns>
    public static string WriteMarkupText(this IAnsiConsole ansiConsole, Stream stream, MarkupStyles? markupStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteMarkupTextAsync(ansiConsole, stream, markupStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
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
        var t = Task.Run(() => WriteMarkupTextAsync(ansiConsole, stream, markupStyles, null, default));
        t.GetAwaiter().GetResult();
    }
}