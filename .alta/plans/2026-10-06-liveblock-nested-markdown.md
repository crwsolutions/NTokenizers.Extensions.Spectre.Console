# LiveBlock — geneste markdown-blocken renderen in Spectre.Console

- Status: Done
- Plan file: `.alta/plans/2026-10-06-liveblock-nested-markdown.md`
- Created: 2026-10-06
- Task: Bouw een growable `LiveBlock`-IRenderable (stackmachine, oneindige nestingsdiepte) en koppel de `MarkdownWriter`-dispatch daarop aan, zodat geneste blockquotes/lijsten/codeblok-kens correct live renderen tegen NTokenizers 7.0.
- Git: not ignored (`.alta/` staat niet in .gitignore) → dit plan-bestand meecommen met de gerelateerde implementatie.

## Architecture change (2026-10-06) — van `Live`-regio's naar forward-only (Colin)

**Besluit (Colin):** live rendering met cursor-repositioning is niet nodig; de renderer schrijft forward-only. De `Live`-regio-architectuur hieronder (Design notes 5, de region-lifecycle in `MarkdownWriter`) is **vervangen**.

**Reden (gegronde met Spectre 0.55.2-sources).** `LiveRenderable.PositionCursor` wordt per frame uitgevoerd (`\r` + `CursorUp`) en heeft een reset-tak die `EraseInDisplay(2) + ClearScrollback() + CursorHome()` doet — de regio rendert relatief aan cursor-state die de renderer niet bezit → de waargenomen (0,0)-overlapping. `LiveDisplayRenderer.Started()` roept `Cursor.Hide()` → `System.Console.CursorVisible` → `IOException` bij geredirigeerde stdout. Symptoom (Colin, `## Text`): blok geschreven op positie 0,0, overwrote de header `=== ` → `TextWriteMarkdown with string (default styles) ===` / `----`.

**Nieuwe architectuur (forward-only).**
- `LiveBlock` blijft een growable `IRenderable` (rendering ongewijzigd: randen, gutters, nesting, breedte-budget).
- Geen `LiveDisplay` meer voor quotes/lijsten/code/headings/HR: een root-block bouwt zijn `LiveBlock` terwijl de content streamt (sub-document via inline-handler) en wordt **één keer** `console.Write(liveBlock)` + `WriteLine()` wanneer het blok compleet is.
- Token-callback wordt **synchron** (`token => markdownWriter.Write(token)`) → geen fire-and-forget `async void`, geen barrier, geen `_startTask`/`_contentDone`/region-lifecycle. Alles draait in documentvolgorde op de parse-thread;exceptions propageren normaal.
- Token-flow (empirisch vastgesteld, handler-logging): een lijst is exact `ListStart → (UnorderedListItem|OrderedListItem)* → ListEnd`; item-content streamt als sub-document via de item-handler (commit = item complete). Root-tokens arriveren in documentvolgorde; sub-document-completie (commit) komt vóór het volgende root-token.
- Root plain paragrafen: ongewijzigd (direct `console.Write(paragraph)`).
- **Tabel** blijft `MarkdownTableWriter` ongewijzigd (eigen `Live`, leaf-block; uitgesloten per scope).
- Voordeel: (0,0)-klasse en redirected-stdout-crash verdwijnen; verificatie door echte output te geredirigeren naar een bestand en te diffen.
- Aangenomen cosmetic: gutter-schommeling bij geordende lijsten > 9 items (R3); de in-place gutter-redraw van `Live` gaat verloren (acceptabel).

**Scope van de rework:** `MarkdownWriter.cs` (region/barrier/lifecycle verwijderen, forward-only block-write, synchrone `Write`), `MarkdownBlockContext.cs` (region-close → "blok schrijven", refresh-plumbing verwijderen), `AnsiConsoleMarkdownExtensions.cs` (synchrone callback, geen `CloseRegionAsync`). `LiveBlock.cs` ongewijzigd.

**Implementatie (2026-10-06, voltooid & geverifieerd).**
- `MarkdownWriter.cs`: `OpenRegion`/`MarkContentDone`/`_closeRegionNow`/`CloseRegionAsync`/barrier/`_pending`/`_startTask`/`_contentDone`/`_blockDone` allemaal verwijderd. Nieuw: `OpenBlock(token)` (maakt `LiveBlock` + root-context met `ownsRegion:true`) en `CompleteBlock()` (`console.Write(_rootBlock)` + `WriteLine()`). Synchrone `Write(MarkdownToken)`. Token-callback is synchrone `Action<MarkdownToken>` (geen fire-and-forget).
- `MarkdownBlockContext.cs`: `_refresh`/`Refresh()`/`refresh`-param verwijderd (13 `Refresh()`-calls eruit); `regionClose`/`ownsRegion`-mechanisme behouden (nu "blok schrijven" i.p.v. "Live-regio sluiten"). Constructor-callers aangepast.
- `AnsiConsoleMarkdownExtensions.cs`: callback `token => markdownWriter.Write(token)` (synchrone); `CloseRegionAsync`-call verwijderd.
- `LiveBlock.cs`, `MarkdownTableWriter.cs`, `BaseInlineWriter.cs` ongewijzigd (tabel houdt eigen `Live`-regio — out of scope).

