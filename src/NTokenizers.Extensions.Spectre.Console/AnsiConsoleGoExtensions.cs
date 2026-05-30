using NTokenizers.Go;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using Spectre.Console;
using System.Text;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render Go code with syntax highlighting.
/// </summary>
public static class AnsiConsoleGoExtensions
{
    /// <summary>
    /// Writes Go code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing Go code to render.</param>
    /// <param name="goStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation with the parsed string.</returns>
    public static async Task<string> WriteGoAsync(
        this IAnsiConsole ansiConsole, Stream stream,
        GoStyles? goStyles = null, Encoding? encoding = null,
        CancellationToken ct = default)
    {
        var goWriter = new GoWriter(ansiConsole, goStyles ?? GoStyles.Default);
        if (encoding is null)
        {
            return await GoTokenizer.Create().ParseAsync(
                stream, ct, goWriter.WriteToken);
        }
        else
        {
            return await GoTokenizer.Create().ParseAsync(
                stream, encoding, ct, goWriter.WriteToken);
        }
    }

    /// <summary>
    /// Writes Go code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing Go code to render.</param>
    /// <param name="goStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The parsed string.</returns>
    public static string WriteGo(this IAnsiConsole ansiConsole, Stream stream,
        GoStyles? goStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteGoAsync(ansiConsole, stream, goStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes Go code to the console with syntax highlighting using default styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The Go code to render.</param>
    public static void WriteGo(this IAnsiConsole ansiConsole, string value) =>
        WriteGo(ansiConsole, value, GoStyles.Default);

    /// <summary>
    /// Writes Go code to the console with syntax highlighting using the specified styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The Go code to render.</param>
    /// <param name="goStyles">The styles to use for syntax highlighting.</param>
    public static void WriteGo(this IAnsiConsole ansiConsole, string value, GoStyles goStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteGoAsync(ansiConsole, stream, goStyles, Encoding.UTF8, default));
        t.GetAwaiter().GetResult();
    }
}
