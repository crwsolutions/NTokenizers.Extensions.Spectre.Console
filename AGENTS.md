# NTokenizers.Extensions.Spectre.Console - Agent Instructions: Adding New Languages

## Goal

Add new language support to the NTokenizers.Extensions.Spectre.Console library.

For each language, three source files must be created following the established pattern:
- **Styles** class (in `Styles/` directory)
- **Writer** class (in `Writers/` directory)
- **Extension** class (in root of the `src/` directory)

Additionally, per language:
- A **Showcase project** demonstrating all extension methods
- A **docs** page following the existing documentation pattern

---

## Reference: C# Pattern

Use the existing C# implementation as the template. Study these three files:

- `src/NTokenizers.Extensions.Spectre.Console/Styles/CSharpStyles.cs`
- `src/NTokenizers.Extensions.Spectre.Console/Writers/CSharpWriter.cs`
- `src/NTokenizers.Extensions.Spectre.Console/AnsiConsoleCSharpExtensions.cs`

And the showcase:
- `tests/NTokenizers.Extensions.Spectre.Console.ShowCase.CSharp/Program.cs`
- `tests/NTokenizers.Extensions.Spectre.Console.ShowCase.CSharp/CSharpExample.cs`

And the docs:
- `docs/csharp.md`

---

## Per-Language Checklist

For each `[Language]`, complete all items below. Replace `[Language]` with the actual language name (e.g., `C`, `Cpp`, `Go`, `Java`, `Kotlin`, `Python`, `Rust`, `Swift`).

### Naming Conventions

| Artifact | Pattern | Example (C) | Example (C++) |
|----------|---------|-------------|---------------|
| Styles class | `[Language]Styles.cs` | `CStyles.cs` | `CppStyles.cs` |
| Writer class | `[Language]Writer.cs` | `CWriter.cs` | `CppWriter.cs` |
| Extension class | `AnsiConsole[Language]Extensions.cs` | `AnsiConsoleCExtensions.cs` | `AnsiConsoleCppExtensions.cs` |
| Extension methods | `Write[Language]` / `Write[Language]Async` | `WriteC` / `WriteCAsync` | `WriteCpp` / `WriteCppAsync` |
| Showcase project | `NTokenizers.Extensions.Spectre.Console.ShowCase.[Language]` | `...ShowCase.C` | `...ShowCase.Cpp` |
| Example class | `[Language]Example.cs` | `CExample.cs` | `CppExample.cs` |
| Docs page | `[language].md` (lowercase) | `c.md` | `cpp.md` |

### Step 1: Styles Class

**Path:** `src/NTokenizers.Extensions.Spectre.Console/Styles/[Language]Styles.cs`

```csharp
using NTokenizers.[Language];
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Styles;

/// <summary>
/// Provides predefined styles for [Language] syntax highlighting in Spectre.Console.
/// </summary>
public sealed class [Language]Styles
{
    /// <summary>
    /// Gets the default [Language] styles.
    /// </summary>
    public static [Language]Styles Default => new();

    // One Style property per TokenType enum value from NTokenizers.[Language].[Language]TokenType
    /// <summary>
    /// Gets or sets the style for [Language] keywords.
    /// </summary>
    public Style Keyword { get; set; } = new Style(Color.Turquoise2);

    // ... one property per enum value ...

    /// <summary>
    /// Gets or sets the style for whitespace characters.
    /// </summary>
    public Style Whitespace { get; set; } = new Style(Color.White);
}
```

**Rules:**
- Load the `[Language]TokenType.cs` file from `src/NTokenizers/[Language]/` in the NTokenizers project
- Create exactly one `Style` property for each enum value (excluding `NotDefined` if preferred, but include it for completeness)
- Every property must have a `/// <summary>` XML doc comment
- Use the color convention table below

**Color Convention:**

