using NTokenizers.Typescript;
using Spectre.Console.Extensions.NTokenizers.Styles;
using Spectre.Console.Extensions.NTokenizers.Writers;
using System.Text;

namespace Spectre.Console.Extensions.NTokenizers;
public static class AnsiConsoleTypescriptExtensions
{
    public static async Task WriteTypescriptAsync(this IAnsiConsole ansiConsole, Stream stream) =>
        await WriteTypescriptAsync(ansiConsole, stream, TypescriptStyles.Default);

    public static async Task WriteTypescriptAsync(this IAnsiConsole ansiConsole, Stream stream, TypescriptStyles typescriptStyles)
    {
        var typescriptWriter = new TypescriptWriter(ansiConsole, typescriptStyles);
        await TypescriptTokenizer.Create().ParseAsync(
            stream,
            typescriptWriter.WriteToken
        );
    }

    public static void WriteTypescript(this IAnsiConsole ansiConsole, Stream stream) =>
        WriteTypescript(ansiConsole, stream, TypescriptStyles.Default);

    public static void WriteTypescript(this IAnsiConsole ansiConsole, Stream stream, TypescriptStyles typescriptStyles)
    {
        var t = Task.Run(() => WriteTypescriptAsync(ansiConsole, stream, typescriptStyles));
        t.GetAwaiter().GetResult();
    }

    public static void WriteTypescript(this IAnsiConsole ansiConsole, string value) =>
        WriteTypescript(ansiConsole, value, TypescriptStyles.Default);

    public static void WriteTypescript(this IAnsiConsole ansiConsole, string value, TypescriptStyles typescriptStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteTypescriptAsync(ansiConsole, stream, typescriptStyles));
        t.GetAwaiter().GetResult();
    }
}
