---
layout: default
title: "Html"
---

# HTML Syntax Highlighting

The `AnsiConsoleHtmlExtensions` class provides extension methods for `IAnsiConsole` to render HTML content with syntax highlighting.

## Methods

### WriteHtmlAsync (Stream, default styles)

Writes HTML content from a stream to the console with default styling asynchronously.

```csharp
Task<string> WriteHtmlAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The `Stream` containing the HTML content.

**Returns:** A task that represents the asynchronous operation. The result is the input stream as a string.

### WriteHtmlAsync (Stream, custom styles)

Writes HTML content from a stream to the console with custom styling asynchronously.

```csharp
Task<string> WriteHtmlAsync(this IAnsiConsole ansiConsole, Stream stream, HtmlStyles htmlStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The `Stream` containing the HTML content.
- `htmlStyles`: The `HtmlStyles` to use for styling the output.

**Returns:** A task that represents the asynchronous operation. The result is the input stream as a string.

### WriteHtml (Stream, default styles)

Writes HTML content from a stream to the console with default styling synchronously.

```csharp
string WriteHtml(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The `Stream` containing the HTML content.

**Returns:** The input stream as a string.

### WriteHtml (Stream, custom styles)

Writes HTML content from a stream to the console with custom styling synchronously.

```csharp
string WriteHtml(this IAnsiConsole ansiConsole, Stream stream, HtmlStyles htmlStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The `Stream` containing the HTML content.
- `htmlStyles`: The `HtmlStyles` to use for styling the output.

**Returns:** The input stream as a string.

### WriteHtml (string, default styles)

Writes HTML content from a string to the console with default styling.

```csharp
void WriteHtml(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The HTML content as a string.

### WriteHtml (string, custom styles)

Writes HTML content from a string to the console with custom styling.

```csharp
void WriteHtml(this IAnsiConsole ansiConsole, string value, HtmlStyles htmlStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The HTML content as a string.
- `htmlStyles`: The `HtmlStyles` to use for styling the output.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var htmlContent = """
    <html>
      <head>
        <title>Example Page</title>
      </head>
      <body>
        <h1>Hello World!</h1>
        <p>This is an example paragraph.</p>
        <ul>
          <li>Item 1</li>
          <li>Item 2</li>
        </ul>
      </body>
    </html>
    """;

AnsiConsole.Console.WriteHtml(htmlContent);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customHtmlStyles = HtmlStyles.Default;
customHtmlStyles.ElementName = new Style(Color.Orange1);

AnsiConsole.Console.WriteHtml(htmlContent, customHtmlStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteHtmlAsync(stream);
```