| Token category | Default color |
|----------------|---------------|
| Keywords | `Color.Turquoise2` |
| Numbers | `Color.Blue` |
| Strings | `Color.DarkSlateGray1` |
| Comments | `Color.Green` |
| Identifiers | `Color.White` |
| Operators | `Color.DeepSkyBlue4_2` |
| Punctuation (parentheses, brackets, braces, etc.) | `Color.Yellow` |
| Whitespace | `Color.White` |
| Boolean literals | `Color.Blue` |
| Null literals | `Color.Blue` |
| Special (Preprocessor, Lifetime, etc.) | `Color.Magenta` |
| NotDefined | `Color.Gray` |

### Step 2: Writer Class

**Path:** `src/NTokenizers.Extensions.Spectre.Console/Writers/[Language]Writer.cs`

```csharp
using NTokenizers.[Language];
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class [Language]Writer(IAnsiConsole ansiConsole, [Language]Styles styles)
    : BaseInlineWriter<[Language]Token, [Language]TokenType>(ansiConsole)
{
    protected override Style GetStyle([Language]TokenType token) => token switch
    {
        [Language]TokenType.Keyword => styles.Keyword,
        // ... one case per TokenType enum value ...
        [Language]TokenType.Whitespace => styles.Whitespace,
        _ => new Style(Color.White)   // fallback
    };
}
```

**Rules:**
- Must have a `case` for every enum value from `[Language]TokenType`
- The fallback `_ => new Style(Color.White)` must always be present
- No XML docs needed (internal class)

### Step 3: Extension Class

**Path:** `src/NTokenizers.Extensions.Spectre.Console/AnsiConsole[Language]Extensions.cs`

```csharp
using NTokenizers.[Language];
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using Spectre.Console;
using System.Text;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for <see cref="IAnsiConsole"/> to render [Language] code with syntax highlighting.
/// </summary>
public static class AnsiConsole[Language]Extensions
{
    /// <summary>
    /// Writes [Language] code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing [Language] code to render.</param>
    /// <param name="[language]Styles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation with the parsed string.</returns>
    public static async Task<string> Write[Language]Async(
        this IAnsiConsole ansiConsole, Stream stream,
        [Language]Styles? [language]Styles = null, Encoding? encoding = null,
        CancellationToken ct = default)
    {
        var [language]Writer = new [Language]Writer(ansiConsole, [language]Styles ?? [Language]Styles.Default);
        if (encoding is null)
        {
            return await [Language]Tokenizer.Create().ParseAsync(
                stream, ct, [language]Writer.WriteToken);
        }
        else
        {
            return await [Language]Tokenizer.Create().ParseAsync(
                stream, encoding, ct, [language]Writer.WriteToken);
        }
    }

    /// <summary>
    /// Writes [Language] code to the console with syntax highlighting using the specified styles and returns the parsed string.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="stream">The stream containing [Language] code to render.</param>
    /// <param name="[language]Styles">The styles to use for syntax highlighting.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The parsed string.</returns>
    public static string Write[Language](this IAnsiConsole ansiConsole, Stream stream,
        [Language]Styles? [language]Styles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => Write[Language]Async(ansiConsole, stream, [language]Styles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes [Language] code to the console with syntax highlighting using default styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The [Language] code to render.</param>
    public static void Write[Language](this IAnsiConsole ansiConsole, string value) =>
        Write[Language](ansiConsole, value, [Language]Styles.Default);

    /// <summary>
    /// Writes [Language] code to the console with syntax highlighting using the specified styles.
    /// </summary>
    /// <param name="ansiConsole">The <see cref="IAnsiConsole"/> to write to.</param>
    /// <param name="value">The [Language] code to render.</param>
    /// <param name="[language]Styles">The styles to use for syntax highlighting.</param>
    public static void Write[Language](this IAnsiConsole ansiConsole, string value, [Language]Styles [language]Styles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => Write[Language]Async(ansiConsole, stream, [language]Styles, Encoding.UTF8, default));
        t.GetAwaiter().GetResult();
    }
}
```

