---
layout: default
title: "Typescript"
---

# TypeScript Syntax Highlighting

The `AnsiConsoleTypescriptExtensions` class provides extension methods for `IAnsiConsole` to render TypeScript code with syntax highlighting.

## Methods

### WriteTypescriptAsync (Stream, default styles)

Writes TypeScript code from a stream to the console with default styling asynchronously.

```csharp
Task<string> WriteTypescriptAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The `Stream` containing TypeScript code.

**Returns:** A task that represents the asynchronous operation. The result is the input stream as a string.

### WriteTypescriptAsync (Stream, custom styles)

Writes TypeScript code from a stream to the console with custom styling asynchronously.

```csharp
Task<string> WriteTypescriptAsync(this IAnsiConsole ansiConsole, Stream stream, TypescriptStyles typescriptStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The `Stream` containing TypeScript code.
- `typescriptStyles`: The `TypescriptStyles` to use for styling.

**Returns:** A task that represents the asynchronous operation. The result is the input stream as a string.

### WriteTypescript (Stream, default styles)

Writes TypeScript code from a stream to the console with default styling synchronously.

```csharp
string WriteTypescript(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The `Stream` containing TypeScript code.

**Returns:** The input stream as a string.

### WriteTypescript (Stream, custom styles)

Writes TypeScript code from a stream to the console with custom styling synchronously.

```csharp
string WriteTypescript(this IAnsiConsole ansiConsole, Stream stream, TypescriptStyles typescriptStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The `Stream` containing TypeScript code.
- `typescriptStyles`: The `TypescriptStyles` to use for styling.

**Returns:** The input stream as a string.

### WriteTypescript (string, default styles)

Writes TypeScript code from a string to the console with default styling.

```csharp
void WriteTypescript(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The TypeScript code as a string.

### WriteTypescript (string, custom styles)

Writes TypeScript code from a string to the console with custom styling.

```csharp
void WriteTypescript(this IAnsiConsole ansiConsole, string value, TypescriptStyles typescriptStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The TypeScript code as a string.
- `typescriptStyles`: The `TypescriptStyles` to use for styling.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var typescriptCode = """
    // Single-line comment: Declare variables
    const message = "Hello, \"world\"!"; // Escaped double quotes
    let age = 25; // Dynamic variable
    var isActive = true; // Boolean value

    /*
    Multi-line comment:
    This block showcases variable declaration,
    string manipulation, and escaping characters.
    */
    const multiLineString = `This is a multiline
    string with embedded \`special characters\`.`;

    function greet(name: string) {
        // Template literal for string interpolation
        const greeting = `Hi, ${name}!`;
        return greeting;
    }

    // Control structure: if-else statement
    if (isActive && age > 18) {
        console.log(message);
        console.log(greet("Alice"));
    } else {
        console.warn('Inactive or age is below threshold.');
    }

    // Array and map
    const numbers: number[] = [1, 2, 3, 4, 5];
    const squared = numbers.map(num => num ** 2);

    // Output using template literals
    console.log(`Squared values: ${squared.join(", ")}`);
    """;

AnsiConsole.Console.WriteTypescript(typescriptCode);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customTypescriptStyles = TypescriptStyles.Default;
customTypescriptStyles.Keyword = new Style(Color.Orange1);

AnsiConsole.Console.WriteTypescript(typescriptCode, customTypescriptStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteTypescriptAsync(stream);
```
