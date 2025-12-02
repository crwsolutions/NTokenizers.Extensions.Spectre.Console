using NTokenizers.Xml;
using Spectre.Console.Extensions.NTokenizers.Styles;
using Spectre.Console.Extensions.NTokenizers.Writers;
using System.Text;

namespace Spectre.Console.Extensions.NTokenizers;
/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render XML content with syntax highlighting.
/// </summary>
/// <remarks>
/// This class offers methods to display XML content in the console with rich styling and syntax highlighting,
/// leveraging the NTokenizers library for parsing and Spectre.Console for rendering.
/// </remarks>
public static class AnsiConsoleXmlExtensions
{
    /// <summary>
    /// Writes XML content from a stream to the console with default styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The <see cref="Stream"/> containing the XML content.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task WriteXmlAsync(this IAnsiConsole ansiConsole, Stream stream) => 
        await WriteXmlAsync(ansiConsole, stream, XmlStyles.Default);

    /// <summary>
    /// Writes XML content from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The <see cref="Stream"/> containing the XML content.</param>
    /// <param name="xmlStyles">The <see cref="XmlStyles"/> to use for styling the output.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task WriteXmlAsync(this IAnsiConsole ansiConsole, Stream stream, XmlStyles xmlStyles)
    {
        var xmlWriter = new XmlWriter(ansiConsole, xmlStyles);
        await XmlTokenizer.Create().ParseAsync(
            stream,
            xmlWriter.WriteToken
        );
    }

    /// <summary>
    /// Writes XML content from a stream to the console with default styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The <see cref="Stream"/> containing the XML content.</param>
    public static void WriteXml(this IAnsiConsole ansiConsole, Stream stream) => 
        WriteXml(ansiConsole, stream, XmlStyles.Default);

    /// <summary>
    /// Writes XML content from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The <see cref="Stream"/> containing the XML content.</param>
    /// <param name="xmlStyles">The <see cref="XmlStyles"/> to use for styling the output.</param>
    public static void WriteXml(this IAnsiConsole ansiConsole, Stream stream, XmlStyles xmlStyles)
    {
        var t = Task.Run(() => WriteXmlAsync(ansiConsole, stream, xmlStyles));
        t.GetAwaiter().GetResult();
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
        var t = Task.Run(() => WriteXmlAsync(ansiConsole, stream, xmlStyles));
        t.GetAwaiter().GetResult();
    }
}