**Verificatie (forward-only, 2026-10-06).**
- `dotnet build NTokenizers.Extensions.Spectre.Console.slnx` → 0 fouten.
- **Geredirigeerde-stdout-scenario (de oude crash) werkt nu**: `dotnet run … 1> out.txt` → exit 0, geen `CursorVisible`/`IOException` (forward-only roept `Cursor.Hide()` niet aan).
- **(0,0)-positiebug opgelost**: met een al aanwezige header `=== WriteMarkdown … ===` + `## Text` rendert de heading nu **onder** de header (`Text` / `----`), niet meer op positie 0,0 die de header overwrote (symptoom van Colin).
- Nested sample geverifieerd (ANSI-afgevlakt): `** Heading **`+`=====`; `│ A root blockquote.` (groene rand, géén `>`); `csharp:`-label boven de gerande code; ` - ` / `  1. `-gutters; genest `│ outer` → `│ │ inner` → `│ │   1. nested` / `│ │ csharp:` / `│ │ │ var x = 1;` / `│ │   2. next`.
- **Residual (tensie, geen blocker voor real-terminal):** tabel blijft `Live` (eigen regio). Geredirigeerde verificatie kan géén tabel bevatten (crasht op `Cursor.Hide`); op een echt terminal werkt de tabel. `ShowCase.Markdown` draait op een echt terminal (Colin) → tabel OK.
- **Open (Colin):** `MarkdownExample.cs` is in de working tree een 3-regels debug-fragment (`> A` / `B` / `> C`); git HEAD heeft de volledige 452-regels sample (incl. tabel). Moet worden hersteld (volledig + nested-voorbeeld, plan-stap 14) of door Colin gekozen.

## Resolution (2026-10-06) — multi-block hang / NRE opgelost

**Symptoom.** `ShowCase.Markdown` en de headless `tmp/verify`-harness hielden intermitterend op met een `NullReferenceException` of deadlocked bij meer dan één root-block; een enkele run lukte soms wel.

**Root cause (empirisch vastgesteld met `tmp/tokens` + `MdDiag`).** De live-regio van een blok wordt opengezet door de display-actie te laten wachten op een *content-complete* signaal (`_contentDone`). Dat signaal werd uitsluitend gevreesd door `MarkContentDone` — dat pas bij het commit van het buitenste blok of `ListEnd` vuurde. Maar bij een vlakke lijst kan de tokenizer een volgend root-`Text`-token streamen terwijl de lijst-regio nog open staat (de renderer zag `blockDone=false` met een open regio) en vroeg toen de regio te sluiten. Die close deed `await _startTask`, maar de display-actie wachtte voor eeuwig op een compleet-signaal dat nooit zou komen → hang.

**Fix (minimale lifecycle-correctie in `MarkdownWriter.cs`).** Het compleet-signaal is nu een `TaskCompletionSource<bool>` (idempotent: dubbel `TrySetResult` is een no-op, in tegenstelling tot `SemaphoreSlim.Release` dat zou gooien) in plaats van een `SemaphoreSlim`. `MarkContentDone` zet het als het blok natuurlijk afloopt; de teardown (`_closeRegionNow`) zet het **onvoorwaardelijk vóórdat** de display-actie wordt afgewacht. Hierdoor kan `await _startTask` nooit meer hangen op een signaal dat niet komt, ongeacht of het blok al dan niet natuurlijk is afgerond.

**Verificatie.** `dotnet build NTokenizers.Extensions.Spectre.Console.slnx` → 0 fouten / 0 warnings. Headless `tmp/verify` (`Spectre.Console.Testing` `TestConsole`): 150/150 én 100/100 én 50/50 iteraties `ok`, geen NRE, geen timeout, geen deadlock. Layout geverifieerd: `csharp:`-label binnen de regio (geen console-direct-write), groene `│`-quote-rand (dubbel `│ │` voor geneste quote), `- `/` 1. `-gutter, `=====`-onderstreping van headings, géén `>`-quote-markers. `MdDiag`-diagnostics verwijderd uit productiesources. `tmp/` blijft geïgnoreerd (niet committen).

> **Bekend detail (geen blocker, residual).** De `TestConsole`-captuur toont herhalingen omdat `TestConsole.Output` elke live-regio `Refresh()`-frame bijvoegt; op een écht terminal overwrite de ANSI-cursor-opmaak in place, dus is alleen de laatste frame zichtbaar. De content van de laatste frame is correct. Eindtoets tegen stabiel `NTokenizers 7.0.0` (csproj-reference dan op `7.0.0`) blijft staan zodra die op NuGet staat (R4).


## Final state (2026-10-07) — per-token streaming (Option B), voltooid & geverifieerd

**Besluit (Colin, 2026-10-07).** De forward-only `LiveBlock`-buffer (bouwen-als-blok-compleet, één `console.Write`) wordt vervangen door **per-token direct schrijven**: elk token wordt geschreven zodra het aankomt, met de linkerkant (randen, gutters, markers) samengesteld uit de actieve nestingsstapel, en een regelewissel alleen wanneer een token niet meer past op de lopende regel. **Tabel blijft ongewijzigd** (eigen `Live`/growing regio, `MarkdownTableWriter`).

**Reden.** Buffer-then-render toont blok-content pas wanneer het blok compleet is; per-token streaming toont content zo snel als de tokenizer stroomt, zonder flickering (geen cursor-repositioning) en zonder de (0,0)-bug-klasse (forward-only, één keer per token).

