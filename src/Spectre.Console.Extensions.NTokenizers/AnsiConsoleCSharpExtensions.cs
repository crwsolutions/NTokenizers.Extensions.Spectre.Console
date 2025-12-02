using NTokenizers.CSharp;
using Spectre.Console.Extensions.NTokenizers.Styles;
using Spectre.Console.Extensions.NTokenizers.Writers;
using System.Text;

namespace Spectre.Console.Extensions.NTokenizers;
public static class AnsiConsoleCSharpExtensions
{
    public static async Task WriteCSharpAsync(this IAnsiConsole ansiConsole, Stream stream) =>
        await WriteCSharpAsync(ansiConsole, stream, CSharpStyles.Default);

    public static async Task WriteCSharpAsync(this IAnsiConsole ansiConsole, Stream stream, CSharpStyles csharpStyles)
    {
        var csharpWriter = new CSharpWriter(ansiConsole, csharpStyles);
        await CSharpTokenizer.Create().ParseAsync(
            stream,
            csharpWriter.WriteToken
        );
    }

    public static void WriteCSharp(this IAnsiConsole ansiConsole, Stream stream) =>
        WriteCSharp(ansiConsole, stream, CSharpStyles.Default);

    public static void WriteCSharp(this IAnsiConsole ansiConsole, Stream stream, CSharpStyles csharpStyles)
    {
        var t = Task.Run(() => WriteCSharpAsync(ansiConsole, stream, csharpStyles));
        t.GetAwaiter().GetResult();
    }

    public static void WriteCSharp(this IAnsiConsole ansiConsole, string value) =>
        WriteCSharp(ansiConsole, value, CSharpStyles.Default);

    public static void WriteCSharp(this IAnsiConsole ansiConsole, string value, CSharpStyles csharpStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteCSharpAsync(ansiConsole, stream, csharpStyles));
        t.GetAwaiter().GetResult();
    }
}
