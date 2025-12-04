---
layout: default
title: "Json"
---

# JSON Syntax Highlighting

The `AnsiConsoleJsonExtensions` class provides extension methods for writing JSON content to the console with syntax highlighting.

## Methods

### WriteJsonAsync (Stream, default styles)

Writes JSON content from a stream to the console with default styling asynchronously.

```csharp
Task<string> WriteJsonAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `stream`: The stream containing JSON content.

**Returns:** The JSON content as a string.

### WriteJsonAsync (Stream, custom styles)

Writes JSON content from a stream to the console with custom styling asynchronously.

```csharp
Task<string> WriteJsonAsync(this IAnsiConsole ansiConsole, Stream stream, JsonStyles jsonStyles)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `stream`: The stream containing JSON content.
- `jsonStyles`: The styles to use for JSON token coloring.

**Returns:** The JSON content as a string.

### WriteJson (Stream, default styles)

Writes JSON content from a stream to the console with default styling synchronously.

```csharp
string WriteJson(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `stream`: The stream containing JSON content.

**Returns:** The JSON content as a string.

### WriteJson (Stream, custom styles)

Writes JSON content from a stream to the console with custom styling synchronously.

```csharp
string WriteJson(this IAnsiConsole ansiConsole, Stream stream, JsonStyles jsonStyles)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `stream`: The stream containing JSON content.
- `jsonStyles`: The styles to use for JSON token coloring.

**Returns:** The JSON content as a string.

### WriteJson (string, default styles)

Writes JSON content from a string to the console with default styling.

```csharp
void WriteJson(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `value`: The JSON string to write.

### WriteJson (string, custom styles)

Writes JSON content from a string to the console with custom styling.

```csharp
void WriteJson(this IAnsiConsole ansiConsole, string value, JsonStyles jsonStyles)
```

**Parameters:**
- `ansiConsole`: The console to write to.
- `value`: The JSON string to write.
- `jsonStyles`: The styles to use for JSON token coloring.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var jsonString = """
    {
        "glossary": {
            "title": "example glossary",
            "GlossDiv": {
                "title": "S",
                "GlossList": {
                    "GlossEntry": {
                        "ID": "SGML",
                        "SortAs": "SGML",
                        "GlossTerm": "Standard Generalized Markup Language",
                        "Acronym": "SGML",
                        "Abbrev": "ISO 8879:1986",
                        "GlossDef": {
                            "para": "A meta-markup language, used to create markup languages such as DocBook.",
                            "GlossSeeAlso": ["GML", "XML"]
                        },
                        "GlossSee": "markup"
                    }
                }
            }
        }
    }
    """;

AnsiConsole.Console.WriteJson(jsonString);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customJsonStyles = JsonStyles.Default;
customJsonStyles.PropertyName = new Style(Color.Orange1);

AnsiConsole.Console.WriteJson(jsonString, customJsonStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteJsonAsync(stream);
```
