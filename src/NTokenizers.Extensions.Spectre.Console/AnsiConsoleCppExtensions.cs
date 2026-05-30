using NTokenizers.Cpp;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using Spectre.Console;
using System.Text;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render C++ code with syntax highlighting.
/// </summary>
public static class AnsiConsoleCppExtensions
{
    /// <summary>
    /// Writes C++ code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing C++ code to render.</param>
    /// <param name="cppStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation with the parsed string.</returns>
    public static async Task<string> WriteCppAsync(
        this IAnsiConsole ansiConsole, Stream stream,
        CppStyles? cppStyles = null, Encoding? encoding = null,
        CancellationToken ct = default)
    {
        var cppWriter = new CppWriter(ansiConsole, cppStyles ?? CppStyles.Default);
        if (encoding is null)
        {
            return await CppTokenizer.Create().ParseAsync(
                stream, ct, cppWriter.WriteToken);
        }
        else
        {
            return await CppTokenizer.Create().ParseAsync(
                stream, encoding, ct, cppWriter.WriteToken);
        }
    }

    /// <summary>
    /// Writes C++ code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing C++ code to render.</param>
    /// <param name="cppStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The parsed string.</returns>
    public static string WriteCpp(this IAnsiConsole ansiConsole, Stream stream,
        CppStyles? cppStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteCppAsync(ansiConsole, stream, cppStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes C++ code to the console with syntax highlighting using default styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The C++ code to render.</param>
    public static void WriteCpp(this IAnsiConsole ansiConsole, string value) =>
        WriteCpp(ansiConsole, value, CppStyles.Default);

    /// <summary>
    /// Writes C++ code to the console with syntax highlighting using the specified styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The C++ code to render.</param>
    /// <param name="cppStyles">The styles to use for syntax highlighting.</param>
    public static void WriteCpp(this IAnsiConsole ansiConsole, string value, CppStyles cppStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteCppAsync(ansiConsole, stream, cppStyles, Encoding.UTF8, default));
        t.GetAwaiter().GetResult();
    }
}
