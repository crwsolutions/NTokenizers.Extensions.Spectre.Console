using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.ShowCase.Python;
using NTokenizers.Extensions.Spectre.Console.Styles;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsolePythonExtensions
var pythonString = PythonExample.GetSamplePython();

var customPythonStyles = PythonStyles.Default;
customPythonStyles.Keyword = new Style(Color.Orange1);

// Method 1: WritePython with string (default styles)
Console.WriteLine("=== WritePython with string (default styles) ===");
AnsiConsole.Console.WritePython(pythonString);

// Method 2: WritePython with string and custom styles
Console.WriteLine("\n=== WritePython with string and custom styles ===");
AnsiConsole.Console.WritePython(pythonString, customPythonStyles);

// Method 3: WritePython with Stream (default styles)
Console.WriteLine("\n=== WritePython with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(pythonString);
AnsiConsole.Console.WritePython(stream);
await writerTask;

// Method 4: WritePython with Stream and custom styles
Console.WriteLine("\n=== WritePython with Stream and custom styles ===");
(writerTask, stream) = SetupStream(pythonString);
AnsiConsole.Console.WritePython(stream, customPythonStyles);
await writerTask;

// Method 5: WritePythonAsync with string (default styles)
Console.WriteLine("\n=== WritePythonAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(pythonString);
await AnsiConsole.Console.WritePythonAsync(stream);
await writerTask;

// Method 6: WritePythonAsync with string and custom styles
Console.WriteLine("\n=== WritePythonAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(pythonString);
await AnsiConsole.Console.WritePythonAsync(stream, customPythonStyles);
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
