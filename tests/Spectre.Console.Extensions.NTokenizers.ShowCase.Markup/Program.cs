using Spectre.Console;
using Spectre.Console.Extensions.NTokenizers;
using Spectre.Console.Extensions.NTokenizers.ShowCase.Markup;
using Spectre.Console.Extensions.NTokenizers.Styles;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleMarkupTextExtensions
var markupString = MarkupExample.GetSampleText();

var customMarkupStyles = MarkupStyles.Default;
customMarkupStyles.Heading = new Style(Color.Orange1);

// Method 1: WriteMarkupText with string (default styles)
Console.WriteLine("=== WriteMarkupText with string (default styles) ===");
AnsiConsole.Console.WriteMarkupText(markupString);

// Method 2: WriteMarkupText with string and custom styles
Console.WriteLine("\n=== WriteMarkupText with string and custom styles ===");
AnsiConsole.Console.WriteMarkupText(markupString, customMarkupStyles);

// Method 3: WriteMarkupText with Stream (default styles)
Console.WriteLine("\n=== WriteMarkupText with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(markupString);
AnsiConsole.Console.WriteMarkupText(stream);
await writerTask;

// Method 4: WriteMarkupText with Stream and custom styles
Console.WriteLine("\n=== WriteMarkupText with Stream and custom styles ===");
(writerTask, stream) = SetupStream(markupString);
AnsiConsole.Console.WriteMarkupText(stream, customMarkupStyles);
await writerTask;

// Method 5: WriteMarkupTextAsync with string (default styles)
Console.WriteLine("\n=== WriteMarkupTextAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(markupString);
await AnsiConsole.Console.WriteMarkupTextAsync(stream);
await writerTask;

// Method 6: WriteMarkupTextAsync with string and custom styles
Console.WriteLine("\n=== WriteMarkupTextAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(markupString);
await AnsiConsole.Console.WriteMarkupTextAsync(stream, customMarkupStyles);
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
