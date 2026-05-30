---
layout: default
title: "Go"
---

# Go Syntax Highlighting

The `AnsiConsoleGoExtensions` class provides extension methods for `IAnsiConsole` to render Go code with syntax highlighting.

## Methods

### WriteGoAsync (Stream, default styles)

Writes Go code from a stream to the console with default syntax highlighting asynchronously.

```csharp
Task<string> WriteGoAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Go code to render.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WriteGoAsync (Stream, custom styles)

Writes Go code from a stream to the console with custom syntax highlighting asynchronously.

```csharp
Task<string> WriteGoAsync(this IAnsiConsole ansiConsole, Stream stream, GoStyles goStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Go code to render.
- `goStyles`: The styles to use for syntax highlighting.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WriteGo (Stream, default styles)

Writes Go code from a stream to the console with default syntax highlighting synchronously.

```csharp
string WriteGo(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Go code to render.

**Returns:** The parsed string.

### WriteGo (Stream, custom styles)

Writes Go code from a stream to the console with custom syntax highlighting synchronously.

```csharp
string WriteGo(this IAnsiConsole ansiConsole, Stream stream, GoStyles goStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Go code to render.
- `goStyles`: The styles to use for syntax highlighting.

**Returns:** The parsed string.

### WriteGo (string, default styles)

Writes Go code from a string to the console with default syntax highlighting.

```csharp
void WriteGo(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The Go code to render.

### WriteGo (string, custom styles)

Writes Go code from a string to the console with custom syntax highlighting.

```csharp
void WriteGo(this IAnsiConsole ansiConsole, string value, GoStyles goStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The Go code to render.
- `goStyles`: The styles to use for syntax highlighting.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var goCode = @"
package main

import ""fmt""

func main() {
    fmt.Println(""Hello, World!"")
}
";

AnsiConsole.Console.WriteGo(goCode);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customGoStyles = GoStyles.Default;
customGoStyles.Keyword = new Style(Color.Orange1);

AnsiConsole.Console.WriteGo(goCode, customGoStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteGoAsync(stream);
```
