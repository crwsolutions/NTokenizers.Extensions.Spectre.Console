using NTokenizers.Rust;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using Spectre.Console;
using System.Text;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render Rust code with syntax highlighting.
/// </summary>
public static class AnsiConsoleRustExtensions
{
    /// <summary>
    /// Writes Rust code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing Rust code to render.</param>
    /// <param name="rustStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation with the parsed string.</returns>
    public static async Task<string> WriteRustAsync(
        this IAnsiConsole ansiConsole, Stream stream,
        RustStyles? rustStyles = null, Encoding? encoding = null,
        CancellationToken ct = default)
    {
        var rustWriter = new RustWriter(ansiConsole, rustStyles ?? RustStyles.Default);
        if (encoding is null)
        {
            return await RustTokenizer.Create().ParseAsync(
                stream, ct, rustWriter.WriteToken);
        }
        else
        {
            return await RustTokenizer.Create().ParseAsync(
                stream, encoding, ct, rustWriter.WriteToken);
        }
    }

    /// <summary>
    /// Writes Rust code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing Rust code to render.</param>
    /// <param name="rustStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The parsed string.</returns>
    public static string WriteRust(this IAnsiConsole ansiConsole, Stream stream,
        RustStyles? rustStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteRustAsync(ansiConsole, stream, rustStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes Rust code to the console with syntax highlighting using default styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The Rust code to render.</param>
    public static void WriteRust(this IAnsiConsole ansiConsole, string value) =>
        WriteRust(ansiConsole, value, RustStyles.Default);

    /// <summary>
    /// Writes Rust code to the console with syntax highlighting using the specified styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The Rust code to render.</param>
    /// <param name="rustStyles">The styles to use for syntax highlighting.</param>
    public static void WriteRust(this IAnsiConsole ansiConsole, string value, RustStyles rustStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteRustAsync(ansiConsole, stream, rustStyles, Encoding.UTF8, default));
        t.GetAwaiter().GetResult();
    }
}
