using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.ShowCase.Yaml;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleYamlExtensions
var yamlString = YamlExample.GetSampleYaml();

var customYamlStyles = YamlStyles.Default;
customYamlStyles.Key = new Style(Color.Orange1);

// Method 1: WriteYaml with string (default styles)
Console.WriteLine("=== WriteYaml with string (default styles) ===");
AnsiConsole.Console.WriteYaml(yamlString);

// Method 2: WriteYaml with string and custom styles
Console.WriteLine("\n=== WriteYaml with string and custom styles ===");
AnsiConsole.Console.WriteYaml(yamlString, customYamlStyles);

// Method 3: WriteYaml with Stream (default styles)
Console.WriteLine("\n=== WriteYaml with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(yamlString);
AnsiConsole.Console.WriteYaml(stream);
await writerTask;

// Method 4: WriteYaml with Stream and custom styles
Console.WriteLine("\n=== WriteYaml with Stream and custom styles ===");
(writerTask, stream) = SetupStream(yamlString);
AnsiConsole.Console.WriteYaml(stream, customYamlStyles);
await writerTask;

// Method 5: WriteYamlAsync with string (default styles)
Console.WriteLine("\n=== WriteYamlAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(yamlString);
await AnsiConsole.Console.WriteYamlAsync(stream);
await writerTask;

// Method 6: WriteYamlAsync with string and custom styles
Console.WriteLine("\n=== WriteYamlAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(yamlString);
await AnsiConsole.Console.WriteYamlAsync(stream, customYamlStyles);
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
