---
layout: default
title: "C"
---

# C Syntax Highlighting

The `AnsiConsoleCExtensions` class provides extension methods for `IAnsiConsole` to render C code with syntax highlighting.

## Methods

### WriteCAsync (Stream, default styles)

Writes C code from a stream to the console with default syntax highlighting asynchronously.

```csharp
Task<string> WriteCAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing C code to render.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WriteCAsync (Stream, custom styles)

Writes C code from a stream to the console with custom syntax highlighting asynchronously.

```csharp
Task<string> WriteCAsync(this IAnsiConsole ansiConsole, Stream stream, CStyles cStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing C code to render.
- `cStyles`: The styles to use for syntax highlighting.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WriteC (Stream, default styles)

Writes C code from a stream to the console with default syntax highlighting synchronously.

```csharp
string WriteC(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing C code to render.

**Returns:** The parsed string.

### WriteC (Stream, custom styles)

Writes C code from a stream to the console with custom syntax highlighting synchronously.

```csharp
string WriteC(this IAnsiConsole ansiConsole, Stream stream, CStyles cStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing C code to render.
- `cStyles`: The styles to use for syntax highlighting.

**Returns:** The parsed string.

### WriteC (string, default styles)

Writes C code from a string to the console with default syntax highlighting.

```csharp
void WriteC(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The C code to render.

### WriteC (string, custom styles)

Writes C code from a string to the console with custom syntax highlighting.

```csharp
void WriteC(this IAnsiConsole ansiConsole, string value, CStyles cStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The C code to render.
- `cStyles`: The styles to use for syntax highlighting.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var cCode = """
    #include <stdio.h>

    // Hello World in C
    int main() {
        printf("Hello, World!\n");
        return 0;
    }
    """;

AnsiConsole.Console.WriteC(cCode);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customCStyles = CStyles.Default;
customCStyles.Keyword = new Style(Color.Orange1);

AnsiConsole.Console.WriteC(cCode, customCStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteCAsync(stream);
```
