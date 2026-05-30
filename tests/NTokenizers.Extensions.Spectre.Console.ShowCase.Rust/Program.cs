using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.ShowCase.Rust;
using NTokenizers.Extensions.Spectre.Console.Styles;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleRustExtensions
var rustString = RustExample.GetSampleRust();

var customRustStyles = RustStyles.Default;
customRustStyles.Keyword = new Style(Color.Orange1);

// Method 1: WriteRust with string (default styles)
Console.WriteLine("=== WriteRust with string (default styles) ===");
AnsiConsole.Console.WriteRust(rustString);

// Method 2: WriteRust with string and custom styles
Console.WriteLine("\n=== WriteRust with string and custom styles ===");
AnsiConsole.Console.WriteRust(rustString, customRustStyles);

// Method 3: WriteRust with Stream (default styles)
Console.WriteLine("\n=== WriteRust with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(rustString);
AnsiConsole.Console.WriteRust(stream);
await writerTask;

// Method 4: WriteRust with Stream and custom styles
Console.WriteLine("\n=== WriteRust with Stream and custom styles ===");
(writerTask, stream) = SetupStream(rustString);
AnsiConsole.Console.WriteRust(stream, customRustStyles);
await writerTask;

// Method 5: WriteRustAsync with string (default styles)
Console.WriteLine("\n=== WriteRustAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(rustString);
await AnsiConsole.Console.WriteRustAsync(stream);
await writerTask;

// Method 6: WriteRustAsync with string and custom styles
Console.WriteLine("\n=== WriteRustAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(rustString);
await AnsiConsole.Console.WriteRustAsync(stream, customRustStyles);
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
