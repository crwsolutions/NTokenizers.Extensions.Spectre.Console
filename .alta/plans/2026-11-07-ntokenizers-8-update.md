---
title: Upgrade to NTokenizers 8.0 and style the new BoldItalic and Strikethrough tokens
status: done
created: 2026-11-07
summary: Bump the NTokenizers reference from 7.* to 8.* and add styles for the two new Markdown token types (BoldItalic, Strikethrough) that 8.0 introduces, so they render styled instead of falling back to the default style.
---

# NTokenizers 8.0 upgrade

NTokenizers 8.0 (published 2026-11-06) adds two Markdown features: `***bold italic***` combined emphasis (`MarkdownTokenType.BoldItalic`) and `~~strikethrough~~` (`MarkdownTokenType.Strikethrough`), plus ToHtml-only changes (returning consumed Markdown from `WriteHtmlAsync`, semantic HTML elements). Today this repo compiles clean against 8.0.0, but the two new token types are unknown to the Markdown style lookup, so `***text***` and `~~text~~` inside paragraphs and table cells silently render with the default (unstyled) look. After this work the reference is on `8.*`, both tokens have dedicated styles, and the showcase demonstrates them.

## What changes

### Package reference

[src/NTokenizers.Extensions.Spectre.Console.csproj](src/NTokenizers.Extensions.Spectre.Console/NTokenizers.Extensions.Spectre.Console.csproj):

```xml
<!-- before -->
<PackageReference Include="NTokenizers" Version="7.*" />

<!-- after -->
<PackageReference Include="NTokenizers" Version="8.*" />
```

`<Version>` stays `3.0` (the package version is not part of this change); `PackageReleaseNotes` is updated to describe the 8.0 upgrade and the two new token styles.

### New styles for the two token types

[MarkdownStyles.cs](src/NTokenizers.Extensions.Spectre.Console/Styles/MarkdownStyles.cs) — two new properties next to `Bold`/`Italic`, and two cases in `GetStyleForToken`. The colors follow the existing conventions of the file (Bold is blue + bold decoration, TypographicReplacement is grey):

```csharp
/// <summary>
/// Gets or sets the style for bold italic text in markdown content.
/// </summary>
public Style BoldItalic { get; set; } = new Style(Color.Blue, decoration: Decoration.Bold | Decoration.Italic);

/// <summary>
/// Gets or sets the style for strikethrough text in markdown content.
/// </summary>
public Style Strikethrough { get; set; } = new Style(Color.Grey, decoration: Decoration.Strikethrough);
```

```csharp
public Style GetStyleForToken(MarkdownTokenType tokenType) => tokenType switch
{
    MarkdownTokenType.Bold => Bold,
    MarkdownTokenType.BoldItalic => BoldItalic,   // NEW
    MarkdownTokenType.Italic => Italic,
    ...
    MarkdownTokenType.Strikethrough => Strikethrough, // NEW
    ...
    _ => DefaultStyle
};
```

