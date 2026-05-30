---
layout: default
title: "Rust"
---

# Rust Syntax Highlighting

The `AnsiConsoleRustExtensions` class provides extension methods for `IAnsiConsole` to render Rust code with syntax highlighting.

## Methods

### WriteRustAsync (Stream, default styles)

Writes Rust code from a stream to the console with default syntax highlighting asynchronously.

```csharp
Task<string> WriteRustAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Rust code to render.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WriteRustAsync (Stream, custom styles)

Writes Rust code from a stream to the console with custom syntax highlighting asynchronously.

```csharp
Task<string> WriteRustAsync(this IAnsiConsole ansiConsole, Stream stream, RustStyles rustStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Rust code to render.
- `rustStyles`: The styles to use for syntax highlighting.

**Returns:** A task representing the asynchronous operation with the parsed string.

### WriteRust (Stream, default styles)

Writes Rust code from a stream to the console with default syntax highlighting synchronously.

```csharp
string WriteRust(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Rust code to render.

**Returns:** The parsed string.

### WriteRust (Stream, custom styles)

Writes Rust code from a stream to the console with custom syntax highlighting synchronously.

```csharp
string WriteRust(this IAnsiConsole ansiConsole, Stream stream, RustStyles rustStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The stream containing Rust code to render.
- `rustStyles`: The styles to use for syntax highlighting.

**Returns:** The parsed string.

### WriteRust (string, default styles)

Writes Rust code from a string to the console with default syntax highlighting.

```csharp
void WriteRust(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The Rust code to render.

### WriteRust (string, custom styles)

Writes Rust code from a string to the console with custom syntax highlighting.

```csharp
void WriteRust(this IAnsiConsole ansiConsole, string value, RustStyles rustStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The Rust code to render.
- `rustStyles`: The styles to use for syntax highlighting.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var rustCode = @"
fn main() {
    println!(""Hello, World!"");
}
";

AnsiConsole.Console.WriteRust(rustCode);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customRustStyles = RustStyles.Default;
customRustStyles.Keyword = new Style(Color.Orange1);

AnsiConsole.Console.WriteRust(rustCode, customRustStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteRustAsync(stream);
```
