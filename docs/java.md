---
layout: default
title: "Java"
---

# Java Syntax Highlighting

The `AnsiConsoleJavaExtensions` class provides extension methods for `IAnsiConsole` to render Java code with syntax highlighting.

## Methods

### WriteJavaAsync (Stream, default styles)

Writes Java code from a stream to the console with default syntax highlighting asynchronously.

```csharp
Task<string> WriteJavaAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Java code to render.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WriteJavaAsync (Stream, custom styles)

Writes Java code from a stream to the console with custom syntax highlighting asynchronously.

```csharp
Task<string> WriteJavaAsync(this IAnsiConsole ansiConsole, Stream stream, JavaStyles javaStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Java code to render.
- `javaStyles`: The styles to use for syntax highlighting.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WriteJava (Stream, default styles)

Writes Java code from a stream to the console with default syntax highlighting synchronously.

```csharp
string WriteJava(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Java code to render.

**Returns:** The parsed string.

### WriteJava (Stream, custom styles)

Writes Java code from a stream to the console with custom syntax highlighting synchronously.

```csharp
string WriteJava(this IAnsiConsole ansiConsole, Stream stream, JavaStyles javaStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Java code to render.
- `javaStyles`: The styles to use for syntax highlighting.

**Returns:** The parsed string.

### WriteJava (string, default styles)

Writes Java code from a string to the console with default syntax highlighting.

```csharp
void WriteJava(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The Java code to render.

### WriteJava (string, custom styles)

Writes Java code from a string to the console with custom syntax highlighting.

```csharp
void WriteJava(this IAnsiConsole ansiConsole, string value, JavaStyles javaStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The Java code to render.
- `javaStyles`: The styles to use for syntax highlighting.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var javaCode = @"
public class Main {
    public static void main(String[] args) {
        System.out.println(""Hello, World!"");
    }
}
";

AnsiConsole.Console.WriteJava(javaCode);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customJavaStyles = JavaStyles.Default;
customJavaStyles.Keyword = new Style(Color.Orange1);

AnsiConsole.Console.WriteJava(javaCode, customJavaStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteJavaAsync(stream);
```
