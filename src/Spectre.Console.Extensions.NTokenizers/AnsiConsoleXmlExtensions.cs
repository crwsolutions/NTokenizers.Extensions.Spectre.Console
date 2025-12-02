using NTokenizers.Xml;
using Spectre.Console.Extensions.NTokenizers.Styles;
using Spectre.Console.Extensions.NTokenizers.Writers;
using System.Text;

namespace Spectre.Console.Extensions.NTokenizers;
public static class AnsiConsoleXmlExtensions
{
    public static async Task WriteXmlAsync(this IAnsiConsole ansiConsole, Stream stream) => 
        await WriteXmlAsync(ansiConsole, stream, XmlStyles.Default);

    public static async Task WriteXmlAsync(this IAnsiConsole ansiConsole, Stream stream, XmlStyles xmlStyles)
    {
        var xmlWriter = new XmlWriter(ansiConsole, xmlStyles);
        await XmlTokenizer.Create().ParseAsync(
            stream,
            xmlWriter.WriteToken
        );
    }

    public static void WriteXml(this IAnsiConsole ansiConsole, Stream stream) => 
        WriteXml(ansiConsole, stream, XmlStyles.Default);

    public static void WriteXml(this IAnsiConsole ansiConsole, Stream stream, XmlStyles xmlStyles)
    {
        var t = Task.Run(() => WriteXmlAsync(ansiConsole, stream, xmlStyles));
        t.GetAwaiter().GetResult();
    }

    public static void WriteXml(this IAnsiConsole ansiConsole, string value) =>
        WriteXml(ansiConsole, value, XmlStyles.Default);

    public static void WriteXml(this IAnsiConsole ansiConsole, string value, XmlStyles xmlStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteXmlAsync(ansiConsole, stream, xmlStyles));
        t.GetAwaiter().GetResult();
    }
}