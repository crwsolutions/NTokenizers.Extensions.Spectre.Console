using NTokenizers.Python;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using Spectre.Console;
using System.Text;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render Python code with syntax highlighting.
/// </summary>
public static class AnsiConsolePythonExtensions
{
    /// <summary>
    /// Writes Python code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing Python code to render.</param>
    /// <param name="pythonStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation with the parsed string.</returns>
    public static async Task<string> WritePythonAsync(
        this IAnsiConsole ansiConsole, Stream stream,
        PythonStyles? pythonStyles = null, Encoding? encoding = null,
        CancellationToken ct = default)
    {
        var pythonWriter = new PythonWriter(ansiConsole, pythonStyles ?? PythonStyles.Default);
        if (encoding is null)
        {
            return await PythonTokenizer.Create().ParseAsync(
                stream, ct, pythonWriter.WriteToken);
        }
        else
        {
            return await PythonTokenizer.Create().ParseAsync(
                stream, encoding, ct, pythonWriter.WriteToken);
        }
    }

    /// <summary>
    /// Writes Python code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing Python code to render.</param>
    /// <param name="pythonStyles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The parsed string.</returns>
    public static string WritePython(this IAnsiConsole ansiConsole, Stream stream,
        PythonStyles? pythonStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WritePythonAsync(ansiConsole, stream, pythonStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes Python code to the console with syntax highlighting using default styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The Python code to render.</param>
    public static void WritePython(this IAnsiConsole ansiConsole, string value) =>
        WritePython(ansiConsole, value, PythonStyles.Default);

    /// <summary>
    /// Writes Python code to the console with syntax highlighting using the specified styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The Python code to render.</param>
    /// <param name="pythonStyles">The styles to use for syntax highlighting.</param>
    public static void WritePython(this IAnsiConsole ansiConsole, string value, PythonStyles pythonStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WritePythonAsync(ansiConsole, stream, pythonStyles, Encoding.UTF8, default));
        t.GetAwaiter().GetResult();
    }
}
