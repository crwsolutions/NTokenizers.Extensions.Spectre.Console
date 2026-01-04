---
layout: default
title: "Css"
---

# Css Syntax Highlighting

The `AnsiConsoleCssExtensions` class provides extension methods for writing CSS content to the console with syntax highlighting.

## Methods

### WriteCssAsync (Stream, default styles)

Writes CSS content from a stream to the console with default styling asynchronously.

```csharp
Task<string> WriteCssAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `stream`: The stream containing CSS content.

**Returns:** The CSS content as a string.

### WriteCssAsync (Stream, custom styles)

Writes CSS content from a stream to the console with custom styling asynchronously.

```csharp
Task<string> WriteCssAsync(this IAnsiConsole ansiConsole, Stream stream, CssStyles cssStyles)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `stream`: The stream containing CSS content.
- `cssStyles`: The styles to use for CSS token coloring.

**Returns:** The CSS content as a string.

### WriteCss (Stream, default styles)

Writes CSS content from a stream to the console with default styling synchronously.

```csharp
string WriteCss(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `stream`: The stream containing CSS content.

**Returns:** The CSS content as a string.

### WriteCss (Stream, custom styles)

Writes CSS content from a stream to the console with custom styling synchronously.

```csharp
string WriteCss(this IAnsiConsole ansiConsole, Stream stream, CssStyles cssStyles)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `stream`: The stream containing CSS content.
- `cssStyles`: The styles to use for CSS token coloring.

**Returns:** The CSS content as a string.

### WriteCss (string, default styles)

Writes CSS content from a string to the console with default styling.

```csharp
void WriteCss(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `value`: The CSS string to write.

### WriteCss (string, custom styles)

Writes CSS content from a string to the console with custom styling.

```csharp
void WriteCss(this IAnsiConsole ansiConsole, string value, CssStyles cssStyles)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `value`: The CSS string to write.
- `cssStyles`: The styles to use for CSS token coloring.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var cssString = """
    body {
        background-color: #f0f0f0;
        font-family: Arial, sans-serif;
    }
    
    .header {
        color: blue;
        margin: 10px;
    }
    """;

AnsiConsole.Console.WriteCss(cssString);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customCssStyles = CssStyles.Default;
customCssStyles.OpenParen = new Style(Color.Orange1);
customCssStyles.CloseParen = new Style(Color.Orange1);

AnsiConsole.Console.WriteCss(cssString, customCssStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteCssAsync(stream);
```