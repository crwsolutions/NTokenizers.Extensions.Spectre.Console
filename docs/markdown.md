---
layout: default
title: "Markdown"
---

# Markdown Syntax Highlighting

The `AnsiConsoleMarkdownExtensions` class provides extension methods for `IAnsiConsole` to render markdown with syntax highlighting.

## Methods

### WriteMarkdownAsync (Stream, default styles)

Writes markdown from a stream to the console with default styling asynchronously.

```csharp
Task<string> WriteMarkdownAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `stream`: The stream containing the markdown.

**Returns:** A task that represents the asynchronous write operation and contains the parsed string.

### WriteMarkdownAsync (Stream, custom styles)

Writes markdown from a stream to the console with specified styling asynchronously.

```csharp
Task<string> WriteMarkdownAsync(this IAnsiConsole ansiConsole, Stream stream, MarkdownStyles markdownStyles)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `stream`: The stream containing the markdown.
- `markdownStyles`: The markdown styles to use for rendering.

**Returns:** A task that represents the asynchronous write operation and contains the parsed string.

### WriteMarkdown (Stream, default styles)

Writes markdown from a stream to the console with default styling synchronously.

```csharp
string WriteMarkdown(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `stream`: The stream containing the markdown.

**Returns:** The parsed string from the input stream.

### WriteMarkdown (Stream, custom styles)

Writes markdown from a stream to the console with specified styling synchronously.

```csharp
string WriteMarkdown(this IAnsiConsole ansiConsole, Stream stream, MarkdownStyles markdownStyles)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `stream`: The stream containing the markdown.
- `markdownStyles`: The markdown styles to use for rendering.

**Returns:** The parsed string from the input stream.

### WriteMarkdown (string, default styles)

Writes markdown from a string to the console with default styling.

```csharp
void WriteMarkdown(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `value`: The markdown to write.

### WriteMarkdown (string, custom styles)

Writes markdown from a string to the console with specified styling.

```csharp
void WriteMarkdown(this IAnsiConsole ansiConsole, string value, MarkdownStyles markdownStyles)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `value`: The markdown to write.
- `markdownStyles`: The markdown styles to use for rendering.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var markdown = """
    ## Text
    Lorem ipsum dolor sit amet, consectetur adipiscing elit.

    # h1 Heading
    ## h2 Heading
    ### h3 Heading

    ## Emphasis
    **This is bold text**
    *This is italic text*
    ***This is bold italic text***
    ~~Strikethrough~~

    ## Lists
    + Create a list by starting a line with `+`, `-`, or `*`
    + Sub-lists are made by indenting 2 spaces

    ## Code
    Inline `code`

    ```csharp
    Console.WriteLine("Hello, World!");
    ```
    """;

AnsiConsole.Console.WriteMarkdown(markdown);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customMarkdownStyles = MarkdownStyles.Default;
customMarkdownStyles.Heading = new Style(Color.Orange1);

AnsiConsole.Console.WriteMarkdown(markdown, customMarkdownStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteMarkdownAsync(stream);
```
