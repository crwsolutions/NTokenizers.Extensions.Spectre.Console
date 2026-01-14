using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.ShowCase.Html;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleHtmlExtensions
var htmlString = HtmlExample.GetSampleHtml();

var customHtmlStyles = HtmlStyles.Default;
customHtmlStyles.ElementName = new Style(Color.Orange1);

// Method 1: WriteHtml with string (default styles)
Console.WriteLine("=== WriteHtml with string (default styles) ===");
AnsiConsole.Console.WriteHtml(htmlString);

// Method 2: WriteHtml with string and custom styles
Console.WriteLine("\n=== WriteHtml with string and custom styles ===");
AnsiConsole.Console.WriteHtml(htmlString, customHtmlStyles);

// Method 3: WriteHtml with Stream (default styles)
Console.WriteLine("\n=== WriteHtml with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(htmlString);
AnsiConsole.Console.WriteHtml(stream);
await writerTask;

// Method 4: WriteHtml with Stream and custom styles
Console.WriteLine("\n=== WriteHtml with Stream and custom styles ===");
(writerTask, stream) = SetupStream(htmlString);
AnsiConsole.Console.WriteHtml(stream, customHtmlStyles);
await writerTask;

// Method 5: WriteHtmlAsync with string (default styles)
Console.WriteLine("\n=== WriteHtmlAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(htmlString);
await AnsiConsole.Console.WriteHtmlAsync(stream);
await writerTask;

// Method 6: WriteHtmlAsync with string and custom styles
Console.WriteLine("\n=== WriteHtmlAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(htmlString);
await AnsiConsole.Console.WriteHtmlAsync(stream, customHtmlStyles);
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
