---
layout: default
title: "Home"
---

# NTokenizers Documentation

Welcome to the documentation for the `NTokenizers` library. This library provides Spectre.Console rendering extensions for NTokenizers (XML, JSON, Markup, TypeScript, C# and SQL), Style-rich console syntax highlighting.

This library builds on:
- **[Spectre.Console](https://spectreconsole.net/)** for advanced console rendering
- **[NTokenizers](https://github.com/crwsolutions/NTokenizers)** for modular, stream‑based tokenization

Together, they enable expressive syntax highlighting directly in the console.

## Example Usage

```csharp
await AnsiConsole.Console.WriteMarkupTextAsync(stream);
```

> **Especially suitable for parsing AI chat streams**, NTokenizers excels at processing real-time tokenized data from AI models, enabling efficient handling of streaming responses and chat conversations without buffering entire responses.

