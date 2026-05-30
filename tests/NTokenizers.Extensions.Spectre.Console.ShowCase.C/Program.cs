using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.ShowCase.C;
using NTokenizers.Extensions.Spectre.Console.Styles;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleCExtensions
var cString = CExample.GetSampleC();

var customCStyles = CStyles.Default;
customCStyles.Keyword = new Style(Color.Orange1);

// Method 1: WriteC with string (default styles)
Console.WriteLine("=== WriteC with string (default styles) ===");
AnsiConsole.Console.WriteC(cString);

// Method 2: WriteC with string and custom styles
Console.WriteLine("\n=== WriteC with string and custom styles ===");
AnsiConsole.Console.WriteC(cString, customCStyles);

// Method 3: WriteC with Stream (default styles)
Console.WriteLine("\n=== WriteC with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(cString);
AnsiConsole.Console.WriteC(stream);
await writerTask;

// Method 4: WriteC with Stream and custom styles
Console.WriteLine("\n=== WriteC with Stream and custom styles ===");
(writerTask, stream) = SetupStream(cString);
AnsiConsole.Console.WriteC(stream, customCStyles);
await writerTask;

// Method 5: WriteCAsync with string (default styles)
Console.WriteLine("\n=== WriteCAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(cString);
await AnsiConsole.Console.WriteCAsync(stream);
await writerTask;

// Method 6: WriteCAsync with string and custom styles
Console.WriteLine("\n=== WriteCAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(cString);
await AnsiConsole.Console.WriteCAsync(stream, customCStyles);
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