**Nieuwe renderlaag: `src/.../Writers/MarkdownStream.cs` (nieuw bestand).**
- Eén gedeelde `MarkdownStream` per root-blok; een stapel van `BlockFrame`-bijdragen ("linkerkant") samengesteld uit de actieve nesting-laag.
- `BlockFrame` (sealed, één model — geen subtype-hiërarchie): `Style`, `FirstLine`, `Continuation`, `FirstLineDone`. Border (quote/code) draagt dezelfde segment op elke regel; een item draagt zijn marker op de eerste regel en gutter-spaces daarna.
- `PushBorder(style, border)`, `PushItem(style, marker)`, `Pop()`. **Frames worden opgeslagen in een `List` (push-volgorde = outermost first).** Dit was de cruciale bug-fix: met een `Stack<T>` enumererde .NET top-first (innermost first), waardoor de prefix in omgekeerde volgorde werd geschreven (genest item ` -    nested a` i.p.v. `    - nested a`).
- Per-token `Write(text, style)`: split op `\n`; per segment `EnsurePrefixed()` (schrijf de samengestelde linkerkant als nog niet geschreven), en wrap alleen wanneer `_content > 0 && _column + part.Length > Width` (een segment breder dan de hele regel overloopt gewoon, wordt niet gesplitst).
- `Width` is **cached** en veerkrachtig: `Console.IsOutputRedirected ? 80 : WindowWidth`, met een `try/catch`-fallback op 80. Dit lost de `System.Console.WindowWidth` → `IOException ("The handle is invalid.")` op geredirigeerde stdout (een productie-issue, niet alleen test-harness).
- `BlankLine()` (linkerkant-alleen regel = blokscheiding), `Finish()` (laatste regel afsluiten), `EnsureNewLine()`, `AvailableWidth` (gebruikt door HR).

**Herkabeld naar `MarkdownStream`:**
- `MarkdownBlockContext.cs` — gebruikt nu `MarkdownStream` i.p.v. de oude `LiveBlock`-buffer; duwt/popt frames op de gedeelde stream per `BlockKind` (Quote/Code/Item); `WriteToken` streamt per token; `Commit()` poppt de frame van het blok.
- `MarkdownWriter.cs` — root-blok lifecycle via `MarkdownStream` (`OpenBlock` maakt een `new MarkdownStream(_ansiConsole)`); root plain paragrafen blijven **direct** (onveranderd); `WireFencedCode` streamt via `WriteToStream`.
- `BaseInlineWriter.cs` / `HtmlWriter.cs` — fenced-code en nested HTML `<style>`/`<script>` content streamen via `WriteToStream(MarkdownStream, token)` (raw text, geen dubbele escaping — de stream escape zelf).

**Wijzigde bestanden (t.o.v. `f496744`):** `MarkdownStream.cs` (nieuw), `MarkdownBlockContext.cs`, `MarkdownWriter.cs`, `BaseInlineWriter.cs`, `HtmlWriter.cs`, `tests/.../ShowCase.Markdown/Program.cs` (door Colin gecommentarieerde showcase-methoden — intentief, niet aangerakt). **Niet aangerakt:** `MarkdownTableWriter.cs` (0 diff), `AnsiConsoleMarkdownExtensions.cs` (async callback-model ongewijzigd), `LiveBlock.cs`.

**Verificatie (2026-10-07).**
- `dotnet build NTokenizers.Extensions.Spectre.Console.slnx` → 0 fouten / 0 warnings.
- **Volledige pariteit met `f496744`**: de nieuwe streaming-output komt regel-voor-regel overeen met de oude buffer-then-render-output voor het volledige nested sample (heading+`===`, quote `│`, lijst `+ item` / `    - nested`, `csharp:`-label + gerande code, root-spacing, geen trailing blank line). Onafhankelijk bevestigd door de oude `f496744` via een `git worktree` te renderen en te diffen (zelfde NTokenizers `7.0.0-local.5`).
- **Redirected-stdout crash opgelost**: het nested sample rendert nu netjes naar een geredirigeerde stdout (voorheen crashte op `WindowWidth`).
- **HTML `<style>`/`<script>`**: 77 ms (geen 2s handler-wait), nested handlers geregistreerd, content en closing tags geredend.
- **Nested-content timing**: 30 ms (per-token, geen buffering tot blokkeinde).
- `tmp/` blijft geïgnoreerd/scratch; tijdelijke `baseline.txt`/`trace.txt`/`new*.txt` opgeruimd; git-status toont alleen de beoogde bronveranderingen + `MarkdownStream.cs`.

