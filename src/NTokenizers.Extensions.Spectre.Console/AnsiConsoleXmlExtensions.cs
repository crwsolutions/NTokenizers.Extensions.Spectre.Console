using NTokenizers.Xml;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using System.Text;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render XML content with syntax highlighting.
/// </summary>
public static class AnsiConsoleXmlExtensions
{
    /// <summary>
    /// Writes XML content from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The <see cref="Stream"/> containing the XML content.</param>
    /// <param name="xmlStyles">The <see cref="XmlStyles"/> to use for styling the output.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The result is the input stream as a string.</returns>
    public static async Task<string> WriteXmlAsync(this IAnsiConsole ansiConsole, Stream stream, XmlStyles? xmlStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var xmlWriter = new XmlWriter(ansiConsole, xmlStyles ?? XmlStyles.Default);
        if (encoding is null)
        {
            // Call overload without encoding to preserve BOM detection
            return await XmlTokenizer.Create().ParseAsync(
                stream,
                ct,
                xmlWriter.WriteToken
            );
        }
        else
        {
            // Call overload with encoding and cancellation token
            return await XmlTokenizer.Create().ParseAsync(
                stream,
                encoding,
                ct,
                xmlWriter.WriteToken
            );
        }
    }

    /// <summary>
    /// Writes XML content from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The <see cref="Stream"/> containing the XML content.</param>
    /// <param name="xmlStyles">The <see cref="XmlStyles"/> to use for styling the output.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The input stream as a string.</returns>
    public static string WriteXml(this IAnsiConsole ansiConsole, Stream stream, XmlStyles? xmlStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteXmlAsync(ansiConsole, stream, xmlStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes XML content from a string to the console with default styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The XML content as a string.</param>
    public static void WriteXml(this IAnsiConsole ansiConsole, string value) =>
        WriteXml(ansiConsole, value, XmlStyles.Default);

    /// <summary>
    /// Writes XML content from a string to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The XML content as a string.</param>
    /// <param name="xmlStyles">The <see cref="XmlStyles"/> to use for styling the output.</param>
    public static void WriteXml(this IAnsiConsole ansiConsole, string value, XmlStyles xmlStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteXmlAsync(ansiConsole, stream, xmlStyles, null, default));
        t.GetAwaiter().GetResult();
    }
}