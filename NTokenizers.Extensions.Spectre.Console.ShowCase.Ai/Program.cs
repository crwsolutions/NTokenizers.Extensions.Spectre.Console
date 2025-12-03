using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using OllamaSharp;
using Spectre.Console;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using NTokenizers.Extensions.Spectre.Console.ShowCase.Ai;

Console.WriteLine();
//Console.ReadLine();

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

PrintHeader();

var builder = Host.CreateApplicationBuilder();
builder.Services.AddSingleton<ChatService>();

var endpoint = "http://localhost:11434/";
var modelId = "hf.co/unsloth/Qwen3-Coder-30B-A3B-Instruct-GGUF:Q5_K_XL";

builder.Services.AddChatClient(ChatClientBuilderChatClientExtensions.AsBuilder(new OllamaApiClient(endpoint, modelId))
    .UseFunctionInvocation()
    .Build()
    );
var app = builder.Build();

AnsiConsole.MarkupLine($"[dodgerblue2]Using model : [silver]'{modelId}'[/] [/]");

OllamaModelStatus status = default!;

await AnsiConsole
    .Status()
    .Spinner(Spinner.Known.Dots)
    .SpinnerStyle(new Style(foreground: Color.Green))
    .StartAsync("Checking model...", async ctx =>
    {
        status = await OllamaEndpoint.GetStatusAsync(endpoint, modelId);
    });

Console.WriteLine();
AnsiConsole.MarkupLine($"Status: Ollama: {(status.IsUp ? "[green]✅[/]" : "[red]❌[/]")} Model: {(status.IsAvailable ? "[green]✅[/]" : "[red]❌[/]")} Running: {(status.IsRunning ? "[green]✅[/]" : "[red]❌[/]")}");
Console.WriteLine();

var chatService = app.Services.GetRequiredService<ChatService>();
await chatService.StartAsync();

static void PrintHeader()
{
    var font = FigletFont.Load(
        Assembly
        .GetExecutingAssembly()
        .GetManifestResourceStream(
            "NTokenizers.Extensions.Spectre.Console.ShowCase.Ai.ANSI_Shadow.flf")!
         );
    AnsiConsole.Write(new FigletText(font, "Ai Demo").Centered().Color(Color.Orange1));
}
