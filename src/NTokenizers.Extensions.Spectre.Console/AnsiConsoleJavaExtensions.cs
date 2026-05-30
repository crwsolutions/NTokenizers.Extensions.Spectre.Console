using NTokenizers.Java;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using Spectre.Console;
using System.Text;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render Java code with syntax highlighting.
/// </summary>
public static class AnsiConsoleJavaExtensions
{
    /// <summary>
    /// Writes Java code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing Java code to render.</param>
    /// <param name="javaStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation with the parsed string.</returns>
    public static async Task<string> WriteJavaAsync(
        this IAnsiConsole ansiConsole, Stream stream,
        JavaStyles? javaStyles = null, Encoding? encoding = null,
        CancellationToken ct = default)
    {
        var javaWriter = new JavaWriter(ansiConsole, javaStyles ?? JavaStyles.Default);
        if (encoding is null)
        {
            return await JavaTokenizer.Create().ParseAsync(
                stream, ct, javaWriter.WriteToken);
        }
        else
        {
            return await JavaTokenizer.Create().ParseAsync(
                stream, encoding, ct, javaWriter.WriteToken);
        }
    }

    /// <summary>
    /// Writes Java code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing Java code to render.</param>
    /// <param name="javaStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The parsed string.</returns>
    public static string WriteJava(this IAnsiConsole ansiConsole, Stream stream,
        JavaStyles? javaStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteJavaAsync(ansiConsole, stream, javaStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes Java code to the console with syntax highlighting using default styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The Java code to render.</param>
    public static void WriteJava(this IAnsiConsole ansiConsole, string value) =>
        WriteJava(ansiConsole, value, JavaStyles.Default);

    /// <summary>
    /// Writes Java code to the console with syntax highlighting using the specified styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The Java code to render.</param>
    /// <param name="javaStyles">The styles to use for syntax highlighting.</param>
    public static void WriteJava(this IAnsiConsole ansiConsole, string value, JavaStyles javaStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteJavaAsync(ansiConsole, stream, javaStyles, Encoding.UTF8, default));
        t.GetAwaiter().GetResult();
    }
}
