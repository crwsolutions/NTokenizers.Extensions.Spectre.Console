---
layout: default
title: "Swift"
---

# Swift Syntax Highlighting

The `AnsiConsoleSwiftExtensions` class provides extension methods for `IAnsiConsole` to render Swift code with syntax highlighting.

## Methods

### WriteSwiftAsync (Stream, default styles)

Writes Swift code from a stream to the console with default syntax highlighting asynchronously.

```csharp
Task<string> WriteSwiftAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Swift code to render.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WriteSwiftAsync (Stream, custom styles)

Writes Swift code from a stream to the console with custom syntax highlighting asynchronously.

```csharp
Task<string> WriteSwiftAsync(this IAnsiConsole ansiConsole, Stream stream, SwiftStyles swiftStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Swift code to render.
- `swiftStyles`: The styles to use for syntax highlighting.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WriteSwift (Stream, default styles)

Writes Swift code from a stream to the console with default syntax highlighting synchronously.

```csharp
string WriteSwift(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Swift code to render.

**Returns:** The parsed string.

### WriteSwift (Stream, custom styles)

Writes Swift code from a stream to the console with custom syntax highlighting synchronously.

```csharp
string WriteSwift(this IAnsiConsole ansiConsole, Stream stream, SwiftStyles swiftStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Swift code to render.
- `swiftStyles`: The styles to use for syntax highlighting.

**Returns:** The parsed string.

### WriteSwift (string, default styles)

Writes Swift code from a string to the console with default syntax highlighting.

```csharp
void WriteSwift(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The Swift code to render.

### WriteSwift (string, custom styles)

Writes Swift code from a string to the console with custom syntax highlighting.

```csharp
void WriteSwift(this IAnsiConsole ansiConsole, string value, SwiftStyles swiftStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The Swift code to render.
- `swiftStyles`: The styles to use for syntax highlighting.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var swiftCode = @"
import Foundation

print(""Hello, World!"")
";

AnsiConsole.Console.WriteSwift(swiftCode);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customSwiftStyles = SwiftStyles.Default;
customSwiftStyles.Keyword = new Style(Color.Orange1);

AnsiConsole.Console.WriteSwift(swiftCode, customSwiftStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteSwiftAsync(stream);
```
