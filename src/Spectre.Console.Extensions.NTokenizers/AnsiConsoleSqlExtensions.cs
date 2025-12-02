using NTokenizers.Sql;
using Spectre.Console.Extensions.NTokenizers.Styles;
using Spectre.Console.Extensions.NTokenizers.Writers;
using System.Text;

namespace Spectre.Console.Extensions.NTokenizers;
public static class AnsiConsoleSqlExtensions
{
    public static async Task WriteSqlAsync(this IAnsiConsole ansiConsole, Stream stream) =>
        await WriteSqlAsync(ansiConsole, stream, SqlStyles.Default);

    public static async Task WriteSqlAsync(this IAnsiConsole ansiConsole, Stream stream, SqlStyles sqlStyles)
    {
        var sqlWriter = new SqlWriter(ansiConsole, sqlStyles);
        await SqlTokenizer.Create().ParseAsync(
            stream,
            sqlWriter.WriteToken
        );
    }

    public static void WriteSql(this IAnsiConsole ansiConsole, Stream stream) =>
        WriteSql(ansiConsole, stream, SqlStyles.Default);

    public static void WriteSql(this IAnsiConsole ansiConsole, Stream stream, SqlStyles sqlStyles)
    {
        var t = Task.Run(() => WriteSqlAsync(ansiConsole, stream, sqlStyles));
        t.GetAwaiter().GetResult();
    }

    public static void WriteSql(this IAnsiConsole ansiConsole, string value) =>
        WriteSql(ansiConsole, value, SqlStyles.Default);

    public static void WriteSql(this IAnsiConsole ansiConsole, string value, SqlStyles sqlStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteSqlAsync(ansiConsole, stream, sqlStyles));
        t.GetAwaiter().GetResult();
    }
}
