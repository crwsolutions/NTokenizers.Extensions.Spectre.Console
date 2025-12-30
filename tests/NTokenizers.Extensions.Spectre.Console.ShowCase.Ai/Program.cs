using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NTokenizers.Extensions.Spectre.Console.ShowCase.Ai;
using OllamaSharp;
using OpenAI;
using Spectre.Console;
using System.ClientModel;
using System.Reflection;
using System.Text;
    
Console.WriteLine();
//Console.ReadLine();

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

PrintHeader();

var builder = Host.CreateApplicationBuilder();
builder.Services.AddSingleton<ChatService>();

// Show selection for AI provider
var selectedProvider = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Select AI Provider")
        .PageSize(10)
        .AddChoices("OpenAI", "Ollama")
);

string endpoint;
string modelId;

if (selectedProvider == "Ollama")
{
    endpoint = "http://localhost:11434/";
    modelId = "hf.co/unsloth/Qwen3-Coder-30B-A3B-Instruct-GGUF:Q5_K_XL";

    builder.Services.AddChatClient(ChatClientBuilderChatClientExtensions.AsBuilder(new OllamaApiClient(endpoint, modelId))
        .UseFunctionInvocation()
        .Build()
    );
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
}
else
{
    endpoint = "http://localhost:8080/";
    modelId = "unsloth/Qwen3-Coder-30B-A3B-Instruct-GGUF:Q6_K";
    builder.Services.AddChatClient(new OpenAIClient(
                new ApiKeyCredential("dummy"), // llama.cpp requires one, value ignored
                new OpenAIClientOptions { Endpoint = new Uri(endpoint) })
        .GetChatClient(modelId)
        .AsIChatClient()
        .AsBuilder()
            .UseFunctionInvocation()
            .Build()
    );

    OpenAiModelStatus statusOai = default!;
    await AnsiConsole
        .Status()
        .Spinner(Spinner.Known.Dots)
        .SpinnerStyle(new Style(foreground: Color.Green))
        .StartAsync("Checking ai model...", async ctx =>
        {
            statusOai = await OpenAiEndpoint.GetStatusAsync(endpoint, null, modelId);
        });
    Console.WriteLine();
    AnsiConsole.MarkupLine($"Status: OpenAi: {(statusOai.IsUp ? "[green]✅[/]" : "[red]❌[/]")} Model: {(statusOai.IsAvailable ? "[green]✅[/]" : "[red]❌[/]")} Running: {(statusOai.IsRunning ? "[green]✅[/]" : "[red]❌[/]")}");
}
AnsiConsole.WriteLine();

var app = builder.Build();

AnsiConsole.MarkupLine($"[dodgerblue2]Using model : [silver]'{modelId}'[/] [/]");

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
