---
layout: default
title: "Python"
---

# Python Syntax Highlighting

The `AnsiConsolePythonExtensions` class provides extension methods for `IAnsiConsole` to render Python code with syntax highlighting.

## Methods

### WritePythonAsync (Stream, default styles)

Writes Python code from a stream to the console with default syntax highlighting asynchronously.

```csharp
Task<string> WritePythonAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Python code to render.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WritePythonAsync (Stream, custom styles)

Writes Python code from a stream to the console with custom syntax highlighting asynchronously.

```csharp
Task<string> WritePythonAsync(this IAnsiConsole ansiConsole, Stream stream, PythonStyles pythonStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Python code to render.
- `pythonStyles`: The styles to use for syntax highlighting.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WritePython (Stream, default styles)

Writes Python code from a stream to the console with default syntax highlighting synchronously.

```csharp
string WritePython(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Python code to render.

**Returns:** The parsed string.

### WritePython (Stream, custom styles)

Writes Python code from a stream to the console with custom syntax highlighting synchronously.

```csharp
string WritePython(this IAnsiConsole ansiConsole, Stream stream, PythonStyles pythonStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Python code to render.
- `pythonStyles`: The styles to use for syntax highlighting.

**Returns:** The parsed string.

### WritePython (string, default styles)

Writes Python code from a string to the console with default syntax highlighting.

```csharp
void WritePython(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The Python code to render.

### WritePython (string, custom styles)

Writes Python code from a string to the console with custom syntax highlighting.

```csharp
void WritePython(this IAnsiConsole ansiConsole, string value, PythonStyles pythonStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The Python code to render.
- `pythonStyles`: The styles to use for syntax highlighting.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var pythonCode = @"
def main():
    print(""Hello, World!"")

if __name__ == ""__main__"":
    main()
";

AnsiConsole.Console.WritePython(pythonCode);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customPythonStyles = PythonStyles.Default;
customPythonStyles.Keyword = new Style(Color.Orange1);

AnsiConsole.Console.WritePython(pythonCode, customPythonStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WritePythonAsync(stream);
```
