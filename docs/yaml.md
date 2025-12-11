---
layout: default
title: "Yaml"
---

# Yaml Syntax Highlighting

The `AnsiConsoleYamlExtensions` class provides extension methods for writing Yaml content to the console with syntax highlighting.

## Methods

### WriteYamlAsync (Stream, default styles)

Writes Yaml content from a stream to the console with default styling asynchronously.

```csharp
Task<string> WriteYamlAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `stream`: The stream containing Yaml content.

**Returns:** The Yaml content as a string.

### WriteYamlAsync (Stream, custom styles)

Writes Yaml content from a stream to the console with custom styling asynchronously.

```csharp
Task<string> WriteYamlAsync(this IAnsiConsole ansiConsole, Stream stream, YamlStyles yamlStyles)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `stream`: The stream containing Yaml content.
- `yamlStyles`: The styles to use for Yaml token coloring.

**Returns:** The Yaml content as a string.

### WriteYaml (Stream, default styles)

Writes Yaml content from a stream to the console with default styling synchronously.

```csharp
string WriteYaml(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `stream`: The stream containing Yaml content.

**Returns:** The Yaml content as a string.

### WriteYaml (Stream, custom styles)

Writes Yaml content from a stream to the console with custom styling synchronously.

```csharp
string WriteYaml(this IAnsiConsole ansiConsole, Stream stream, YamlStyles yamlStyles)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `stream`: The stream containing Yaml content.
- `yamlStyles`: The styles to use for Yaml token coloring.

**Returns:** The Yaml content as a string.

### WriteYaml (string, default styles)

Writes Yaml content from a string to the console with default styling.

```csharp
void WriteYaml(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `value`: The Yaml string to write.

### WriteYaml (string, custom styles)

Writes Yaml content from a string to the console with custom styling.

```csharp
void WriteYaml(this IAnsiConsole ansiConsole, string value, YamlStyles yamlStyles)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `value`: The Yaml string to write.
- `yamlStyles`: The styles to use for Yaml token coloring.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var yamlString = """
    ---
    # A sample yaml file
    company: spacelift
    domain:
        - indeed
    tutorial:
        - yaml:
            name: "This is a string"
            type: awesome
            born: 2001
    """;

AnsiConsole.Console.WriteYaml(yamlString);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customYamlStyles = YamlStyles.Default;
customYamlStyles.PropertyName = new Style(Color.Orange1);

AnsiConsole.Console.WriteYaml(yamlString, customYamlStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteYamlAsync(stream);
```
