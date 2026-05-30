using NTokenizers.Swift;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using Spectre.Console;
using System.Text;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render Swift code with syntax highlighting.
/// </summary>
public static class AnsiConsoleSwiftExtensions
{
    /// <summary>
    /// Writes Swift code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing Swift code to render.</param>
    /// <param name="swiftStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation with the parsed string.</returns>
    public static async Task<string> WriteSwiftAsync(
        this IAnsiConsole ansiConsole, Stream stream,
        SwiftStyles? swiftStyles = null, Encoding? encoding = null,
        CancellationToken ct = default)
    {
        var swiftWriter = new SwiftWriter(ansiConsole, swiftStyles ?? SwiftStyles.Default);
        if (encoding is null)
        {
            return await SwiftTokenizer.Create().ParseAsync(
                stream, ct, swiftWriter.WriteToken);
        }
        else
        {
            return await SwiftTokenizer.Create().ParseAsync(
                stream, encoding, ct, swiftWriter.WriteToken);
        }
    }

    /// <summary>
    /// Writes Swift code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing Swift code to render.</param>
    /// <param name="swiftStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The parsed string.</returns>
    public static string WriteSwift(this IAnsiConsole ansiConsole, Stream stream,
        SwiftStyles? swiftStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteSwiftAsync(ansiConsole, stream, swiftStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes Swift code to the console with syntax highlighting using default styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The Swift code to render.</param>
    public static void WriteSwift(this IAnsiConsole ansiConsole, string value) =>
        WriteSwift(ansiConsole, value, SwiftStyles.Default);

    /// <summary>
    /// Writes Swift code to the console with syntax highlighting using the specified styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The Swift code to render.</param>
    /// <param name="swiftStyles">The styles to use for syntax highlighting.</param>
    public static void WriteSwift(this IAnsiConsole ansiConsole, string value, SwiftStyles swiftStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteSwiftAsync(ansiConsole, stream, swiftStyles, Encoding.UTF8, default));
        t.GetAwaiter().GetResult();
    }
}
