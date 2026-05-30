using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.ShowCase.Cpp;
using NTokenizers.Extensions.Spectre.Console.Styles;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleCppExtensions
var cppString = CppExample.GetSampleCpp();

var customCppStyles = CppStyles.Default;
customCppStyles.Keyword = new Style(Color.Orange1);

// Method 1: WriteCpp with string (default styles)
Console.WriteLine("=== WriteCpp with string (default styles) ===");
AnsiConsole.Console.WriteCpp(cppString);

// Method 2: WriteCpp with string and custom styles
Console.WriteLine("\n=== WriteCpp with string and custom styles ===");
AnsiConsole.Console.WriteCpp(cppString, customCppStyles);

// Method 3: WriteCpp with Stream (default styles)
Console.WriteLine("\n=== WriteCpp with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(cppString);
AnsiConsole.Console.WriteCpp(stream);
await writerTask;

// Method 4: WriteCpp with Stream and custom styles
Console.WriteLine("\n=== WriteCpp with Stream and custom styles ===");
(writerTask, stream) = SetupStream(cppString);
AnsiConsole.Console.WriteCpp(stream, customCppStyles);
await writerTask;

// Method 5: WriteCppAsync with string (default styles)
Console.WriteLine("\n=== WriteCppAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(cppString);
await AnsiConsole.Console.WriteCppAsync(stream);
await writerTask;

// Method 6: WriteCppAsync with string and custom styles
Console.WriteLine("\n=== WriteCppAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(cppString);
await AnsiConsole.Console.WriteCppAsync(stream, customCppStyles);
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
            await Task.Delay(rng.Next(0, 20));
        }

        pipe.Close();
    });
    return (writerTask, reader);
}
