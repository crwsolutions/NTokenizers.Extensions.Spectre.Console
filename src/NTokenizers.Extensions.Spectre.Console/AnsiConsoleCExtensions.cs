using NTokenizers.C;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using Spectre.Console;
using System.Text;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render C code with syntax highlighting.
/// </summary>
public static class AnsiConsoleCExtensions
{
    /// <summary>
    /// Writes C code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing C code to render.</param>
    /// <param name="cStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation with the parsed string.</returns>
    public static async Task<string> WriteCAsync(
        this IAnsiConsole ansiConsole, Stream stream,
        CStyles? cStyles = null, Encoding? encoding = null,
        CancellationToken ct = default)
    {
        var cWriter = new CWriter(ansiConsole, cStyles ?? CStyles.Default);
        if (encoding is null)
        {
            return await CTokenizer.Create().ParseAsync(
                stream, ct, cWriter.WriteToken);
        }
        else
        {
            return await CTokenizer.Create().ParseAsync(
                stream, encoding, ct, cWriter.WriteToken);
        }
    }

    /// <summary>
    /// Writes C code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing C code to render.</param>
    /// <param name="cStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The parsed string.</returns>
    public static string WriteC(this IAnsiConsole ansiConsole, Stream stream,
        CStyles? cStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteCAsync(ansiConsole, stream, cStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes C code to the console with syntax highlighting using default styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The C code to render.</param>
    public static void WriteC(this IAnsiConsole ansiConsole, string value) =>
        WriteC(ansiConsole, value, CStyles.Default);

    /// <summary>
    /// Writes C code to the console with syntax highlighting using the specified styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The C code to render.</param>
    /// <param name="cStyles">The styles to use for syntax highlighting.</param>
    public static void WriteC(this IAnsiConsole ansiConsole, string value, CStyles cStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteCAsync(ansiConsole, stream, cStyles, Encoding.UTF8, default));
        t.GetAwaiter().GetResult();
    }
}
