using Spectre.Console;
using Spectre.Console.Extensions.NTokenizers;
using Spectre.Console.Extensions.NTokenizers.ShowCase.Sql;
using Spectre.Console.Extensions.NTokenizers.Styles;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleSqlExtensions
var sqlString = SqlExample.GetSampleSql();

var customSqlStyles = SqlStyles.Default;
customSqlStyles.Keyword = new Style(Color.Orange1);

// Method 1: WriteSql with string (default styles)
Console.WriteLine("=== WriteSql with string (default styles) ===");
AnsiConsole.Console.WriteSql(sqlString);

// Method 2: WriteSql with string and custom styles
Console.WriteLine("\n=== WriteSql with string and custom styles ===");
AnsiConsole.Console.WriteSql(sqlString, customSqlStyles);

// Method 3: WriteSql with Stream (default styles)
Console.WriteLine("\n=== WriteSql with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(sqlString);
AnsiConsole.Console.WriteSql(stream);
await writerTask;

// Method 4: WriteSql with Stream and custom styles
Console.WriteLine("\n=== WriteSql with Stream and custom styles ===");
(writerTask, stream) = SetupStream(sqlString);
AnsiConsole.Console.WriteSql(stream, customSqlStyles);
await writerTask;

// Method 5: WriteSqlAsync with string (default styles)
Console.WriteLine("\n=== WriteSqlAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(sqlString);
await AnsiConsole.Console.WriteSqlAsync(stream);
await writerTask;

// Method 6: WriteSqlAsync with string and custom styles
Console.WriteLine("\n=== WriteSqlAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(sqlString);
await AnsiConsole.Console.WriteSqlAsync(stream, customSqlStyles);
await writerTask;

Console.WriteLine("\nDone.");

static (Task writerTask, AnonymousPipeClientStream reader) SetupStream(string sampleString)
{
    var pipe = new AnonymousPipeServerStream(PipeDirection.Out);
    var reader = new AnonymousPipeClientStream(PipeDirection.In, pipe.ClientSafePipeHandle);
    var writerTask = Task.Run(async () =>
    {
        var rng = new Random();
        byte[] bytes = Encoding.UTF8.GetBytes(sampleString);
        foreach (var b in bytes)
        {
            await pipe.WriteAsync(new[] { b }.AsMemory(0, 1));
            await pipe.FlushAsync();
            await Task.Delay(rng.Next(0, 2));
        }

        pipe.Close();
    });
    return (writerTask, reader);
}
