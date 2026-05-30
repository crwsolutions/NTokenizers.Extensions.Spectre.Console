---
layout: default
title: "Toml"
---

# TOML Syntax Highlighting

The `AnsiConsoleTomlExtensions` class provides extension methods for writing TOML content to the console with syntax highlighting.

## Methods

### WriteTomlAsync (Stream, default styles)

Writes TOML content from a stream to the console with default styling asynchronously.

```csharp
Task<string> WriteTomlAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `stream`: The stream containing TOML content.

**Returns:** The TOML content as a string.

### WriteTomlAsync (Stream, custom styles)

Writes TOML content from a stream to the console with custom styling asynchronously.

```csharp
Task<string> WriteTomlAsync(this IAnsiConsole ansiConsole, Stream stream, TomlStyles tomlStyles)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `stream`: The stream containing TOML content.
- `tomlStyles`: The styles to use for TOML token coloring.

**Returns:** The TOML content as a string.

### WriteToml (Stream, default styles)

Writes TOML content from a stream to the console with default styling synchronously.

```csharp
string WriteToml(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `stream`: The stream containing TOML content.

**Returns:** The TOML content as a string.

### WriteToml (Stream, custom styles)

Writes TOML content from a stream to the console with custom styling synchronously.

```csharp
string WriteToml(this IAnsiConsole ansiConsole, Stream stream, TomlStyles tomlStyles)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `stream`: The stream containing TOML content.
- `tomlStyles`: The styles to use for TOML token coloring.

**Returns:** The TOML content as a string.

### WriteToml (string, default styles)

Writes TOML content from a string to the console with default styling.

```csharp
void WriteToml(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `value`: The TOML string to write.

### WriteToml (string, custom styles)

Writes TOML content from a string to the console with custom styling.

```csharp
void WriteToml(this IAnsiConsole ansiConsole, string value, TomlStyles tomlStyles)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `value`: The TOML string to write.
- `tomlStyles`: The styles to use for TOML token coloring.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var tomlString = """
    # This is a TOML document

    title = "TOML Example"

    [owner]
    name = "Tom Preston-Werner"
    dob = 1979-05-27T07:32:00-08:00

    [database]
    enabled = true
    ports = [ 8001, 8001, 8002 ]
    data = [ ["delta", "phi"], [3.14] ]
    temp_targets = { cpu = 79.5, case = 72.0 }
    """;

AnsiConsole.Console.WriteToml(tomlString);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customTomlStyles = TomlStyles.Default;
customTomlStyles.Identifier = new Style(Color.Orange1);

AnsiConsole.Console.WriteToml(tomlString, customTomlStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteTomlAsync(stream);
```
