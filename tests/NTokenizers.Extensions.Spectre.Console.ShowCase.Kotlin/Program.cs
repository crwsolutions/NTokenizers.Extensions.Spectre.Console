using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.ShowCase.Kotlin;
using NTokenizers.Extensions.Spectre.Console.Styles;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleKotlinExtensions
var kotlinString = KotlinExample.GetSampleKotlin();

var customKotlinStyles = KotlinStyles.Default;
customKotlinStyles.Keyword = new Style(Color.Orange1);

// Method 1: WriteKotlin with string (default styles)
Console.WriteLine("=== WriteKotlin with string (default styles) ===");
AnsiConsole.Console.WriteKotlin(kotlinString);

// Method 2: WriteKotlin with string and custom styles
Console.WriteLine("\n=== WriteKotlin with string and custom styles ===");
AnsiConsole.Console.WriteKotlin(kotlinString, customKotlinStyles);

// Method 3: WriteKotlin with Stream (default styles)
Console.WriteLine("\n=== WriteKotlin with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(kotlinString);
AnsiConsole.Console.WriteKotlin(stream);
await writerTask;

// Method 4: WriteKotlin with Stream and custom styles
Console.WriteLine("\n=== WriteKotlin with Stream and custom styles ===");
(writerTask, stream) = SetupStream(kotlinString);
AnsiConsole.Console.WriteKotlin(stream, customKotlinStyles);
await writerTask;

// Method 5: WriteKotlinAsync with string (default styles)
Console.WriteLine("\n=== WriteKotlinAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(kotlinString);
await AnsiConsole.Console.WriteKotlinAsync(stream);
await writerTask;

// Method 6: WriteKotlinAsync with string and custom styles
Console.WriteLine("\n=== WriteKotlinAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(kotlinString);
await AnsiConsole.Console.WriteKotlinAsync(stream, customKotlinStyles);
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
