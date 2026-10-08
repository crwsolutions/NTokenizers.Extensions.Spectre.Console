using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.ShowCase.Markdown;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleMarkupTextExtensions
var markdownString = MarkdownExample.GetSampleText();

var customMarkdownStyles = MarkdownStyles.Default;
customMarkdownStyles.MarkdownHeadingStyles.Level1 = Color.Orange1;
customMarkdownStyles.MarkdownHeadingStyles.Level2To4 = Color.Orange3;
customMarkdownStyles.MarkdownHeadingStyles.Level5AndAbove = Color.Orange4;

// Method 1: WriteMarkdown with string (default styles)
Console.WriteLine("=== WriteMarkdown with string (default styles) ===");
AnsiConsole.Console.WriteMarkdown(markdownString);

// Method 2: WriteMarkdown with string and custom styles
Console.WriteLine("\n=== WriteMarkdown with string and custom styles ===");
AnsiConsole.Console.WriteMarkdown(markdownString, customMarkdownStyles);

// Method 3: WriteMarkdown with Stream (default styles)
Console.WriteLine("\n=== WriteMarkdown with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(markdownString);
AnsiConsole.Console.WriteMarkdown(stream);
await writerTask;

// Method 4: WriteMarkdown with Stream and custom styles
Console.WriteLine("\n=== WriteMarkdown with Stream and custom styles ===");
(writerTask, stream) = SetupStream(markdownString);
AnsiConsole.Console.WriteMarkdown(stream, customMarkdownStyles);
await writerTask;

// Method 5: WriteMarkdownAsync with string (default styles)
Console.WriteLine("\n=== WriteMarkdownAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(markdownString);
await AnsiConsole.Console.WriteMarkdownAsync(stream);
await writerTask;

// Method 6: WriteMarkdownAsync with string and custom styles
Console.WriteLine("\n=== WriteMarkdownAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(markdownString);
await AnsiConsole.Console.WriteMarkdownAsync(stream, customMarkdownStyles);
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
