using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.ShowCase.Swift;
using NTokenizers.Extensions.Spectre.Console.Styles;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleSwiftExtensions
var swiftString = SwiftExample.GetSampleSwift();

var customSwiftStyles = SwiftStyles.Default;
customSwiftStyles.Keyword = new Style(Color.Orange1);

// Method 1: WriteSwift with string (default styles)
Console.WriteLine("=== WriteSwift with string (default styles) ===");
AnsiConsole.Console.WriteSwift(swiftString);

// Method 2: WriteSwift with string and custom styles
Console.WriteLine("\n=== WriteSwift with string and custom styles ===");
AnsiConsole.Console.WriteSwift(swiftString, customSwiftStyles);

// Method 3: WriteSwift with Stream (default styles)
Console.WriteLine("\n=== WriteSwift with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(swiftString);
AnsiConsole.Console.WriteSwift(stream);
await writerTask;

// Method 4: WriteSwift with Stream and custom styles
Console.WriteLine("\n=== WriteSwift with Stream and custom styles ===");
(writerTask, stream) = SetupStream(swiftString);
AnsiConsole.Console.WriteSwift(stream, customSwiftStyles);
await writerTask;

// Method 5: WriteSwiftAsync with string (default styles)
Console.WriteLine("\n=== WriteSwiftAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(swiftString);
await AnsiConsole.Console.WriteSwiftAsync(stream);
await writerTask;

// Method 6: WriteSwiftAsync with string and custom styles
Console.WriteLine("\n=== WriteSwiftAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(swiftString);
await AnsiConsole.Console.WriteSwiftAsync(stream, customSwiftStyles);
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
