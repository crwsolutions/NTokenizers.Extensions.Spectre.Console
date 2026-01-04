using NTokenizers.Css;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using System.Text;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for writing CSS content to the console with syntax highlighting.
/// This class enables style-rich console output for CSS data using the Spectre.Console library
/// and NTokenizers for tokenization.
/// </summary>
public static class AnsiConsoleCssExtensions
{
    /// <summary>
    /// Writes CSS content from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The console to write to.</param>
    /// <param name="stream">The stream containing CSS content.</param>
    /// <param name="cssStyles">The styles to use for CSS token coloring.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The CSS content as a string.</returns>
    public static async Task<string> WriteCssAsync(this IAnsiConsole ansiConsole, Stream stream, CssStyles? cssStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var cssWriter = new CssWriter(ansiConsole, cssStyles ?? CssStyles.Default);
        if (encoding is null)
        {
            // Call overload without encoding to preserve BOM detection
            return await CssTokenizer.Create().ParseAsync(
                stream,
                ct,
                cssWriter.WriteToken
            );
        }
        else
        {
            // Call overload with encoding and cancellation token
            return await CssTokenizer.Create().ParseAsync(
                stream,
                encoding,
                ct,
                cssWriter.WriteToken
            );
        }
    }

    /// <summary>
    /// Writes CSS content from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The console to write to.</param>
    /// <param name="stream">The stream containing CSS content.</param>
    /// <param name="cssStyles">The styles to use for CSS token coloring.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The CSS content as a string.</returns>
    public static string WriteCss(this IAnsiConsole ansiConsole, Stream stream, CssStyles? cssStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteCssAsync(ansiConsole, stream, cssStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes CSS content from a string to the console with default styling.
    /// </summary>
    /// <param name="ansiConsole">The console to write to.</param>
    /// <param name="value">The CSS string to write.</param>
    public static void WriteCss(this IAnsiConsole ansiConsole, string value) =>
        WriteCss(ansiConsole, value, CssStyles.Default);

    /// <summary>
    /// Writes CSS content from a string to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The console to write to.</param>
    /// <param name="value">The CSS string to write.</param>
    /// <param name="cssStyles">The styles to use for CSS token coloring.</param>
    public static void WriteCss(this IAnsiConsole ansiConsole, string value, CssStyles cssStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteCssAsync(ansiConsole, stream, cssStyles, Encoding.UTF8, default));
        t.GetAwaiter().GetResult();
    }
}
