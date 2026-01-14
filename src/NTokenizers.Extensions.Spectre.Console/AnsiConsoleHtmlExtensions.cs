using NTokenizers.Html;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using System.Text;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render HTML content with syntax highlighting.
/// </summary>
public static class AnsiConsoleHtmlExtensions
{
    /// <summary>
    /// Writes HTML content from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The <see cref="Stream"/> containing the HTML content.</param>
    /// <param name="htmlStyles">The <see cref="HtmlStyles"/> to use for styling the output.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The result is the input stream as a string.</returns>
    public static async Task<string> WriteHtmlAsync(this IAnsiConsole ansiConsole, Stream stream, HtmlStyles? htmlStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        htmlStyles ??= HtmlStyles.Default;
        var htmlWriter = new HtmlWriter(ansiConsole, htmlStyles);
        if (encoding is null)
        {
            // Call overload without encoding to preserve BOM detection
            return await HtmlTokenizer.Create().ParseAsync(
                stream,
                ct,
                async token => await htmlWriter.WriteAsync(token)
            );
        }
        else
        {
            // Call overload with encoding and cancellation token
            return await HtmlTokenizer.Create().ParseAsync(
                stream,
                encoding,
                ct,
                async token => await htmlWriter.WriteAsync(token)
            );
        }
    }

    /// <summary>
    /// Writes HTML content from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The <see cref="Stream"/> containing the HTML content.</param>
    /// <param name="htmlStyles">The <see cref="HtmlStyles"/> to use for styling the output.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The input stream as a string.</returns>
    public static string WriteHtml(this IAnsiConsole ansiConsole, Stream stream, HtmlStyles? htmlStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteHtmlAsync(ansiConsole, stream, htmlStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes HTML content from a string to the console with default styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The HTML content as a string.</param>
    public static void WriteHtml(this IAnsiConsole ansiConsole, string value) =>
        WriteHtml(ansiConsole, value, HtmlStyles.Default);

    /// <summary>
    /// Writes HTML content from a string to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The HTML content as a string.</param>
    /// <param name="htmlStyles">The <see cref="HtmlStyles"/> to use for styling the output.</param>
    public static void WriteHtml(this IAnsiConsole ansiConsole, string value, HtmlStyles htmlStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteHtmlAsync(ansiConsole, stream, htmlStyles, Encoding.UTF8, default));
        t.GetAwaiter().GetResult();
    }
}