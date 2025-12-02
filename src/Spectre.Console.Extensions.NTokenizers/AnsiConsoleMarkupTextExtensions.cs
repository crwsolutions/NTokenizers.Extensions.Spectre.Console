using NTokenizers.Markup;
using Spectre.Console.Extensions.NTokenizers.Styles;
using Spectre.Console.Extensions.NTokenizers.Writers;
using System.Text;

namespace Spectre.Console.Extensions.NTokenizers;
public static class AnsiConsoleMarkupTextExtensions
{
    public static async Task WriteMarkupTextAsync(this IAnsiConsole ansiConsole, Stream stream) => 
        await WriteMarkupTextAsync(ansiConsole, stream, MarkupStyles.Default);

    public static async Task WriteMarkupTextAsync(this IAnsiConsole ansiConsole, Stream stream, MarkupStyles markupStyles)
    {
        var markupWriter = MarkupWriter.Create(ansiConsole);
        MarkupWriter.MarkupStyles = markupStyles;
        await MarkupTokenizer.Create().ParseAsync(
            stream,
            async token => await markupWriter.WriteAsync(token)
        );
    }

    public static void WriteMarkupText(this IAnsiConsole ansiConsole, Stream stream) => 
        WriteMarkupText(ansiConsole, stream, MarkupStyles.Default);

    public static void WriteMarkupText(this IAnsiConsole ansiConsole, Stream stream, MarkupStyles markupStyles)
    {
        var t = Task.Run(() => WriteMarkupTextAsync(ansiConsole, stream));
        t.GetAwaiter().GetResult();
    }

    public static void WriteMarkupText(this IAnsiConsole ansiConsole, string value) =>
        WriteMarkupText(ansiConsole, value, MarkupStyles.Default);

    public static void WriteMarkupText(this IAnsiConsole ansiConsole, string value, MarkupStyles markupStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteMarkupTextAsync(ansiConsole, stream));
        t.GetAwaiter().GetResult();
    }
}