`GetStyleForToken` is the single lookup used by [MarkdownWriter.WriteRootInline](src/NTokenizers.Extensions.Spectre.Console/Writers/MarkdownWriter.cs#L228) and [MarkdownBlockContext](src/NTokenizers.Extensions.Spectre.Console/Writers/MarkdownBlockContext.cs#L243) (paragraphs, headings, quotes, list items), so those contexts pick the new styles up automatically.

### Table cells

[MarkdownTableWriter.WriteCell](src/NTokenizers.Extensions.Spectre.Console/Writers/MarkdownTableWriter.cs#L81) keeps its own small switch (Bold, Italic, CodeInline, Link). Add the two cases:

```csharp
var style = token.TokenType switch
{
    MarkdownTokenType.Bold => markdownStyles.Bold,
    MarkdownTokenType.BoldItalic => markdownStyles.BoldItalic,   // NEW
    MarkdownTokenType.Italic => markdownStyles.Italic,
    MarkdownTokenType.Strikethrough => markdownStyles.Strikethrough, // NEW
    MarkdownTokenType.CodeInline => markdownStyles.CodeInline,
    MarkdownTokenType.Link => markdownStyles.Link,
    _ => markdownStyles.TableCell
};
```

### Showcase and docs example

[MarkdownExample.cs](tests/NTokenizers.Extensions.Spectre.Console.ShowCase.Markdown/MarkdownExample.cs) already contains `~~Strikethrough~~` in its Emphasis section; add the combined-emphasis line so both new features are visible in the output:

```
## Emphasis

**This is bold text**
__This is bold text__
*This is italic text*
_This is italic text_
***This is bold italic text***   <- new
~~Strikethrough~~
```

The same line goes into the usage sample in [docs/markdown.md](docs/markdown.md) (its Emphasis block already shows `~~Strikethrough~~`).

## Steps

- [x] 1. Bump `NTokenizers` to `8.*` in [src/NTokenizers.Extensions.Spectre.Console.csproj](src/NTokenizers.Extensions.Spectre.Console/NTokenizers.Extensions.Spectre.Console.csproj) and rewrite `PackageReleaseNotes` for the 8.0 upgrade.
- [x] 2. Add `BoldItalic` and `Strikethrough` properties plus their `GetStyleForToken` cases in [MarkdownStyles.cs](src/NTokenizers.Extensions.Spectre.Console/Styles/MarkdownStyles.cs).
- [x] 3. Add `BoldItalic` and `Strikethrough` cases to the switch in [MarkdownTableWriter.WriteCell](src/NTokenizers.Extensions.Spectre.Console/Writers/MarkdownTableWriter.cs).
- [x] 4. Add the `***This is bold italic text***` line to the Emphasis section of [MarkdownExample.cs](tests/NTokenizers.Extensions.Spectre.Console.ShowCase.Markdown/MarkdownExample.cs) and to the sample in [docs/markdown.md](docs/markdown.md).
- [x] 5. Build the full solution and run the Markdown showcase (see below).

## How it is verified

- [x] `dotnet build NTokenizers.Extensions.Spectre.Console.slnx` succeeds with 0 warnings / 0 errors (restore resolves NTokenizers 8.0.0). Confirmed: `dotnet list package` shows NTokenizers `8.* -> 8.0.0`.
- [x] Rendering check: a scratch console app (Ansi forced on) renders `***This is bold italic text***` as `\x1b[1;3;38;5;12m` (bold + italic + blue) and `~~Strikethrough~~` as `\x1b[9;38;5;8m` (strikethrough + grey); the existing `**bold**` stays `\x1b[1;38;5;12m` (blue + bold). Style objects resolve to `Blue/Bold|Italic` and `Grey/Strikethrough` respectively.

Note: the full Markdown showcase cannot run to completion when stdout is redirected on Windows because the table live-region hides the cursor (`ConsolePal.set_CursorVisible` throws `IOException: The handle is invalid`). This is a pre-existing Spectre.Console redirection limitation in `MarkdownTableWriter`, unrelated to this change; the emphasis/strikethrough lines themselves render correctly before the crash.

## Decisions

| Decision | Choice | Why |
| --- | --- | --- |
| Version reference | floating `8.*`, not pinned `8.0.0` | Matches the existing `7.*` pattern in the csproj. |
| BoldItalic style | blue + bold + italic decoration | Consistent with the existing `Bold` (blue + bold) convention in `MarkdownStyles`. |
| Strikethrough style | grey + strikethrough decoration | Consistent with `TypographicReplacement` (grey); the decoration carries the meaning. |
| Package `<Version>` | stays `3.0` | Version numbering of the extension package is a release decision, not part of this dependency update. |
| ToHtml changes in 8.0 | ignored | This repo consumes the tokenizer directly and does not use the `ToHtml` API. |

## Risks

- None material: the build against 8.0.0 is already verified clean for all 20 projects; the only behavior change is styling of two previously unstyled tokens.

## Out of scope

- The ToHtml API changes of NTokenizers 8.0 (returning consumed Markdown, semantic HTML elements, autoscroll fix) — no consumer of them in this repo.
- Regenerating `docs/assets/animated-demo.svg`.
- A package version bump or publishing.
