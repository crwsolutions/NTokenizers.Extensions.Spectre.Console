# NTokenizers.Extensions.Spectre.Console
**Stream-capable** Spectre.Console rendering extensions for NTokenizers (XML, JSON, Markdown, Java-/TypeScript, CSS, HTML, C# and SQL), Style-rich console syntax highlighting

This library builds on:
- **[Spectre.Console](https://spectreconsole.net/)** for advanced console rendering
- **[NTokenizers](https://github.com/crwsolutions/NTokenizers)** for modular, stream‑based tokenization

Together, they enable expressive syntax highlighting directly in the console.

```csharp
await AnsiConsole.Console.WriteMarkdownAsync(stream);
```

> [!WARNING] 
>
> All references to Markup were renamed to Markdown. And so WriteMarkupText was renamed to **WriteMarkdown** in **v2**.

> **Especially suitable for parsing AI chat streams**, NTokenizers excels at processing real-time tokenized data from AI models, enabling efficient handling of streaming responses and chat conversations without buffering entire responses.
> 
> Check out the [AI Example](https://crwsolutions.github.io/NTokenizers.Extensions.Spectre.Console/ai) to learn more about that.

## Example: Writing a 'simple' string

```csharp
var help = """"
           #### Some extra instructions to accomodate multiline input:
           |Command|Description|
           |**Shift-Enter**|to place a soft new line|
           |**Ctrl-B**|to paste multiline, do not use **Ctrl-V**, because that will input a _<Enter>_'s|
           |**bye**|type bye to end the chat session|
           |**clear**|type clear to clear the chat history|
           """";

AnsiConsole.Console.WriteMarkdown(help);
```

Check out the documentation [Here](https://crwsolutions.github.io/NTokenizers.Extensions.Spectre.Console/).
