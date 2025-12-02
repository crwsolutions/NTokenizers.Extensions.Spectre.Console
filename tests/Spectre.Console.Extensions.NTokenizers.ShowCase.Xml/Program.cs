using Spectre.Console;
using Spectre.Console.Extensions.NTokenizers;
using Spectre.Console.Extensions.NTokenizers.ShowCase.Xml;
using Spectre.Console.Extensions.NTokenizers.Styles;
using System.IO.Pipes;
using System.Text;

// Showcase all methods of AnsiConsoleXmlExtensions
var xmlString = XmlExample.GetSampleXml();

var customXmlStyles = XmlStyles.Default;
customXmlStyles.ElementName = new Style(Color.Orange1);

// Method 1: WriteXml with string (default styles)
Console.WriteLine("=== WriteXml with string (default styles) ===");
AnsiConsole.Console.WriteXml(xmlString);

// Method 2: WriteXml with string and custom styles
Console.WriteLine("\n=== WriteXml with string and custom styles ===");
AnsiConsole.Console.WriteXml(xmlString, customXmlStyles);

// Method 3: WriteXml with Stream (default styles)
Console.WriteLine("\n=== WriteXml with Stream (default styles) ===");
var (writerTask, stream) = SetupStream(xmlString);
AnsiConsole.Console.WriteXml(stream);
await writerTask;

// Method 4: WriteXml with Stream and custom styles
Console.WriteLine("\n=== WriteXml with Stream and custom styles ===");
(writerTask, stream) = SetupStream(xmlString);
AnsiConsole.Console.WriteXml(stream, customXmlStyles);
await writerTask;

// Method 5: WriteXmlAsync with string (default styles)
Console.WriteLine("\n=== WriteXmlAsync with string (default styles) ===");
(writerTask, stream) = SetupStream(xmlString);
await AnsiConsole.Console.WriteXmlAsync(stream);
await writerTask;

// Method 6: WriteXmlAsync with string and custom styles
Console.WriteLine("\n=== WriteXmlAsync with string and custom styles ===");
(writerTask, stream) = SetupStream(xmlString);
await AnsiConsole.Console.WriteXmlAsync(stream, customXmlStyles);
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