**Rules:**
- Four public methods per extension class (async stream, sync stream, string default, string custom)
- Every public member must have complete XML documentation (`/// <summary>`, `/// <param>`, `/// <returns>`)
- Use `<see cref="..."/>` for type references in XML docs
- The local variable for the writer instance uses camelCase: `[language]Writer`
- The parameter for styles uses camelCase: `[language]Styles`

### Step 4: Update MarkdownWriter

**Path:** `src/NTokenizers.Extensions.Spectre.Console/Writers/MarkdownWriter.cs`

Add a `using` statement at the top:
```csharp
using NTokenizers.[Language];
```

Add an `else if` branch in the `WriteAsync` method, right before the `GenericCodeBlockMetadata` branch:
```csharp
        else if (token.Metadata is [Language]CodeBlockMetadata [language]Meta)
        {
            var writer = new [Language]Writer(ansiConsole, MarkdownStyles.[Language]Styles);
            await writer.WriteAsync([language]Meta);
        }
```

### Step 5: Update MarkdownStyles

**Path:** `src/NTokenizers.Extensions.Spectre.Console/Styles/MarkdownStyles.cs`

Add a property for the language styles, after the existing language style properties:
```csharp
    /// <summary>
    /// Gets the [Language] styles used for rendering [Language] code in markdown content.
    /// </summary>
    public [Language]Styles [Language]Styles { get; } = [Language]Styles.Default;
```

### Step 6: Showcase Project

**Path:** `tests/NTokenizers.Extensions.Spectre.Console.ShowCase.[Language]/`

Create three files:

**`NTokenizers.Extensions.Spectre.Console.ShowCase.[Language].csproj`:**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\src\NTokenizers.Extensions.Spectre.Console\NTokenizers.Extensions.Spectre.Console.csproj" />
  </ItemGroup>
</Project>
```

**`Program.cs`:** Follow the exact structure of `ShowCase.CSharp/Program.cs`. Replace all C# references with `[Language]` equivalents. The program must demonstrate all 6 usage patterns:
1. `Write[Language]` with string (default styles)
2. `Write[Language]` with string and custom styles
3. `Write[Language]` with Stream (default styles)
4. `Write[Language]` with Stream and custom styles
5. `Write[Language]Async` with Stream (default styles)
6. `Write[Language]Async` with Stream and custom styles

**`[Language]Example.cs`:**
```csharp
namespace NTokenizers.Extensions.Spectre.Console.ShowCase.[Language];

