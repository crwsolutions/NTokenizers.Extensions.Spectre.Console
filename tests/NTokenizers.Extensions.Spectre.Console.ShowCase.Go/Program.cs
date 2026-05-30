using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.ShowCase.Go;
using NTokenizers.Extensions.Spectre.Console.Styles;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleGoExtensions
var goString = GoExample.GetSampleGo();

var customGoStyles = GoStyles.Default;
customGoStyles.Keyword = new Style(Color.Orange1);

// Method 1: WriteGo with string (default styles)
Console.WriteLine("=== WriteGo with string (default styles) ===");
AnsiConsole.Console.WriteGo(goString);

// Method 2: WriteGo with string and custom styles
Console.WriteLine("\n=== WriteGo with string and custom styles ===");
AnsiConsole.Console.WriteGo(goString, customGoStyles);

// Method 3: WriteGo with Stream (default styles)
Console.WriteLine("\n=== WriteGo with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(goString);
AnsiConsole.Console.WriteGo(stream);
await writerTask;

// Method 4: WriteGo with Stream and custom styles
Console.WriteLine("\n=== WriteGo with Stream and custom styles ===");
(writerTask, stream) = SetupStream(goString);
AnsiConsole.Console.WriteGo(stream, customGoStyles);
await writerTask;

// Method 5: WriteGoAsync with string (default styles)
Console.WriteLine("\n=== WriteGoAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(goString);
await AnsiConsole.Console.WriteGoAsync(stream);
await writerTask;

// Method 6: WriteGoAsync with string and custom styles
Console.WriteLine("\n=== WriteGoAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(goString);
await AnsiConsole.Console.WriteGoAsync(stream, customGoStyles);
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
