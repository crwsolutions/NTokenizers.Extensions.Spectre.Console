using NTokenizers.CSharp;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using Spectre.Console;
using System.Text;

namespace NTokenizers.Extensions.Spectre.Console;
/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render C# code with syntax highlighting.
/// </summary>
/// <remarks>
/// This library offers style-rich console syntax highlighting for various languages including C#, 
/// XML, JSON, Markup, TypeScript, and SQL using NTokenizers and Spectre.Console.
/// </remarks>
public static class AnsiConsoleCSharpExtensions
{
    /// <summary>
    /// Writes C# code to the console with syntax highlighting using default styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing C# code to render.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task WriteCSharpAsync(this IAnsiConsole ansiConsole, Stream stream) =>
        await WriteCSharpAsync(ansiConsole, stream, CSharpStyles.Default);

    /// <summary>
    /// Writes C# code to the console with syntax highlighting using the specified styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing C# code to render.</param>
    /// <param name="csharpStyles">The styles to use for syntax highlighting.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task WriteCSharpAsync(this IAnsiConsole ansiConsole, Stream stream, CSharpStyles csharpStyles)
    {
        var csharpWriter = new CSharpWriter(ansiConsole, csharpStyles);
        await CSharpTokenizer.Create().ParseAsync(
            stream,
            csharpWriter.WriteToken
        );
    }

    /// <summary>
    /// Writes C# code to the console with syntax highlighting using default styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing C# code to render.</param>
    public static void WriteCSharp(this IAnsiConsole ansiConsole, Stream stream) =>
        WriteCSharp(ansiConsole, stream, CSharpStyles.Default);

    /// <summary>
    /// Writes C# code to the console with syntax highlighting using the specified styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing C# code to render.</param>
    /// <param name="csharpStyles">The styles to use for syntax highlighting.</param>
    public static void WriteCSharp(this IAnsiConsole ansiConsole, Stream stream, CSharpStyles csharpStyles)
    {
        var t = Task.Run(() => WriteCSharpAsync(ansiConsole, stream, csharpStyles));
        t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes C# code to the console with syntax highlighting using default styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The C# code to render.</param>
    public static void WriteCSharp(this IAnsiConsole ansiConsole, string value) =>
        WriteCSharp(ansiConsole, value, CSharpStyles.Default);

    /// <summary>
    /// Writes C# code to the console with syntax highlighting using the specified styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The C# code to render.</param>
    /// <param name="csharpStyles">The styles to use for syntax highlighting.</param>
    public static void WriteCSharp(this IAnsiConsole ansiConsole, string value, CSharpStyles csharpStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteCSharpAsync(ansiConsole, stream, csharpStyles));
        t.GetAwaiter().GetResult();
    }
}