**Residual / apart (niet in deze scope).**
- `LiveBlock.cs` is na de streaming-rewrite **dead code** (alleen nog eigen factory's + doc-comments; geen call-sites meer in de markdown-pad — de tabel gebruikt `MarkdownTableWriter`, niet `LiveBlock`). Geïntroduceerd in `8fa0805` (vóór deze task). Verwijderen is een aparte opruiming; per "meest consistente, kleinste verandering" en "niet onverwacht verwijderen" niet automatisch verwijderd — door Colin bekrachtigen.
- `BaseInlineWriter.AppendToken` lijkt onbruikt (alleen `WriteToStream`/`WriteTokenInLiveTarget` worden gebruikt). Eveneens apart.
- De bekende (losse, niet-deze-pad) crash: een `Live`-tabel crasht op geredirigeerde stdout (`Cursor.Hide`/`Console`-handle) — daarom staat de tabel niet in de headless-captuur; op een echt terminal werkt de tabel (Colin).
- Eindcheck tegen stabiel NTokenizers `7.0.0` (csproj dan op `7.0.0`) blijft staan zodra die op NuGet staat.


## Objective

- Geneste markdown-blocken (quotes, lijsten, code) renderen als één growable `LiveBlock`-boom met één `Live`-regio per buitenste block, zonder geneste `Live`-instanties voor quotes/lijsten/code.
- Root-level plain paragrafen blijven plat en direct op de console; headings, HR, tables blijven bestaand gedrag (alleen gerouteerd door de nieuwe dispatcher).
- Het taal-label van codeblocs komt via het block-model in de juiste (geneste) context; console-direct-writes verdwijnen (`MarkdownWriter.cs:43`).
- Niet-doelen: tables in geneste contexten, custom containers, footnotes, ToHtml-pipeline, versie-bump/pakket van dit project.

## Context and evidence

**Dit repo**
- `src/.../Writers/MarkdownWriter.cs` — vlakke if/else-dispatch; `liveTarget`/`Paragraph`-plumbing (`WriteAsync(Paragraph?, token, defaultStyle)`, regel 38); taal-label via `ansiConsole.Write(new Text($"{code}:\n"))` (regel 43) — ontsnapt aan Live-regio (bug 9 uit de brainstorm).
- `src/.../Writers/BaseInlineWriter.cs` — huidige Live-per-blok constructie: `new LiveDisplay(_ansiConsole, GetIRendable())`, handler registreert per inline token, `ctx.Refresh()` per token; `FinalizeAsync`-hook. Wordt vervangen door context-gebaseerde appends (zie Design notes 3).
- `src/.../Writers/MarkdownBlockquoteWriter.cs` — `BaseInlineWriter`-afgeleide; `GetIRendable()` = `Panel(_liveParagraph).Border(new LeftBoxBorder()).BorderStyle(new Style(Color.Green))`.
- `src/.../Writers/LeftBoxBorder.cs` — `BoxBorder` met alleen linker `│`, overige delen ` `.
- `src/.../Writers/MarkdownListItemWriter.cs` / `MarkdownOrderedListItemWriter.cs` — schrijven marker direct naar console + inline-handler + `"\n" //HACK` (regel 14/12). Geen Live-regio, dus onbruikbaar in geneste contexten.
- `src/.../Writers/MarkdownHeadingWriter.cs` — heading-styling per level, `_lenght`-telling voor de onderstreping, level-1 `** … **`-decoratie.
- `src/.../Writers/MarkdownTableWriter.cs` — eigen `Live`-regio met Spectre-`Table`; blijft ongewijzigd (ook in geneste context: sub-`Live` is toegestaan door Spectre).
- `src/.../Styles/MarkdownStyles.cs` — publieke styles; `MarkdownListItemStyles.Marker` = Turquoise2; er is nog geen style voor quote-/code-rand of gutter.
- `tests/.../ShowCase.Markdown/MarkdownExample.cs` — heeft al geneste lijsten (2-niveau) en een plain quote; het nested `> > 1. ~~~csharp`-voorbeeld ontbreekt.
- `tests/.../ShowCase.Markdown/Program.cs` + `NTokenizers.Extensions.Spectre.Console.csproj` — **working tree is al aangepast (door Colin)**: reference staat op `NTokenizers 7.0.0-local.4` en showcase-methoden 2–6 zijn weggecommentarieerd; `tmp/v7.txt` bevat een oude capture. Dit zijn intentieve POC-wijzigingen: erop voortbouwen, niet terugdraaien.
- Geen `NuGet.Config` in repo; `7.0.0-local.*` packages zitten al in de global cache (`~/.nuget/packages/ntokenizers/`).

**NTokenizers (read-only, `C:\Users\colin\source\repos\crwsolutions\ntokenizers`, versie `7.0-local.4`, dirty worktree)**
- `Languages/Markdown/MarkdownTokenType.cs` — v7-enum met o.a. `ParagraphBlockStart`, `ParagraphBlockEnd`, `ListStart`, `ListEnd`, `IndentedCodeBlock`, `Blockquote`.
- `Languages/Markdown/MarkdownToken.cs` — sealed `MarkdownToken(TokenType, Value, Metadata?)`.
- `Languages/Markdown/Metadata/` — `BlockquoteMetadata()`, `ListMetadata(bool IsOrdered)`, `ListItemMetadata(char Marker)`, `OrderedListItemMetadata(int Number, char Marker)` (alleen `.` en `)`), `IndentedCodeBlockMetadata()`, `HeadingMetadata(int Level)`, `TableMetadata`. `ListItemMetadata`/`OrderedListItemMetadata`/`ListMetadata`/`BlockquoteMetadata` zijn `InlineMetadata<MarkdownToken>` → content komt via `RegisterInlineTokenHandler`.
- `Core/InlineMetadata.cs` — `RegisterInlineTokenHandler(handler, onInlinesCompleted = null)`: tweede argument is de commit-hook (wordt aangeroepen door de parser vóórdat het processing-task complete).
- `Core/ICodeBlockMetadata.cs` — `string Language { get; }` (fence-content bypasst de markdown-block-grammar: de taaltokens komen rechtstreeks door de metadata-handler).
- `Languages/Markdown/MarkdownTokenizer.cs` — diep-gestapelde wandeling: blockquote-decision table per diepte; list-flushing; indented-code via aparte content-tokenizer. Token-structuur: `ParagraphBlockEnd → ListStart → (UnorderedListItem|OrderedListItem)* → ListEnd`.
- `ToHtml/Writers/` — architectuur-referentie:
  - `MarkdownBlockTokenDispatcher.cs` — shared dispatch per niveau; `_inParagraph`, `_pendingBlockBreak` (pending separation: schrijven vóór volgend token, weglaten bij stream-einde); `FlushPendingBlockBreak` wordt door containers bij close aangeroepen.
  - `BlockquoteHtmlWriter.cs` — verse dispatcher per quote-laag; commit via `onInlinesCompleted` (flush + sluit).
  - `ListItemHtmlWriter.cs` (regel 33) / `OrderedListItemHtmlWriter.cs` — de block-construct-set voor de `_hasContent`-test: `ParagraphBlockStart, Blockquote, ListStart, CodeBlock, IndentedCodeBlock, Heading, Table, HorizontalRule`; als eerste item-token een block is → content op nieuwe (ingetrakte) regel; `if (_lastTokenType == ListEnd) Write('\n')` voorafgaand aan item-close.

**Spectre 0.55.2 (verifieerd via package-XML)**
- `Paragraph.Append(string, Style?, Link?)` → hyperlinks zijn native in `Paragraph` (relevante voor link-writers in live-regio's).
- `LiveDisplay` heeft `Overflow`, `Cropping`, `AutoClear`-opties → bij verticale overflow (lange showcase op 80x30) de overflow-strategie controleren/instellen tijdens implementatie.
- Custom `IRenderable` (`Measure`/`Render`) en geneste `Live`-regio's (tabel binnen LiveBlock) zijn toegestaan.

## Assumptions and open decisions

- A1. De aangebrachte working-tree-wijzigingen (7.0.0-local.4-reference, gecommentarieerde showcase-methoden) zijn intentieve POC-wijzigingen; het plan bouwt erop voort en schakelt de showcase-methoden pas weer in zodra die werken (stap 15/16).
- A2. Na POC-acceptatie tegen stabiele 7.0.0 (door eigenaar gereld op NuGet) wordt de csproj-reference op `7.0.0` gezet en blijft die er staan; versies van dit project worden niet gedragen.
- A3. Gutter-tint: één subtiele tint voor alle gutter-spaces (suggesie: `Color.DarkGray`), niet per-marker afgeleiden — anders zou een gemengde marker-lijst (`*`/`+`/`-`) met twee marker-colors wisselende tinten geven.
- A4. Code-randkleur: `Color.DodgerBlue1` (of vergelijkbaar cyan-blauw) — duidelijk anders dan de groene quote-rand; exacte kleur mag bij review worden aangepast.
- A5. `BaseInlineWriter` en de oude blok-writers (`MarkdownBlockquoteWriter`, `MarkdownListItemWriter`, `MarkdownOrderedListItemWriter`, `MarkdownHeadingWriter`) worden **verwijderd** nadat de context-pad werkt, mits de taal-writers (`CSharpWriter` e.d.) niet meer op hun `LiveDisplay`-constructie leunen (zie Design notes 3; de taal-writers hebben alleen `GetStyle` nodig). `LeftBoxBorder` blijft (quote-rand).
- A6. De `ShowCase.Markdown`-run voor acceptatie draait op een minimaal 80x40 terminal; `tmp/v7.txt` is een referentie-capture (geen commit-ware artifact, `tmp/` is untracked).
- Besloten (2026-10-06, Colin): (1) `docs/assets/animated-demo.svg` **niet** regenereren — out of scope. (2) `tmp/` **laten liggen** (untracked, niet in de commit) — out of scope.

## Design notes

### 1. LiveBlock = custom `IRenderable` (internal)

`src/.../Writers/LiveBlock.cs`:

- `Kind`: `Quote` | `Code` | `List` | `Bare` (bare = geen decoratie; gebruikt voor root-regio, labels, headings, HR).
- Content-model: `List<LiveRow>`; elke `LiveRow` heeft een `IRenderable Content` (meestal een `Paragraph`) en voor `List`-kind een `string? MarkerPrefix` (null = geen marker, bv. voor de marker-loze structurele regels).
- `List`-kind: gutter = max prefix-breedte **op rendertijd** (groeiend; geen speciale logica). `Render` bouwt per regel `Line(prefixText, content)`:
  - prefix van de marker-regel = de prefix met lengte opgevuld tot de gutter; de marker-char(s) en -getal krijgen hun marker-stijl, de overige spaces krijgen de gutter-tint (A3).
  - prefix van continuatie-regels = `gutter` spaces in de gutter-tint.
  - **Uitlijnings-invariant**: continuatie-regels staan direct onder de content van de eerste regel (gutter = prefix-breedte).
- Prefix-formules (vaste beslissing 4):
  - Ongeordend: `" " + Marker + " "` (marker trouw aan `ListItemMetadata.Marker`).
  - Geordend: `" " + Number.ToString().PadLeft(2) + Separator + " "` (separator trouw aan `OrderedListItemMetadata.Marker`, `.` of `)`).
- `Quote`-kind: links `LeftBoxBorder` in groen (`Panel`-achtig gedrag: border-col + 1 space padding), geen `>`-marker.
- `Code`-kind: links border in de code-randkleur (A4) + 1 space padding.
- Breedte-budget: `Measure` = max(`Measure` van children) + per-laag 2 (border + padding). `Render` reekst children met die maxWidth. Oneindige diepte valt eruit; het budget wordt per niveau uit `maxWidth` berekend (Spectre custom-renderable-patronen, zie Referenties).
- **Re-renderbaar**: contexten muteren de `LiveBlock` (rows toevoegen, `Paragraph`'s growen) en roepen `LiveDisplayContext.Refresh()` aan; `Live` recalculeert. `LiveBlock` zelf mag dus gewoon `Line`/`Text`/`Paragraph`-children hergebruiken.

### 2. Context-stapel (spiegel van ToHtml-architectuur)

Nieuwe classes in `src/.../Writers/`:

- `MarkdownBlockContext` — één per nesting-laag, net als `BlockquoteHtmlWriter` een verse `MarkdownBlockTokenDispatcher` krijgt:
  - Staat: eigen `_inParagraph`, `_pendingBlockBreak` (pending separation: schrijven vóór volgend token, weglaten bij stream-einde), `LiveBlock`-instantie, voor lijsten: per-item-prefixen + `gutter`, voor geordende lijsten: item-sequencing.
  - `Parent`-reference voor commit (pop).
  - `OnComplete` = commit: flush pending separation in eigen regio, regio afsluiten (`Live`-close voor buitenste blok; kind-regio's zijn pure sub-renderables en "sluiten" door uit de parent-groeilijst op te komen).
- `MarkdownBlockDispatcher` (naamgeving gemarkeerd met ToHtml: context/flush/commit, vaste beslissing 6) — shared token-routing per niveau, 1-op-1 overgenomen van `MarkdownBlockTokenDispatcher`:
  - `Text` buiten paragraaf → blokscheiding (vergeten); binnen paragraaf → naar actieve `Paragraph`.
  - `ParagraphBlockStart`/`ParagraphBlockEnd` → paragraaf-state + pending separation.
  - `Heading`, `HorizontalRule`, `Table` → leaf-block-regel (zie 4).
  - `Blockquote`, `ListStart`, `CodeBlock` (fenced), `IndentedCodeBlock` → push nieuwe context.
  - Inlines (`Bold`, `Italic`, `CodeInline`, `Link`, `Image`, `Emoji`, …) → direct naar actieve `Paragraph` (bestaande style-lookup uit `MarkdownWriter.WriteMarkdown`).
  - `ListEnd` → close actieve list-context (ToHtml-regel: vóór het afsluiten een break schrijven als `_lastTokenType == ListEnd`, dus nested list als laatste content).
  - De block-construct-set voor de eerste-token-test van list-items: `ParagraphBlockStart, Blockquote, ListStart, CodeBlock, IndentedCodeBlock, Heading, Table, HorizontalRule` (vaste beslissing: `ListItemHtmlWriter` regel 33).
- Stack-mechaniek: de dispatcher-regel voor een container-token doet `new XxxContext(...)` + push + `metadata.RegisterInlineTokenHandler(token => activeDispatcher.WriteToken(token), onInlinesCompleted: context.Commit)`. De `ListStart`-context zelf is de list-block (gutter/state); `UnorderedListItem`/`OrderedListItem`-tokens binnen een list-context pushen een item-context.

### 3. Consolidatie bestaande Live-constructie

- `BaseInlineWriter`: de `WriteAsync(InlineMetadata)`/`LiveDisplay`-pad wordt **vervangen** door een `AppendToken(Paragraph, TToken)`-stijl API (taaltokens → `Paragraph.Append(value, GetStyle(type))`), zodat codeblok-content in een `LiveBlock`-`Paragraph` kan streamen zonder eigen `Live`. `GetStyle`/taal-writers (`CSharpWriter` e.d.) blijven ongewijzigd in hun switch.
- Verwijderen (niet meer nodig): `MarkdownBlockquoteWriter`, `MarkdownListItemWriter`, `MarkdownOrderedListItemWriter`, `MarkdownHeadingWriter` (heading-logic verhuist naar een `HeadingBlock`-regeling in de context; zie 4). A5.
- `MarkdownWriter` behoudt zijn rol als entry-point en inline-style-lookup, maar de `Paragraph? liveTarget`/`defaultStyle`-plumbing verdwijnt; de entry-point wordt `MarkdownWriter.WriteAsync(token)` → route naar root-context/dispatcher. De 19 `ICodeBlockMetadata`-branches (C, Cpp, CSharp, …) worden gecomprimeerd tot één `CodeBlock`-regeling die per taal de juiste taal-writer (`GetStyle`) kiest — zelfde set, nu in de context-pad (onderhoudingsbaarder; geen taal verloren).
- `MarkdownTableWriter` ongewijzigd (ook in geneste context: Spectre-toegestane sub-`Live`).
- Inlines in live-regio's: `Link`/`Image` krijgen `Paragraph.Append(text, style, new Link(url))` (hyperlink-capability van Spectre 0.55.2, verifieerd); `Emoji`/`FootnoteReference`/`FootnoteDefinition`/`CustomContainer`/`HtmlTag` blijven text-appends in hun stijl (out-of-scope gedrag behouden).

### 4. Render-regels per bloktsoep (vaste beslissingen 1, 5–9)

- **Root**: plain paragrafen plat naar console (bestaand gedrag, direct per token, inclusief `ParagraphBlockStart/End`-scheiding). Eerste niet-plain root-block opent de `Live`-regio met een `Bare` LiveBlock (root-regio) als target.
- **Blockquote** (`Quote`): groene `LeftBoxBorder`, `>` niet gedrukt, padding 1 space.
- **Fenced code** (`Code`): code-randkleur (A4) + taal-label (`{language}:`) als `Bare`-regel in de **zelfde** regio, **boven** de border-box, in de code-stylesheet (`MarkdownStyles.CodeBlock`-stijl voor het label, of een nieuwe `CodeLabel`-property). Geen console-direct-write meer (fix voor bug 9).
- **Indented code**: idem, label `code:`.
- **Ongeordende/ geordende list** (`List`): geen doos; gutter per regel 1; marker-regels en continuatie-regels alignen op de gutter; geordend min. 2 posities.
- **Heading** (`Bare`): bestaand uiterlijk (`** … **` niveau 1, `=`/`-` onderstreping, level-styling) maar binnen de LiveBlock; `StartedAsync`/`FinalizeAsync`-logic vertaalt naar context-hoofd-afsluiting.
- **HR** (`Bare`): `─`-regel, breedte `Console.WindowWidth` (bestaand gedrag), in de regio.
- **Table**: ongewijzigd (`MarkdownTableWriter`).
- **Stream-einde**: pending separations worden weggelaten (ToHtml-whitespace-principle: writer voegt alleen scheiding toe, haalt nooit whitespace weg).

### 5. Live-regio-beheer

- Open: eerste niet-plain root-block; `LiveDisplay(rootLiveBlock)` met `AutoClear = false` (laatste frame blijft in scrollback = commit).
- Refresh: per token `ctx.Refresh()` (bestaand patroon uit `BaseInlineWriter.WriteTokenInLiveTarget`).
- Sluit: bij commit van het buitenste blok. Vervolgblokken op root starten een nieuwe regio.
- Verticale overflow: `LiveDisplay.Overflow`/`Cropping` checken (showcase is hoog); indien nodig `Overflow = OverflowWrap`-achtige instelling of showcase-terminal 80x40.

## Risks and challenges

- R1. **Verticale overflow van Live-regio's** op een 80x30 console: lange showcase + geneste blocks overschrijden de zichtbare hoogte. Mitigatie: `Overflow`/`Cropping` instellen bij LiveDisplay; acceptatie op 80x40 (A6).
- R2. **Performantie**: per-token `Measure`/`Render` van de volledige boom; voor showcase-maat (±KB markdown) geen probleem, maar bij AI-streams met grote codeblocs de oogopmerk houden; refresh-batching is een latente optimisatie, niet in scope.
- R3. **Gutter-groei**: op het moment dat item 100 aankomt verschuift de gehele lijst met 1 kolom (bewust, vaste beslissing 4). Visual check in showcase (lijst `57. foo / 1. bar` geeft al gutter 6).
- R4. **NTokenizers 7.0 is nog niet stabiel gepubliceerd**; lokale `7.0.0-local.x`-varianten verschijnen tussentijds (cache per versie — altijd nieuwe versie-nummer). Eindcheck tegen 7.0.0 kan pas plaatsvinden zodra die gepubliceerd is (eigenaar).
- R5. **v7-metadata-broken**: `LinkMetadata`, `EmojiMetadata`, `FootnoteMetadata` zijn nu plain metadata (geen inline-handler meer) — de bestaande `MarkdownLinkWriter` e.d. gebruiken alleen properties, dus compatibel; maar de oude v6-gedrag van `LinkMetadata.Text`-null-fallback moet handhaven blijven.
- R6. **Token-stream details** (list-flushing, sibling-item, blockquote-decision table) liggen in de NTokenizers-repo (dirty worktree, `7.0-local.4`); gedrag mag daar nog verschuiven — de renderer is per contract (`MarkdownTokenType` + metadata + `RegisterInlineTokenHandler`) robuust, maar acceptatie is per definitie tegen de toenige 7.0.0.
- R7. **Nested tables/footnotes/custom containers** renderen via hun bestaande pad (soms sub-`Live`/console-direct); binnen een live-regio is het uiterlijk niet afdoende getest — expliciet out-of-scope, maar residual risk voor AI-streams die ze bevatten.

## Implementation checklist

- [ ] 1. `LiveBlock`-IRenderable: `Kind` (Quote/Code/List/Bare), `LiveRow` (Content + optional MarkerPrefix), `Measure`/`Render` met breedte-budget (border+padding=2 per laag), List-gutter op rendertijd. → `src/.../Writers/LiveBlock.cs`
- [ ] 2. `MarkdownBlockContext`: state (`_inParagraph`, `_pendingBlockBreak`), `Parent`, `Commit()`, refresh-bridge naar de `LiveDisplayContext`; list-state (prefixen, gutter, item-sequencing, `_hasContent`/`_lastTokenType` voor de ToHtml-invariants). → `src/.../Writers/MarkdownBlockContext.cs`
- [ ] 3. `MarkdownBlockDispatcher`: shared routing per niveau 1-op-1 naar ToHtml (`MarkdownBlockTokenDispatcher.cs`), incl. whitespace-principle, pending separation, flush-bij-close, block-construct-set eerste-token-test. → `src/.../Writers/MarkdownBlockDispatcher.cs`
- [ ] 4. `BaseInlineWriter`: `LiveDisplay`-pad vervangen door `AppendToken(Paragraph, TToken)` (+ `AppendValue(Paragraph, string, Style)` voor label/prefix); taal-writers (`CSharpWriter` e.d.) ongewijzigd in hun `GetStyle`.
- [ ] 5. Blockquote-regel: `Quote`-LiveBlock per `BlockquoteMetadata`, groene `LeftBoxBorder`, `>`-marker niet gedrukt.
- [ ] 6. Fenced-code-regel: `Code`-LiveBlock + taal-label (`Bare`-regel, code-stylesheet, `{language}:` of `code:`) in dezelfde regio; taal-writer-choice per `ICodeBlockMetadata`-type (zelfde taalset als huidige `MarkdownWriter`-branches).
- [ ] 7. Indented-code-regel: idem als 6 met label `code:`, content = plain Text-tokens.
- [ ] 8. List-regels: unordered (`" " + Marker + " "`), ordered (`" " + n.PadLeft(2) + Separator + " "`), gutter-groei, uitlijnings-invariant, item-context per `ListItemMetadata`/`OrderedListItemMetadata`, `ListEnd`-close incl. de ToHtml `_lastTokenType == ListEnd`-break-regel.
- [ ] 9. Heading/HR-regels in de regio: heading-logic uit `MarkdownHeadingWriter` vertalen (level-styling, `**`-decoratie niveau 1, onderstreping-lengte), HR `─` x `Console.WindowWidth`.
- [ ] 10. Table/footnote/emoji/link/image/custom-container: inlines → `Paragraph.Append` (link met `Link`-argument); table via ongewijzigde `MarkdownTableWriter`.
- [ ] 11. `MarkdownWriter` herstructureren: `liveTarget`-plumbing en de 19 codeblock-branches verwijderen; entry-point route naar root-context; inline-style-lookup (`WriteMarkdown`-switch) behouden/overbrengen.
- [ ] 12. `MarkdownStyles`: toevoegen `QuoteBorder`, `CodeBorder`, `CodeLabel`, `Gutter` (subtiele tint, A3/A4) met XML-docs; `LeftBoxBorder` ongewijzigd.
- [ ] 13. Verwijderen (A5): `MarkdownBlockquoteWriter`, `MarkdownListItemWriter`, `MarkdownOrderedListItemWriter`, `MarkdownHeadingWriter` (+ hun styles indien niet langer gebruikt).
- [ ] 14. `ShowCase.Markdown/MarkdownExample.cs`: nested-voorbeeld toevoegen (`> > 1. ~~~csharp …`), bestaande geneste-lijst-voorbeeld behouden, eventueel een 100+-item geordende lijst voor gutter-groei.
- [ ] 15. `ShowCase.Markdown/Program.cs`: de zes usage-patterns weer aanzetten (huidig gecommentarieerd, A1) en de streaming-delay (`Task.Delay` in `SetupStream`) weer activeren voor de live-check.
- [ ] 16. Docs: `docs/markdown.md` (nieuw renderen van geneste blocks, voorbeelden) en `README.md` (korte notitie + screenshot/voorbeeld indien bruikbaar).
- [ ] 17. Uitsluitingscheck: `docs/assets/animated-demo.svg` ongewijzigd laten, `tmp/` niet committen (beide out of scope per Colin).

## Verification checklist

- [ ] `dotnet build NTokenizers.Extensions.Spectre.Console.slnx` slaagt (0 errors).
- [ ] `ShowCase.Markdown` draait op 80x40 terminal; output komt overeen met de ASCII-art (root-level per blok én het nested `> > 1. ~~~csharp`-voorbeeld) in de brainstorm.
- [ ] Bestaand root-level gedrag van paragrafen, headings, HR, tables en inlines ongewijzigd (visueel vergelijken met `tmp/v7.txt`-capture).
- [ ] Taal-labels verschijnen **niet** via console-direct-write (geen `ansiConsole.Write` meer in de codeblock-pad; grep-verify).
- [ ] Nesting van quotes en lijsten verder dan 3 niveaus (showcase-voorbeeld of ad-hoc teststring) blijft correct qua breedte-budget en randen.
- [ ] Gutter-groei: geordende lijst met item ≥ 100 (of `57.`/`1.`-voorbeeld) schuift de gehele lijst correct 1 kolom op.
- [ ] Geneste codeblock in quote en geneste lijst in quote renderen in één Live-regio per buitenste blok (geen geneste `Live` voor quote/list/code; tabel mag sub-`Live` zijn).
- [ ] POC-eindtoestand: volledige showcase-run tegen NTokenizers 7.0.0 (stabil, zodra gepubliceerd); csproj-reference dan op `7.0.0`.
- [ ] Geen nieuwe dependencies (slechts bestaande Spectre.Console + NTokenizers).
- [ ] Self-review diff: geen onbedoelde verwijderingen van publieke API (`AnsiConsoleMarkdownExtensions` blijft signature-stabil); plan-bestand meecommen.

## Handoff notes

- Werkvolgorde: eerst `LiveBlock` + context + dispatcher met plain-quote en plain-list (geen code) werkend krijgen; daarna codeblocs; daarna showcase en docs. De ToHtml-writers zijn de autoritieve referentie voor de invariants — lees `MarkdownBlockTokenDispatcher.cs` en `ListItemHtmlWriter.cs` volledig vóórdat je de dispatcher schrijft.
- De token-stream is al een diep-gestapelde wandeling (container-tokens pushen hun sub-document door de inline-handler; afsluiting = pop). De renderer is een stackmachine die dat spiegelbeeldt; geen buffering van de stream.
- `RegisterInlineTokenHandler(handler, onInlinesCompleted)` — gebruik het **tweede** argument als commit-moment (flush pending separation + sluit regio), net als `BlockquoteHtmlWriter`. De huidige `FinalizeAsync`-constructie van `BaseInlineWriter` vervalt daarmee.
- De `LiveDisplayContext.Refresh()`-call komt per token (bestaand patroon). `Live`-regio: `AutoClear = false` zodat de laatste frame in de scrollback blijft.
- NTokenizers-repo is read-only; de `7.0.0-local.x`-packages zitten in de global cache; als een nieuwe local-versie nodig is, die eerst bouwen in de NTokenizers-repo (niet in deze task).
- Publieke API van `AnsiConsoleMarkdownExtensions` blijft signature-stabil; alleen `MarkdownStyles` krijgt nieuwe (optionele) style-properties.
- Plan-bestand (dit bestand) meecommen met de implementatie.
