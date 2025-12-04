---
layout: default
title: "C#"
---

# C# Syntax Highlighting

The `AnsiConsoleCSharpExtensions` class provides extension methods for `IAnsiConsole` to render C# code with syntax highlighting.

## Methods

### WriteCSharpAsync (Stream, default styles)

Writes C# code from a stream to the console with default syntax highlighting asynchronously.

```csharp
Task<string> WriteCSharpAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing C# code to render.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WriteCSharpAsync (Stream, custom styles)

Writes C# code from a stream to the console with custom syntax highlighting asynchronously.

```csharp
Task<string> WriteCSharpAsync(this IAnsiConsole ansiConsole, Stream stream, CSharpStyles csharpStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing C# code to render.
- `csharpStyles`: The styles to use for syntax highlighting.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WriteCSharp (Stream, default styles)

Writes C# code from a stream to the console with default syntax highlighting synchronously.

```csharp
string WriteCSharp(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing C# code to render.

**Returns:** The parsed string.

### WriteCSharp (Stream, custom styles)

Writes C# code from a stream to the console with custom syntax highlighting synchronously.

```csharp
string WriteCSharp(this IAnsiConsole ansiConsole, Stream stream, CSharpStyles csharpStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing C# code to render.
- `csharpStyles`: The styles to use for syntax highlighting.

**Returns:** The parsed string.

### WriteCSharp (string, default styles)

Writes C# code from a string to the console with default syntax highlighting.

```csharp
void WriteCSharp(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The C# code to render.

### WriteCSharp (string, custom styles)

Writes C# code from a string to the console with custom syntax highlighting.

```csharp
void WriteCSharp(this IAnsiConsole ansiConsole, string value, CSharpStyles csharpStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The C# code to render.
- `csharpStyles`: The styles to use for syntax highlighting.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var csharpCode = """
    using System;

    // Main class
    public class Program {
        public static void Main() {
            Console.WriteLine("Hello, World!");
        }
    }
    """;

AnsiConsole.Console.WriteCSharp(csharpCode);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customCSharpStyles = CSharpStyles.Default;
customCSharpStyles.Keyword = new Style(Color.Orange1);

AnsiConsole.Console.WriteCSharp(csharpCode, customCSharpStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteCSharpAsync(stream);
```
