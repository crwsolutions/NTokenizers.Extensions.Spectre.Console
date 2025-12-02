using Spectre.Console;
using Spectre.Console.Extensions.NTokenizers;
using Spectre.Console.Extensions.NTokenizers.ShowCase.CSharp;
using Spectre.Console.Extensions.NTokenizers.Styles;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleCSharpExtensions
var csharpString = CSharpExample.GetSampleCSharp();

var customCSharpStyles = CSharpStyles.Default;
customCSharpStyles.Keyword = new Style(Color.Orange1);

// Method 1: WriteCSharp with string (default styles)
Console.WriteLine("=== WriteCSharp with string (default styles) ===");
AnsiConsole.Console.WriteCSharp(csharpString);

// Method 2: WriteCSharp with string and custom styles
Console.WriteLine("\n=== WriteCSharp with string and custom styles ===");
AnsiConsole.Console.WriteCSharp(csharpString, customCSharpStyles);

// Method 3: WriteCSharp with Stream (default styles)
Console.WriteLine("\n=== WriteCSharp with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(csharpString);
AnsiConsole.Console.WriteCSharp(stream);
await writerTask;

// Method 4: WriteCSharp with Stream and custom styles
Console.WriteLine("\n=== WriteCSharp with Stream and custom styles ===");
(writerTask, stream) = SetupStream(csharpString);
AnsiConsole.Console.WriteCSharp(stream, customCSharpStyles);
await writerTask;

// Method 5: WriteCSharpAsync with string (default styles)
Console.WriteLine("\n=== WriteCSharpAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(csharpString);
await AnsiConsole.Console.WriteCSharpAsync(stream);
await writerTask;

// Method 6: WriteCSharpAsync with string and custom styles
Console.WriteLine("\n=== WriteCSharpAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(csharpString);
await AnsiConsole.Console.WriteCSharpAsync(stream, customCSharpStyles);
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
