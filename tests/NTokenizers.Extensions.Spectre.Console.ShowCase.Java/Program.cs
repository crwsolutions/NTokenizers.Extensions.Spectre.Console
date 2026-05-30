using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.ShowCase.Java;
using NTokenizers.Extensions.Spectre.Console.Styles;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleJavaExtensions
var javaString = JavaExample.GetSampleJava();

var customJavaStyles = JavaStyles.Default;
customJavaStyles.Keyword = new Style(Color.Orange1);

// Method 1: WriteJava with string (default styles)
Console.WriteLine("=== WriteJava with string (default styles) ===");
AnsiConsole.Console.WriteJava(javaString);

// Method 2: WriteJava with string and custom styles
Console.WriteLine("\n=== WriteJava with string and custom styles ===");
AnsiConsole.Console.WriteJava(javaString, customJavaStyles);

// Method 3: WriteJava with Stream (default styles)
Console.WriteLine("\n=== WriteJava with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(javaString);
AnsiConsole.Console.WriteJava(stream);
await writerTask;

// Method 4: WriteJava with Stream and custom styles
Console.WriteLine("\n=== WriteJava with Stream and custom styles ===");
(writerTask, stream) = SetupStream(javaString);
AnsiConsole.Console.WriteJava(stream, customJavaStyles);
await writerTask;

// Method 5: WriteJavaAsync with string (default styles)
Console.WriteLine("\n=== WriteJavaAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(javaString);
await AnsiConsole.Console.WriteJavaAsync(stream);
await writerTask;

// Method 6: WriteJavaAsync with string and custom styles
Console.WriteLine("\n=== WriteJavaAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(javaString);
await AnsiConsole.Console.WriteJavaAsync(stream, customJavaStyles);
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
