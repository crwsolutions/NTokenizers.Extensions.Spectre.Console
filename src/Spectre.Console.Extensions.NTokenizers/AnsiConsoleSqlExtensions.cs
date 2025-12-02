using NTokenizers.Sql;
using Spectre.Console.Extensions.NTokenizers.Styles;
using Spectre.Console.Extensions.NTokenizers.Writers;
using System.Text;

namespace Spectre.Console.Extensions.NTokenizers;
/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render SQL syntax-highlighted output.
/// </summary>
/// <remarks>
/// This class offers methods to write SQL content with syntax highlighting to the console,
/// supporting both synchronous and asynchronous operations with customizable styling.
/// </remarks>
public static class AnsiConsoleSqlExtensions
{
    /// <summary>
    /// Writes SQL content from a stream to the console with default styling.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="stream">The stream containing SQL content.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task WriteSqlAsync(this IAnsiConsole ansiConsole, Stream stream) =>
        await WriteSqlAsync(ansiConsole, stream, SqlStyles.Default);

    /// <summary>
    /// Writes SQL content from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="stream">The stream containing SQL content.</param>
    /// <param name="sqlStyles">The SQL styles to use for syntax highlighting.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task WriteSqlAsync(this IAnsiConsole ansiConsole, Stream stream, SqlStyles sqlStyles)
    {
        var sqlWriter = new SqlWriter(ansiConsole, sqlStyles);
        await SqlTokenizer.Create().ParseAsync(
            stream,
            sqlWriter.WriteToken
        );
    }

    /// <summary>
    /// Writes SQL content from a stream to the console with default styling.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="stream">The stream containing SQL content.</param>
    public static void WriteSql(this IAnsiConsole ansiConsole, Stream stream) =>
        WriteSql(ansiConsole, stream, SqlStyles.Default);

    /// <summary>
    /// Writes SQL content from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The ANSI console to write to.</param>
    /// <param name="stream">The stream containing SQL content.</param>
    /// <param name="sqlStyles">The SQL styles to use for syntax highlighting.</param>
    public static void WriteSql(this IAnsiConsole ansiConsole, Stream stream, SqlStyles sqlStyles)
    {
        var t = Task.Run(() => WriteSqlAsync(ansiConsole, stream, sqlStyles));
        t.GetAwaiter().GetResult();
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
        var t = Task.Run(() => WriteSqlAsync(ansiConsole, stream, sqlStyles));
        t.GetAwaiter().GetResult();
    }
}