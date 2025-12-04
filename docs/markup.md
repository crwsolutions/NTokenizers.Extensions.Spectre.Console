---
layout: default
title: "Markup"
---

# Markup Text Syntax Highlighting

The `AnsiConsoleMarkupTextExtensions` class provides extension methods for `IAnsiConsole` to render markup text (Markdown) with syntax highlighting.

## Methods

### WriteMarkupTextAsync (Stream, default styles)

Writes markup text from a stream to the console with default styling asynchronously.

```csharp
Task<string> WriteMarkupTextAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `stream`: The stream containing the markup text.

**Returns:** A task that represents the asynchronous write operation and contains the parsed string.

### WriteMarkupTextAsync (Stream, custom styles)

Writes markup text from a stream to the console with specified styling asynchronously.

```csharp
Task<string> WriteMarkupTextAsync(this IAnsiConsole ansiConsole, Stream stream, MarkupStyles markupStyles)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `stream`: The stream containing the markup text.
- `markupStyles`: The markup styles to use for rendering.

**Returns:** A task that represents the asynchronous write operation and contains the parsed string.

### WriteMarkupText (Stream, default styles)

Writes markup text from a stream to the console with default styling synchronously.

```csharp
string WriteMarkupText(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `stream`: The stream containing the markup text.

**Returns:** The parsed string from the input stream.

### WriteMarkupText (Stream, custom styles)

Writes markup text from a stream to the console with specified styling synchronously.

```csharp
string WriteMarkupText(this IAnsiConsole ansiConsole, Stream stream, MarkupStyles markupStyles)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `stream`: The stream containing the markup text.
- `markupStyles`: The markup styles to use for rendering.

**Returns:** The parsed string from the input stream.

### WriteMarkupText (string, default styles)

Writes markup text from a string to the console with default styling.

```csharp
void WriteMarkupText(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `value`: The markup text to write.

### WriteMarkupText (string, custom styles)

Writes markup text from a string to the console with specified styling.

```csharp
void WriteMarkupText(this IAnsiConsole ansiConsole, string value, MarkupStyles markupStyles)
```

**Parameters:**
- `ansiConsole`: The ANSI console to write to.
- `value`: The markup text to write.
- `markupStyles`: The markup styles to use for rendering.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var markupText = """
    ## Text
    Lorem ipsum dolor sit amet, consectetur adipiscing elit.

    # h1 Heading
    ## h2 Heading
    ### h3 Heading

    ## Emphasis
    **This is bold text**
    *This is italic text*
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

AnsiConsole.Console.WriteMarkupText(markupText);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customMarkupStyles = MarkupStyles.Default;
customMarkupStyles.Heading = new Style(Color.Orange1);

AnsiConsole.Console.WriteMarkupText(markupText, customMarkupStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteMarkupTextAsync(stream);
```
