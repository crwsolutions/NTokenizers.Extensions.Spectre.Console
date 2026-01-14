using NTokenizers.Markdown;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using System.Text;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render markdown text with syntax highlighting.
/// </summary>
public static class AnsiConsoleMarkdownExtensions
{
    /// <summary>
    /// Writes markdown text to the console asynchronously using specified markdown styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="stream">The stream containing the markdown text.</param>
    /// <param name="markdownStyles">The markdown styles to use for rendering.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous write operation and contains the parsed string.</returns>
    public static async Task<string> WriteMarkdownAsync(this IAnsiConsole ansiConsole, Stream stream, MarkdownStyles? markdownStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var markdownWriter = MarkdownWriter.Create(ansiConsole);
        markdownWriter.MarkdownStyles = markdownStyles ?? MarkdownStyles.Default;
        if (encoding is null)
        {
            // Call overload without encoding to preserve BOM detection
            return await MarkdownTokenizer.Create().ParseAsync(
                stream,
                ct,
                async token => await markdownWriter.WriteAsync(token)
            );
        }
        else
        {
            // Call overload with encoding and cancellation token
            return await MarkdownTokenizer.Create().ParseAsync(
                stream,
                encoding,
                ct,
                async token => await markdownWriter.WriteAsync(token)
            );
        }
    }

    /// <summary>
    /// Writes markdown text to the console synchronously using specified markdown styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="stream">The stream containing the markdown text.</param>
    /// <param name="markdownStyles">The markdown styles to use for rendering.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The parsed string from the input stream.</returns>
    public static string WriteMarkdown(this IAnsiConsole ansiConsole, Stream stream, MarkdownStyles? markdownStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteMarkdownAsync(ansiConsole, stream, markdownStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes markdown text to the console synchronously using default markdown styles.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="value">The markdown text to write.</param>
    public static void WriteMarkdown(this IAnsiConsole ansiConsole, string value) =>
        WriteMarkdown(ansiConsole, value, MarkdownStyles.Default);

    /// <summary>
    /// Writes markdown text to the console synchronously using specified markdown styles.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="value">The markdown text to write.</param>
    /// <param name="markdownStyles">The markdown styles to use for rendering.</param>
    public static void WriteMarkdown(this IAnsiConsole ansiConsole, string value, MarkdownStyles markdownStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteMarkdownAsync(ansiConsole, stream, markdownStyles, Encoding.UTF8, default));
        t.GetAwaiter().GetResult();
    }
}