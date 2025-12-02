using Spectre.Console;
using Spectre.Console.Extensions.NTokenizers;
using Spectre.Console.Extensions.NTokenizers.ShowCase.Typescript;
using Spectre.Console.Extensions.NTokenizers.Styles;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleTypescriptExtensions
var typescriptString = TypescriptExample.GetSampleTypescript();

var customTypescriptStyles = TypescriptStyles.Default;
customTypescriptStyles.Keyword = new Style(Color.Orange1);

// Method 1: WriteTypescript with string (default styles)
Console.WriteLine("=== WriteTypescript with string (default styles) ===");
AnsiConsole.Console.WriteTypescript(typescriptString);

// Method 2: WriteTypescript with string and custom styles
Console.WriteLine("\n=== WriteTypescript with string and custom styles ===");
AnsiConsole.Console.WriteTypescript(typescriptString, customTypescriptStyles);

// Method 3: WriteTypescript with Stream (default styles)
Console.WriteLine("\n=== WriteTypescript with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(typescriptString);
AnsiConsole.Console.WriteTypescript(stream);
await writerTask;

// Method 4: WriteTypescript with Stream and custom styles
Console.WriteLine("\n=== WriteTypescript with Stream and custom styles ===");
(writerTask, stream) = SetupStream(typescriptString);
AnsiConsole.Console.WriteTypescript(stream, customTypescriptStyles);
await writerTask;

// Method 5: WriteTypescriptAsync with string (default styles)
Console.WriteLine("\n=== WriteTypescriptAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(typescriptString);
await AnsiConsole.Console.WriteTypescriptAsync(stream);
await writerTask;

// Method 6: WriteTypescriptAsync with string and custom styles
Console.WriteLine("\n=== WriteTypescriptAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(typescriptString);
await AnsiConsole.Console.WriteTypescriptAsync(stream, customTypescriptStyles);
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
