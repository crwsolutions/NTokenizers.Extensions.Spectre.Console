---
layout: default
title: "AI Example"
---

# AI Chat Stream Example

This page demonstrates how to use NTokenizers.Extensions.Spectre.Console with AI chat clients, particularly for handling streaming responses from language models. This is especially useful when working with AI frameworks like Microsoft.Extensions.AI.

## Overview

NTokenizers excels at processing real-time tokenized data from AI models, enabling efficient handling of streaming responses and chat conversations without buffering entire responses. The library can render syntax-highlighted output as the AI generates it character by character.

## Converting Chat Client Response Chunks to a Stream

The key to integrating NTokenizers with AI chat clients is converting the streaming response chunks into a `Stream` that NTokenizers can process. Here's how to do it:

```csharp
using Microsoft.Extensions.AI;
using System.IO.Pipelines;
using System.Text;

public static Stream GetChatResponseStream(
    IChatClient chatClient, 
    IList<ChatMessage> history, 
    ChatOptions options)
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
```

### How It Works

1. **Create a Pipe**: Use `System.IO.Pipelines.Pipe` to create a high-performance, async-friendly pipe for streaming data.

2. **Start Background Task**: Run a background task that consumes the AI's streaming response.

3. **Write Chunks**: For each text chunk from the AI, encode it to UTF-8 bytes and write to the pipe's writer.

4. **Complete the Pipe**: When the streaming response is finished, complete the pipe writer.

5. **Return as Stream**: Convert the pipe reader to a stream that NTokenizers can consume.

## Complete Chat Service Example

Here's a complete example of a chat service that renders AI responses with syntax highlighting:

```csharp
using Microsoft.Extensions.AI;
using Spectre.Console;
using System.IO.Pipelines;
using System.Text;
using NTokenizers.Extensions.Spectre.Console;

public class ChatService
{
    private readonly IChatClient _chatClient;

    public ChatService(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task StartAsync()
    {
        var chatOptions = new ChatOptions { };
        List<ChatMessage> chatHistory = [];
        
        while (true)
        {
            // Get user prompt and add to chat history
            AnsiConsole.Markup("[green]Your prompt: [/]");
            var userPrompt = Console.ReadLine();

            if (string.Equals(userPrompt, "bye", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

            // Convert AI response stream to a Stream for NTokenizers
            var stream = GetChatResponseStream(_chatClient, chatHistory, chatOptions);
            
            // Render with markup/markdown syntax highlighting
            var result = await AnsiConsole.Console.WriteMarkupTextAsync(stream);

            // Add assistant response to history for context
            chatHistory.Add(new ChatMessage(ChatRole.Assistant, result));
            Console.WriteLine();
        }
        
        AnsiConsole.MarkupLine("[red]Bye, Bye! 💖[/]");
    }

    public static Stream GetChatResponseStream(
        IChatClient chatClient, 
        IList<ChatMessage> history, 
        ChatOptions options)
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
```

## Setting Up with Dependency Injection

Here's how to set up the chat service with dependency injection using Microsoft.Extensions.Hosting:

```csharp
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OllamaSharp;

var builder = Host.CreateApplicationBuilder();
builder.Services.AddSingleton<ChatService>();

var endpoint = "http://localhost:11434/";
var modelId = "your-model-id";

builder.Services.AddChatClient(
    new OllamaApiClient(endpoint, modelId)
        .AsBuilder()
        .UseFunctionInvocation()
        .Build()
);

var app = builder.Build();
var chatService = app.Services.GetRequiredService<ChatService>();
await chatService.StartAsync();
```

## Why Use Pipes?

Using `System.IO.Pipelines.Pipe` provides several benefits:

- **High Performance**: Pipes are designed for high-throughput, low-allocation scenarios.
- **Backpressure Handling**: Automatically handles backpressure when the consumer is slower than the producer.
- **Async-Friendly**: Built for async/await patterns.
- **Memory Efficient**: Minimizes memory allocations during streaming.

## Alternative: Simple MemoryStream Approach

For simpler scenarios where you don't need real-time streaming, you can buffer the entire response:

```csharp
var response = new StringBuilder();
await foreach (var chunk in chatClient.GetStreamingResponseAsync(history, options))
{
    if (!string.IsNullOrEmpty(chunk.Text))
    {
        response.Append(chunk.Text);
    }
}

// Then render the complete response
AnsiConsole.Console.WriteMarkupText(response.ToString());
```

However, the pipe-based approach is recommended for real-time rendering as the AI generates its response, providing a better user experience.
