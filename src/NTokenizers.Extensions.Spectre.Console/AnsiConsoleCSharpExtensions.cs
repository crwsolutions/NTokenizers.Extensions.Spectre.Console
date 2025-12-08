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
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation with the parsed string.</returns>
    public static async Task<string> WriteCSharpAsync(this IAnsiConsole ansiConsole, Stream stream, CSharpStyles? csharpStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var csharpWriter = new CSharpWriter(ansiConsole, csharpStyles ?? CSharpStyles.Default);
        if (encoding is null)
        {
            // Call overload without encoding to preserve BOM detection
            return await CSharpTokenizer.Create().ParseAsync(
                stream,
                ct,
                csharpWriter.WriteToken
            );
        }
        else
        {
            // Call overload with encoding and cancellation token
            return await CSharpTokenizer.Create().ParseAsync(
                stream,
                encoding,
                ct,
                csharpWriter.WriteToken
            );
        }
    }

    /// <summary>
    /// Writes C# code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing C# code to render.</param>
    /// <param name="csharpStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The parsed string.</returns>
    public static string WriteCSharp(this IAnsiConsole ansiConsole, Stream stream, CSharpStyles? csharpStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteCSharpAsync(ansiConsole, stream, csharpStyles, encoding, ct), ct);
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
        var t = Task.Run(() => WriteCSharpAsync(ansiConsole, stream, csharpStyles, null, default));
        t.GetAwaiter().GetResult();
    }
}
