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

            chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

            var stream = GetChatResponseStream(_chatClient, chatHistory, chatOptions);
            var result = await AnsiConsole.Console.WriteMarkupTextAsync(stream);

            Debug.WriteLine(result);

            chatHistory.Add(new ChatMessage(ChatRole.Assistant, result));
            System.Console.WriteLine();
        }
        AnsiConsole.MarkupLine("[red]Bye, Bye! 💖[/]");
    }

    public static Stream GetChatResponseStream(IChatClient chatClient, IList<ChatMessage> history, ChatOptions options)
    {
        var pipe = new Pipe();

        _ = Task.Run(async () =>
        {
            await foreach (var chunk in chatClient.GetStreamingResponseAsync(history, options))
            {
                if (!string.IsNullOrEmpty(chunk.Text))
                {
                    var bytes = Encoding.UTF8.GetBytes(chunk.Text);
                    await pipe.Writer.WriteAsync(bytes);
                }
            }

            await pipe.Writer.CompleteAsync();
        });

        return pipe.Reader.AsStream();
    }
}