internal static class [Language]Example
{
    internal static string GetSample[Language]() =>
        """
        // Representative [Language] code sample (3-6 lines)
        // Must exercise: keywords, identifiers, strings, numbers, operators, punctuation, comments
        """;
}
```

### Step 7: Docs Page

**Path:** `docs/[language].md`

Follow the structure of `docs/csharp.md`. Include:
- Title and description
- All extension methods with signatures, parameters, and return values
- Basic usage example with string
- Custom styles example
- Async with stream example

---

## Global Checklist (One-Time)

After adding a language, complete these steps:

### 1. Build Temporary NTokenizers NuGet Package
- [ ] Bump version in `src/NTokenizers/NTokenizers.csproj` with `.local` postfix (e.g., `5.0.0-local`)
- [ ] Build the package and output to your local NuGet feed:
  ```bash
  cd /home/colin/Development/crwsolutions/ntokenizers
  dotnet pack src/NTokenizers/NTokenizers.csproj -c Release -o ~/local-nuget
  ```
- [ ] Reference the local package in the Spectre.Console extension project:
  ```bash
  cd /home/colin/Development/crwsolutions/NTokenizers.Extensions.Spectre.Console
  dotnet add src/NTokenizers.Extensions.Spectre.Console/NTokenizers.Extensions.Spectre.Console.csproj package NTokenizers -v <NEW-VERSION> -s ~/local-nuget
  ```
  > **Important:** Always bump the version in `NTokenizers.csproj` before packing. NuGet caches packages by version — if you reuse the same version, the cached (unfixed) package will be used instead of your newly built one. Increment the patch or add a unique suffix (e.g., `5.0.2-local`, `5.0.3-local`).
- [ ] Ensure `NuGet.Config` exists with the local source (use absolute path, `~` is not expanded):
  ```xml
  <?xml version="1.0" encoding="utf-8"?>
  <configuration>
    <packageSources>
      <clear />
      <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
      <add key="local-nuget" value="/home/colin/local-nuget" />
    </packageSources>
  </configuration>
  ```

### 2. Update csproj
- [ ] `src/NTokenizers.Extensions.Spectre.Console/NTokenizers.Extensions.Spectre.Console.csproj`
  - Update `<Description>` to include new languages
  - Update `<PackageTags>` to include new language tags
  - Update NTokenizers package reference to the new version

### 3. Update Solution File
- [ ] `NTokenizers.Extensions.Spectre.Console.slnx`
  - Add all new showcase projects

### 4. Update MarkdownWriter
- [ ] `src/NTokenizers.Extensions.Spectre.Console/Writers/MarkdownWriter.cs`
  - Add `using NTokenizers.[Language];` for each new language
  - Add an `else if` branch for each new language's `CodeBlockMetadata` in `WriteAsync`

### 5. Update MarkdownStyles
- [ ] `src/NTokenizers.Extensions.Spectre.Console/Styles/MarkdownStyles.cs`
  - Add a `[Language]Styles` property for each new language

### 6. Update Documentation
- [ ] `README.md` (root) - update language list in the description line
- [ ] `docs/index.md` - update language list
- [ ] `docs/_config.yml` - add sidebar navigation entry for the new language page:
  ```yaml
  - title: "[Language]"
    url: "[language]"
  ```

### 7. Extend Markdown Showcase
- [ ] `tests/NTokenizers.Extensions.Spectre.Console.ShowCase.Markdown/MarkdownExample.cs`
  - Add a fenced code block for each new language (3-6 lines of representative code)
  - Use the correct fence identifier (e.g., ` ```c `, ` ```cpp `, ` ```go `, etc.)

### 8. Build and Verify
- [ ] Build the full solution: `dotnet build NTokenizers.Extensions.Spectre.Console.slnx`
- [ ] Run each showcase individually to verify output

---

## Important Notes

1. **Each language has different token types.** Read the `[Language]TokenType.cs` enum from the NTokenizers project before creating the Styles class. Do not copy token types from another language -- use the actual enum.

2. **NotDefined token type.** Every language has a `NotDefined` token type. Style it with `Color.Gray` or `Color.White`.

3. **Language-specific token types.** Some languages have unique token types that others do not have (e.g., `Preprocessor` for C, `Lifetime` for Rust, `Hash` for Python). Only include properties for token types that actually exist in that language's enum.

4. **XML documentation is mandatory** for all public API members:
   - `/// <summary>` on every public class, method, and property
   - `/// <param name="...">` on every method parameter
   - `/// <returns>` on every method that returns a value
   - Use `<see cref="..."/>` for type references

5. **Showcase projects must compile and run.** Each showcase should demonstrate all extension method overloads and produce colored output.

---

## Expected File Structure After Implementation

```
src/NTokenizers.Extensions.Spectre.Console/
├── AnsiConsole[Language]Extensions.cs      [NEW per language]
├── Styles/
│   └── [Language]Styles.cs                 [NEW per language]
└── Writers/
    └── [Language]Writer.cs                 [NEW per language]

tests/
└── NTokenizers.Extensions.Spectre.Console.ShowCase.[Language]/
    ├── NTokenizers.Extensions.Spectre.Console.ShowCase.[Language].csproj
    ├── Program.cs
    └── [Language]Example.cs

docs/
└── [language].md                            [NEW per language]
```

**Total new files per language:** 3 (src) + 3 (showcase) + 1 (docs) = 7 files
**For 8 languages:** 56 new files + 6 files to modify (csproj, slnx, MarkdownWriter.cs, MarkdownStyles.cs, MarkdownExample.cs, docs/index.md)
