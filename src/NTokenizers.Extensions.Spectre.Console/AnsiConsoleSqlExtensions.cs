using NTokenizers.Sql;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using System.Text;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render SQL syntax-highlighted output.
/// </summary>
/// <remarks>
public static class AnsiConsoleSqlExtensions
{
    /// <summary>
    /// Writes SQL content from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="stream">The stream containing SQL content.</param>
    /// <param name="sqlStyles">The SQL styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task<string> WriteSqlAsync(this IAnsiConsole ansiConsole, Stream stream, SqlStyles? sqlStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var sqlWriter = new SqlWriter(ansiConsole, sqlStyles ?? SqlStyles.Default);
        if (encoding is null)
        {
            // Call overload without encoding to preserve BOM detection
            return await SqlTokenizer.Create().ParseAsync(
                stream,
                ct,
                sqlWriter.WriteToken
            );
        }
        else
        {
            // Call overload with encoding and cancellation token
            return await SqlTokenizer.Create().ParseAsync(
                stream,
                encoding,
                ct,
                sqlWriter.WriteToken
            );
        }
    }

    /// <summary>
    /// Writes SQL content from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="stream">The stream containing SQL content.</param>
    /// <param name="sqlStyles">The SQL styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The processed SQL content as a string.</returns>
    public static string WriteSql(this IAnsiConsole ansiConsole, Stream stream, SqlStyles? sqlStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteSqlAsync(ansiConsole, stream, sqlStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes SQL content from a string to the console with default styling.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="value">The SQL content as a string.</param>
    public static void WriteSql(this IAnsiConsole ansiConsole, string value) =>
        WriteSql(ansiConsole, value, SqlStyles.Default);

    /// <summary>
    /// Writes SQL content from a string to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="value">The SQL content as a string.</param>
    /// <param name="sqlStyles">The SQL styles to use for syntax highlighting.</param>
    public static void WriteSql(this IAnsiConsole ansiConsole, string value, SqlStyles sqlStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteSqlAsync(ansiConsole, stream, sqlStyles, null, default));
        t.GetAwaiter().GetResult();
    }
}