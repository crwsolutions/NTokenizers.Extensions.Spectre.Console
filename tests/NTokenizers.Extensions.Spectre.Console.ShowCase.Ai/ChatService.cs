using Microsoft.Extensions.AI;
using NTokenizers.Extensions.Spectre.Console.ShowCase.Ai.Prompts;
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
        var help = """"
                   #### Some extra instructions to accomodate multiline input:
                   |Command|Description|
                   |**Shift-Enter**|to place a soft new line|
                   |**Ctrl-B**|to paste multiline, do not use **Ctrl-V**, because that will input a _<Enter>_'s|
                   |**bye**|type bye to end the chat session|
                   |**clear**|type clear to clear the chat history|
                   """";

        AnsiConsole.Console.WriteMarkdown(help);

        var chatOptions = new ChatOptions { };
        List<ChatMessage> chatHistory = [];
        while (true)
        {
            System.Console.WriteLine();

            // Get user prompt and add to chat history
            //AnsiConsole.MarkupLine("[green]Your prompt: [/]");
            var userPrompt = AnsiConsole.Prompt(new PromptBox().Placeholder("Your prompt"));

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

            var stream = GetChatResponseStream(_chatClient, chatHistory, chatOptions, cts.Token, out var firstChunkTask);

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .SpinnerStyle(new Style(foreground: Color.Green))
                .StartAsync("Processing...", async ctx =>
                {
                    await firstChunkTask;
                });

            AnsiConsole.Console.WriteLine();

            var result = await AnsiConsole.Console.WriteMarkdownAsync(stream, encoding: Encoding.UTF8, ct: cts.Token);

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

    public static Stream GetChatResponseStream(IChatClient chatClient, IList<ChatMessage> history, ChatOptions options, CancellationToken ct, out Task firstChunkTask)
    {
        var pipe = new Pipe();
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        firstChunkTask = tcs.Task; // expose this to await in spinner

        _ = Task.Run(async () =>
        {
            await foreach (var chunk in chatClient.GetStreamingResponseAsync(history, options, ct))
            {
                if (!string.IsNullOrEmpty(chunk.Text))
                {
                    // Signal that first chunk arrived
                    tcs.TrySetResult();

                    var bytes = Encoding.UTF8.GetBytes(chunk.Text);
                    await pipe.Writer.WriteAsync(bytes, ct);
                }
            }

            await pipe.Writer.CompleteAsync();
        }, ct);

        return pipe.Reader.AsStream();
    }
}
