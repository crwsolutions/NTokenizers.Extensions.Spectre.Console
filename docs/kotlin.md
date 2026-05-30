---
layout: default
title: "Kotlin"
---

# Kotlin Syntax Highlighting

The `AnsiConsoleKotlinExtensions` class provides extension methods for `IAnsiConsole` to render Kotlin code with syntax highlighting.

## Methods

### WriteKotlinAsync (Stream, default styles)

Writes Kotlin code from a stream to the console with default syntax highlighting asynchronously.

```csharp
Task<string> WriteKotlinAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Kotlin code to render.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WriteKotlinAsync (Stream, custom styles)

Writes Kotlin code from a stream to the console with custom syntax highlighting asynchronously.

```csharp
Task<string> WriteKotlinAsync(this IAnsiConsole ansiConsole, Stream stream, KotlinStyles kotlinStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Kotlin code to render.
- `kotlinStyles`: The styles to use for syntax highlighting.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WriteKotlin (Stream, default styles)

Writes Kotlin code from a stream to the console with default syntax highlighting synchronously.

```csharp
string WriteKotlin(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Kotlin code to render.

**Returns:** The parsed string.

### WriteKotlin (Stream, custom styles)

Writes Kotlin code from a stream to the console with custom syntax highlighting synchronously.

```csharp
string WriteKotlin(this IAnsiConsole ansiConsole, Stream stream, KotlinStyles kotlinStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Kotlin code to render.
- `kotlinStyles`: The styles to use for syntax highlighting.

**Returns:** The parsed string.

### WriteKotlin (string, default styles)

Writes Kotlin code from a string to the console with default syntax highlighting.

```csharp
void WriteKotlin(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The Kotlin code to render.

### WriteKotlin (string, custom styles)

Writes Kotlin code from a string to the console with custom syntax highlighting.

```csharp
void WriteKotlin(this IAnsiConsole ansiConsole, string value, KotlinStyles kotlinStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The Kotlin code to render.
- `kotlinStyles`: The styles to use for syntax highlighting.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var kotlinCode = @"
fun main() {
    println(""Hello, World!"")
}
";

AnsiConsole.Console.WriteKotlin(kotlinCode);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customKotlinStyles = KotlinStyles.Default;
customKotlinStyles.Keyword = new Style(Color.Orange1);

AnsiConsole.Console.WriteKotlin(kotlinCode, customKotlinStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteKotlinAsync(stream);
```
