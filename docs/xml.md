---
layout: default
title: "Xml"
---

# XML Syntax Highlighting

The `AnsiConsoleXmlExtensions` class provides extension methods for `IAnsiConsole` to render XML content with syntax highlighting.

## Methods

### WriteXmlAsync (Stream, default styles)

Writes XML content from a stream to the console with default styling asynchronously.

```csharp
Task<string> WriteXmlAsync(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The `Stream` containing the XML content.

**Returns:** A task that represents the asynchronous operation. The result is the input stream as a string.

### WriteXmlAsync (Stream, custom styles)

Writes XML content from a stream to the console with custom styling asynchronously.

```csharp
Task<string> WriteXmlAsync(this IAnsiConsole ansiConsole, Stream stream, XmlStyles xmlStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The `Stream` containing the XML content.
- `xmlStyles`: The `XmlStyles` to use for styling the output.

**Returns:** A task that represents the asynchronous operation. The result is the input stream as a string.

### WriteXml (Stream, default styles)

Writes XML content from a stream to the console with default styling synchronously.

```csharp
string WriteXml(this IAnsiConsole ansiConsole, Stream stream)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The `Stream` containing the XML content.

**Returns:** The input stream as a string.

### WriteXml (Stream, custom styles)

Writes XML content from a stream to the console with custom styling synchronously.

```csharp
string WriteXml(this IAnsiConsole ansiConsole, Stream stream, XmlStyles xmlStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `stream`: The `Stream` containing the XML content.
- `xmlStyles`: The `XmlStyles` to use for styling the output.

**Returns:** The input stream as a string.

### WriteXml (string, default styles)

Writes XML content from a string to the console with default styling.

```csharp
void WriteXml(this IAnsiConsole ansiConsole, string value)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The XML content as a string.

### WriteXml (string, custom styles)

Writes XML content from a string to the console with custom styling.

```csharp
void WriteXml(this IAnsiConsole ansiConsole, string value, XmlStyles xmlStyles)
```

**Parameters:**
- `ansiConsole`: The `IAnsiConsole` to write to.
- `value`: The XML content as a string.
- `xmlStyles`: The `XmlStyles` to use for styling the output.

## Example Usage

### Basic Usage with String

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

var xmlContent = """
    <?xml version="1.0"?>
    <glossary>
      <title>example glossary</title>
      <GlossDiv><title>S</title>
      <GlossList>
       <!-- GlossEntry -->
       <GlossEntry ID="SGML" SortAs="SGML">
        <GlossTerm>Standard Generalized Markup Language</GlossTerm>
        <Acronym>SGML</Acronym>
        <Abbrev>ISO 8879:1986</Abbrev>
        <GlossDef>
         <para>A meta-markup language, used to create markup languages such as DocBook.</para>
         <GlossSeeAlso OtherTerm="GML"/>
         <GlossSeeAlso OtherTerm="XML"/>
         <![CDATA[This is CDATA content.]]>
        </GlossDef>
        <GlossSee OtherTerm="markup"/>
       </GlossEntry>
      </GlossList>
     </GlossDiv>
    </glossary>
    """;

AnsiConsole.Console.WriteXml(xmlContent);
```

### Custom Styles

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;
using NTokenizers.Extensions.Spectre.Console.Styles;

var customXmlStyles = XmlStyles.Default;
customXmlStyles.ElementName = new Style(Color.Orange1);

AnsiConsole.Console.WriteXml(xmlContent, customXmlStyles);
```

### Async with Stream

```csharp
using Spectre.Console;
using NTokenizers.Extensions.Spectre.Console;

await AnsiConsole.Console.WriteXmlAsync(stream);
```
