using NTokenizers.Markup;
using NTokenizers.Markup.Metadata;
using NTokenizers.Extensions.Spectre.Console.Styles;
using System.Diagnostics;
using SpectreLib = Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal class MarkupWriter(SpectreLib.IAnsiConsole ansiConsole)
{
    internal static MarkupStyles MarkupStyles { get; set; } = MarkupStyles.Default;

    internal static MarkupWriter Create(SpectreLib.IAnsiConsole ansiConsole) => new(ansiConsole);

    internal async Task WriteAsync(MarkupToken token) => 
        await WriteAsync(null, token, null);

    internal async Task WriteAsync(SpectreLib.Paragraph? liveTarget, MarkupToken token, SpectreLib.Style? defaultStyle)
    {
        if (token.Metadata is ICodeBlockMetadata codeBlockMetadata)
        {
            var code = string.IsNullOrWhiteSpace(codeBlockMetadata.Language) ? "code" : codeBlockMetadata.Language;
            ansiConsole.Write(new SpectreLib.Text($"{code}:\n"));
        }
        if (token.Metadata is HeadingMetadata meta)
        {
            var writer = new MarkupHeadingWriter(ansiConsole, MarkupStyles.MarkupHeadingStyles);
            await writer.WriteAsync(meta);
        }
        else if (token.Metadata is CSharpCodeBlockMetadata csharpMeta)
        {
            var writer = new CSharpWriter(ansiConsole, MarkupStyles.CSharpStyles);
            await writer.WriteAsync(csharpMeta);
        }
        else if (token.Metadata is XmlCodeBlockMetadata xmlMeta)
        {
            var writer = new XmlWriter(ansiConsole, MarkupStyles.XmlStyles);
            await writer.WriteAsync(xmlMeta);
        }
        else if (token.Metadata is TypeScriptCodeBlockMetadata tsMeta)
        {
            var writer = new TypescriptWriter(ansiConsole, MarkupStyles.TypescriptStyles);
            await writer.WriteAsync(tsMeta);
        }
        else if (token.Metadata is JsonCodeBlockMetadata jsonMeta)
        {
            var writer = new JsonWriter(ansiConsole, MarkupStyles.JsonStyles);
            await writer.WriteAsync(jsonMeta);
        }
        else if (token.Metadata is SqlCodeBlockMetadata sqlMeta)
        {
            var writer = new SqlWriter(ansiConsole, MarkupStyles.SqlStyles);
            await writer.WriteAsync(sqlMeta);
        }
        else if (token.Metadata is GenericCodeBlockMetadata genericMeta)
        {
            var writer = new GenericWriter(ansiConsole);
            await writer.WriteAsync(genericMeta);
        }
        else if (token.Metadata is LinkMetadata linkMeta)
        {
            var writer = new MarkupLinkWriter(ansiConsole);
            writer.Write(linkMeta);
        }
        else if (token.Metadata is BlockquoteMetadata blockquoteMeta)
        {
            var writer = new MarkupBlockquoteWriter(ansiConsole);
            await writer.WriteAsync(blockquoteMeta);
        }
        else if (token.Metadata is FootnoteMetadata footnoteMeta)
        {
            var writer = new MarkupFootnoteWriter(ansiConsole);
            writer.Write(footnoteMeta);
        }
        else if (token.Metadata is EmojiMetadata emojiMeta)
        {
            var writer = new MarkupEmojiWriter(ansiConsole);
            writer.Write(emojiMeta);
        }
        else if (token.Metadata is OrderedListItemMetadata orderedListItemMeta)
        {
            var writer = new MarkupOrderedListItemWriter(ansiConsole, MarkupStyles.MarkupOrderedListItemStyles);
            await writer.WriteAsync(orderedListItemMeta);
        }
        else if (token.Metadata is ListItemMetadata listItemMeta)
        {
            var writer = new MarkupListItemWriter(ansiConsole, MarkupStyles.MarkupListItemStyles);
            await writer.WriteAsync(listItemMeta);
        }
        else if (token.Metadata is TableMetadata tableMeta)
        {
            var writer = new MarkupTableWriter(ansiConsole, MarkupStyles);
            await writer.WriteAsync(tableMeta);
        }
        else
        {
            WriteMarkup(liveTarget, token, defaultStyle);
        }
    }

    private void Write(SpectreLib.Paragraph? liveTarget, string value, SpectreLib.Style? style = null)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }
        Debug.WriteLine($"Writing token: `{value}` with style `[{style?.Foreground}/{style?.Background}]`");

        var text = SpectreLib.Markup.Escape(value);
        if (liveTarget is not null)
        {
            liveTarget.Append(text, style);
        }
        else if (style is not null)
        {
            ansiConsole.Write(new SpectreLib.Markup(text, style));
        }
        else
        {
            ansiConsole.Write(new SpectreLib.Text(text));
        }
    }

    internal void WriteMarkup(SpectreLib.Paragraph? liveTarget, MarkupToken token, SpectreLib.Style? defaultStyle)
    {
        var style = token.TokenType switch
        {
            MarkupTokenType.Heading => MarkupStyles.Heading,
            MarkupTokenType.Bold => MarkupStyles.Bold,
            MarkupTokenType.Italic => MarkupStyles.Italic,
            MarkupTokenType.HorizontalRule => MarkupStyles.HorizontalRule,
            MarkupTokenType.CodeInline => MarkupStyles.CodeInline,
            MarkupTokenType.CodeBlock => MarkupStyles.CodeBlock,
            MarkupTokenType.Link => MarkupStyles.Link,
            MarkupTokenType.Image => MarkupStyles.Image,
            MarkupTokenType.Blockquote => MarkupStyles.Blockquote,
            MarkupTokenType.UnorderedListItem => MarkupStyles.UnorderedListItem,
            MarkupTokenType.OrderedListItem => MarkupStyles.OrderedListItem,
            MarkupTokenType.TableCell => MarkupStyles.TableCell,
            MarkupTokenType.Emphasis => MarkupStyles.Emphasis,
            MarkupTokenType.TypographicReplacement => MarkupStyles.TypographicReplacement,
            MarkupTokenType.FootnoteReference => MarkupStyles.FootnoteReference,
            MarkupTokenType.FootnoteDefinition => MarkupStyles.FootnoteDefinition,
            MarkupTokenType.DefinitionTerm => MarkupStyles.DefinitionTerm,
            MarkupTokenType.DefinitionDescription => MarkupStyles.DefinitionDescription,
            MarkupTokenType.Abbreviation => MarkupStyles.Abbreviation,
            MarkupTokenType.CustomContainer => MarkupStyles.CustomContainer,
            MarkupTokenType.HtmlTag => MarkupStyles.HtmlTag,
            MarkupTokenType.Subscript => MarkupStyles.Subscript,
            MarkupTokenType.Superscript => MarkupStyles.Superscript,
            MarkupTokenType.InsertedText => MarkupStyles.InsertedText,
            MarkupTokenType.MarkedText => MarkupStyles.MarkedText,
            MarkupTokenType.Emoji => MarkupStyles.Emoji,
            _ => defaultStyle ?? MarkupStyles.DefaultStyle
        };

        if (token.TokenType == MarkupTokenType.HorizontalRule)
        {
            var value = new string('─', System.Console.WindowWidth);
            Write(liveTarget, value, style);
        }
        else
        {
            Write(liveTarget, token.Value, style);
        }
    }
}

