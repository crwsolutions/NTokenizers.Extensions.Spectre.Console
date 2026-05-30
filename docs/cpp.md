---
layout: default
title: "C++"
---

# C++ Syntax Highlighting

The `AnsiConsoleCppExtensions` class provides extension methods for `IAnsiConsole` to render C++ code with syntax highlighting.

## Methods

### WriteCppAsync (Stream, default styles)

Writes C++ code from a stream to the console with default syntax highlighting asynchronously.

```csharp
Task<string> WriteCppAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing C++ code to render.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WriteCppAsync (Stream, custom styles)

Writes C++ code from a stream to the console with custom syntax highlighting asynchronously.

```csharp
Task<string> WriteCppAsync(this IAnsiConsole ansiConsole, Stream stream, CppStyles cppStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing C++ code to render.
- `cppStyles`: The styles to use for syntax highlighting.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WriteCpp (Stream, default styles)

Writes C++ code from a stream to the console with default syntax highlighting synchronously.

```csharp
string WriteCpp(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing C++ code to render.

**Returns:** The parsed string.

### WriteCpp (Stream, custom styles)

Writes C++ code from a stream to the console with custom syntax highlighting synchronously.

```csharp
string WriteCpp(this IAnsiConsole ansiConsole, Stream stream, CppStyles cppStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing C++ code to render.
- `cppStyles`: The styles to use for syntax highlighting.

**Returns:** The parsed string.

### WriteCpp (string, default styles)

Writes C++ code from a string to the console with default syntax highlighting.

```csharp
void WriteCpp(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The C++ code to render.

### WriteCpp (string, custom styles)

Writes C++ code from a string to the console with custom syntax highlighting.

```csharp
void WriteCpp(this IAnsiConsole ansiConsole, string value, CppStyles cppStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The C++ code to render.
- `cppStyles`: The styles to use for syntax highlighting.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var cppCode = @"
#include <iostream>

int main() {
    std::cout << ""Hello, World!"" << std::endl;
    return 0;
}
";

AnsiConsole.Console.WriteCpp(cppCode);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customCppStyles = CppStyles.Default;
customCppStyles.Keyword = new Style(Color.Orange1);

AnsiConsole.Console.WriteCpp(cppCode, customCppStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteCppAsync(stream);
```
