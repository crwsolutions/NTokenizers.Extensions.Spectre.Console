---
layout: default
title: "Home"
---

# NTokenizers.Extensions.Spectre.Console Documentation

Welcome to the documentation for the `NTokenizers.Extensions.Spectre.Console` library. This library provides a NTokenizers rendering extension for Spectre.Console. This packages renders Style-rich console syntax highlighting for XML, JSON, Markup, TypeScript, C# and SQL.

This library builds on:
- **[Spectre.Console](https://spectreconsole.net/)** for advanced console rendering
- **[NTokenizers](https://github.com/crwsolutions/NTokenizers)** for modular, stream‑based tokenization

Together, they enable expressive syntax highlighting directly in the console.

## Example Usage

```csharp
await AnsiConsole.Console.WriteMarkupTextAsync(stream);
```

> **Especially suitable for parsing AI chat streams**, NTokenizers excels at processing real-time tokenized data from AI models, enabling efficient handling of streaming responses and chat conversations without buffering entire responses.

