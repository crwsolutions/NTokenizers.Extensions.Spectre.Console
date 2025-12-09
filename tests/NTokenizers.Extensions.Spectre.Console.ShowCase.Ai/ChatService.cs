using Microsoft.Extensions.AI;
using Spectre.Console;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Text;

namespace NTokenizers.Extensions.Spectre.Console.ShowCase.Ai;

public partial class ChatService
{
    [Dependency]
    private readonly IChatClient _chatClient;

    public async Task StartAsync()
    {
        System.Console.WriteLine();

        var chatOptions = new ChatOptions { };
        List<ChatMessage> chatHistory = [];
        while (true)
        {
            // Get user prompt and add to chat history
            AnsiConsole.Markup("[green]Your prompt: [/]");
            var userPrompt = System.Console.ReadLine();

            if (string.Equals(userPrompt, "bye", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            if (string.Equals(userPrompt, "clear", StringComparison.OrdinalIgnoreCase))
            {
                chatHistory.Clear();
                AnsiConsole.MarkupLine("[darkorange3_1]Cleared the history 🧹[/]");
                System.Console.WriteLine();
                continue;
            }

            chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

            // Create a new cancellation token for this request
            var cts = new CancellationTokenSource();

            // Start listening for ESC key in a separate task
            var keyTask = Task.Run(async () =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    if (System.Console.KeyAvailable && System.Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        cts.Cancel();
                        break;
                    }
                    await Task.Delay(100); // Prevent busy waiting
                }
            });

            var stream = GetChatResponseStream(_chatClient, chatHistory, chatOptions, cts.Token);
            var result = await AnsiConsole.Console.WriteMarkupTextAsync(stream, encoding: Encoding.UTF8, ct: cts.Token);

            Debug.WriteLine(result);

            if (cts.IsCancellationRequested)
            {
                System.Console.WriteLine();
                AnsiConsole.MarkupLine("[red]Request cancelled.[/]");
            }
            else
            {
                chatHistory.Add(new ChatMessage(ChatRole.Assistant, result));
            }

            cts.Cancel();
            await keyTask;

            System.Console.WriteLine();
        }
        AnsiConsole.MarkupLine("[red]Bye, Bye! 💖[/]");
    }

    public static Stream GetChatResponseStream(IChatClient chatClient, IList<ChatMessage> history, ChatOptions options, CancellationToken ct)
    {
        var pipe = new Pipe();

        _ = Task.Run(async () =>
        {
            await foreach (var chunk in chatClient.GetStreamingResponseAsync(history, options, ct))
            {
                if (!string.IsNullOrEmpty(chunk.Text))
                {
                    var bytes = Encoding.UTF8.GetBytes(chunk.Text);
                    await pipe.Writer.WriteAsync(bytes);
                }
            }

            await pipe.Writer.CompleteAsync();
        }, ct);

        return pipe.Reader.AsStream();
    }
}
