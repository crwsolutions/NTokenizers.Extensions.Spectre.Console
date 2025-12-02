using Spectre.Console;
using Spectre.Console.Extensions.NTokenizers;
using Spectre.Console.Extensions.NTokenizers.ShowCase.Json;
using Spectre.Console.Extensions.NTokenizers.Styles;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleJsonExtensions
var jsonString = JsonExample.GetSampleJson();

var customJsonStyles = JsonStyles.Default;
customJsonStyles.PropertyName = new Style(Color.Orange1);

// Method 1: WriteJson with string (default styles)
Console.WriteLine("=== WriteJson with string (default styles) ===");
AnsiConsole.Console.WriteJson(jsonString);

// Method 2: WriteJson with string and custom styles
Console.WriteLine("\n=== WriteJson with string and custom styles ===");
AnsiConsole.Console.WriteJson(jsonString, customJsonStyles);

// Method 3: WriteJson with Stream (default styles)
Console.WriteLine("\n=== WriteJson with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(jsonString);
AnsiConsole.Console.WriteJson(stream);
await writerTask;

// Method 4: WriteJson with Stream and custom styles
Console.WriteLine("\n=== WriteJson with Stream and custom styles ===");
(writerTask, stream) = SetupStream(jsonString);
AnsiConsole.Console.WriteJson(stream, customJsonStyles);
await writerTask;

// Method 5: WriteJsonAsync with string (default styles)
Console.WriteLine("\n=== WriteJsonAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(jsonString);
await AnsiConsole.Console.WriteJsonAsync(stream);
await writerTask;

// Method 6: WriteJsonAsync with string and custom styles
Console.WriteLine("\n=== WriteJsonAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(jsonString);
await AnsiConsole.Console.WriteJsonAsync(stream, customJsonStyles);
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
