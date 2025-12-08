using NTokenizers.CSharp;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using Spectre.Console;
using System.Text;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render C# code with syntax highlighting.
/// </summary>
public static class AnsiConsoleCSharpExtensions
{
    /// <summary>
    /// Writes C# code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing C# code to render.</param>
    /// <param name="csharpStyles">The styles to use for syntax highlighting.</param>
    /// <returns>A task representing the asynchronous operation with the parsed string.</returns>
    public static async Task<string> WriteCSharpAsync(this IAnsiConsole ansiConsole, Stream stream, CSharpStyles? csharpStyles = null)
    {
        var csharpWriter = new CSharpWriter(ansiConsole, csharpStyles ?? CSharpStyles.Default);
        return await CSharpTokenizer.Create().ParseAsync(
            stream,
            csharpWriter.WriteToken
        );
    }

    /// <summary>
    /// Writes C# code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing C# code to render.</param>
    /// <param name="csharpStyles">The styles to use for syntax highlighting.</param>
    /// <returns>The parsed string.</returns>
    public static string WriteCSharp(this IAnsiConsole ansiConsole, Stream stream, CSharpStyles? csharpStyles = null)
    {
        var t = Task.Run(() => WriteCSharpAsync(ansiConsole, stream, csharpStyles));
        return t.GetAwaiter().GetResult();
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
