using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.ShowCase.Toml;
using NTokenizers.Extensions.Spectre.Console.Styles;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleTomlExtensions
var tomlString = TomlExample.GetSampleToml();

var customTomlStyles = TomlStyles.Default;
customTomlStyles.Identifier = new Style(Color.Orange1);

// Method 1: WriteToml with string (default styles)
Console.WriteLine("=== WriteToml with string (default styles) ===");
AnsiConsole.Console.WriteToml(tomlString);

// Method 2: WriteToml with string and custom styles
Console.WriteLine("\n=== WriteToml with string and custom styles ===");
AnsiConsole.Console.WriteToml(tomlString, customTomlStyles);

// Method 3: WriteToml with Stream (default styles)
Console.WriteLine("\n=== WriteToml with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(tomlString);
AnsiConsole.Console.WriteToml(stream);
await writerTask;

// Method 4: WriteToml with Stream and custom styles
Console.WriteLine("\n=== WriteToml with Stream and custom styles ===");
(writerTask, stream) = SetupStream(tomlString);
AnsiConsole.Console.WriteToml(stream, customTomlStyles);
await writerTask;

// Method 5: WriteTomlAsync with string (default styles)
Console.WriteLine("\n=== WriteTomlAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(tomlString);
await AnsiConsole.Console.WriteTomlAsync(stream);
await writerTask;

// Method 6: WriteTomlAsync with string and custom styles
Console.WriteLine("\n=== WriteTomlAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(tomlString);
await AnsiConsole.Console.WriteTomlAsync(stream, customTomlStyles);
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
        pipe.Dispose();
    });
    return (writerTask, reader);
}
