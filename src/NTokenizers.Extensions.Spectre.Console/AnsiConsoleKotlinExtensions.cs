using NTokenizers.Kotlin;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using Spectre.Console;
using System.Text;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render Kotlin code with syntax highlighting.
/// </summary>
public static class AnsiConsoleKotlinExtensions
{
    /// <summary>
    /// Writes Kotlin code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing Kotlin code to render.</param>
    /// <param name="kotlinStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation with the parsed string.</returns>
    public static async Task<string> WriteKotlinAsync(
        this IAnsiConsole ansiConsole, Stream stream,
        KotlinStyles? kotlinStyles = null, Encoding? encoding = null,
        CancellationToken ct = default)
    {
        var kotlinWriter = new KotlinWriter(ansiConsole, kotlinStyles ?? KotlinStyles.Default);
        if (encoding is null)
        {
            return await KotlinTokenizer.Create().ParseAsync(
                stream, ct, kotlinWriter.WriteToken);
        }
        else
        {
            return await KotlinTokenizer.Create().ParseAsync(
                stream, encoding, ct, kotlinWriter.WriteToken);
        }
    }

    /// <summary>
    /// Writes Kotlin code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing Kotlin code to render.</param>
    /// <param name="kotlinStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The parsed string.</returns>
    public static string WriteKotlin(this IAnsiConsole ansiConsole, Stream stream,
        KotlinStyles? kotlinStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteKotlinAsync(ansiConsole, stream, kotlinStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes Kotlin code to the console with syntax highlighting using default styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The Kotlin code to render.</param>
    public static void WriteKotlin(this IAnsiConsole ansiConsole, string value) =>
        WriteKotlin(ansiConsole, value, KotlinStyles.Default);

    /// <summary>
    /// Writes Kotlin code to the console with syntax highlighting using the specified styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The Kotlin code to render.</param>
    /// <param name="kotlinStyles">The styles to use for syntax highlighting.</param>
    public static void WriteKotlin(this IAnsiConsole ansiConsole, string value, KotlinStyles kotlinStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteKotlinAsync(ansiConsole, stream, kotlinStyles, Encoding.UTF8, default));
        t.GetAwaiter().GetResult();
    }
}
