using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.ShowCase.Css;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleCssExtensions
var cssString = CssExample.GetSampleCss();

var customCssStyles = CssStyles.Default;
customCssStyles.OpenParen = new Style(Color.Orange1);
customCssStyles.CloseParen = new Style(Color.Orange1);

// Method 1: WriteCss with string (default styles)
Console.WriteLine("=== WriteCss with string (default styles) ===");
AnsiConsole.Console.WriteCss(cssString);

// Method 2: WriteCss with string and custom styles
Console.WriteLine("\n=== WriteCss with string and custom styles ===");
AnsiConsole.Console.WriteCss(cssString, customCssStyles);

// Method 3: WriteCss with Stream (default styles)
Console.WriteLine("\n=== WriteCss with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(cssString);
AnsiConsole.Console.WriteCss(stream);
await writerTask;

// Method 4: WriteCss with Stream and custom styles
Console.WriteLine("\n=== WriteCss with Stream and custom styles ===");
(writerTask, stream) = SetupStream(cssString);
AnsiConsole.Console.WriteCss(stream, customCssStyles);
await writerTask;

// Method 5: WriteCssAsync with string (default styles)
Console.WriteLine("\n=== WriteCssAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(cssString);
await AnsiConsole.Console.WriteCssAsync(stream);
await writerTask;

// Method 6: WriteCssAsync with string and custom styles
Console.WriteLine("\n=== WriteCssAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(cssString);
await AnsiConsole.Console.WriteCssAsync(stream, customCssStyles);
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
            //await Task.Delay(rng.Next(0, 2));
        }

        pipe.Close();
    });
    return (writerTask, reader);
}