using NTokenizers.Markdown;
using NTokenizers.Markdown.Metadata;
using NTokenizers.Extensions.Spectre.Console.Styles;
using System.Diagnostics;
using Spectre.Console;


namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal class MarkdownWriter(IAnsiConsole ansiConsole)
{
    internal static MarkdownStyles MarkdownStyles { get; set; } = MarkdownStyles.Default;

    internal static MarkdownWriter Create(IAnsiConsole ansiConsole) => new(ansiConsole);

    internal async Task WriteAsync(MarkdownToken token) => 
        await WriteAsync(null, token, null);

    internal async Task WriteAsync(Paragraph? liveTarget, MarkdownToken token, Style? defaultStyle)
    {
        if (token.Metadata is ICodeBlockMetadata codeBlockMetadata)
        {
            var code = string.IsNullOrWhiteSpace(codeBlockMetadata.Language) ? "code" : codeBlockMetadata.Language;
            ansiConsole.Write(new Text($"{code}:\n"));
        }
        if (token.Metadata is HeadingMetadata meta)
        {
            var writer = new MarkdownHeadingWriter(ansiConsole, MarkdownStyles.MarkdownHeadingStyles);
            await writer.WriteAsync(meta);
        }
        else if (token.Metadata is CSharpCodeBlockMetadata csharpMeta)
        {
            var writer = new CSharpWriter(ansiConsole, MarkdownStyles.CSharpStyles);
            await writer.WriteAsync(csharpMeta);
        }
        else if (token.Metadata is XmlCodeBlockMetadata xmlMeta)
        {
            var writer = new XmlWriter(ansiConsole, MarkdownStyles.XmlStyles);
            await writer.WriteAsync(xmlMeta);
        }
        else if (token.Metadata is TypeScriptCodeBlockMetadata tsMeta)
        {
            var writer = new TypescriptWriter(ansiConsole, MarkdownStyles.TypescriptStyles);
            await writer.WriteAsync(tsMeta);
        }
        else if (token.Metadata is JsonCodeBlockMetadata jsonMeta)
        {
            var writer = new JsonWriter(ansiConsole, MarkdownStyles.JsonStyles);
            await writer.WriteAsync(jsonMeta);
        }
        else if (token.Metadata is YamlCodeBlockMetadata yamlMeta)
        {
            var writer = new YamlWriter(ansiConsole, MarkdownStyles.YamlStyles);
            await writer.WriteAsync(yamlMeta);
        }
        else if (token.Metadata is SqlCodeBlockMetadata sqlMeta)
        {
            var writer = new SqlWriter(ansiConsole, MarkdownStyles.SqlStyles);
            await writer.WriteAsync(sqlMeta);
        }
        else if (token.Metadata is GenericCodeBlockMetadata genericMeta)
        {
            var writer = new GenericWriter(ansiConsole);
            await writer.WriteAsync(genericMeta);
        }
        else if (token.Metadata is LinkMetadata linkMeta)
        {
            var writer = new MarkdownLinkWriter(ansiConsole);
            writer.Write(linkMeta);
        }
        else if (token.Metadata is BlockquoteMetadata blockquoteMeta)
        {
            var writer = new MarkdownBlockquoteWriter(ansiConsole);
            await writer.WriteAsync(blockquoteMeta);
        }
        else if (token.Metadata is FootnoteMetadata footnoteMeta)
        {
            var writer = new MarkdownFootnoteWriter(ansiConsole);
            writer.Write(footnoteMeta);
        }
        else if (token.Metadata is EmojiMetadata emojiMeta)
        {
            var writer = new MarkdownEmojiWriter(ansiConsole);
            writer.Write(emojiMeta);
        }
        else if (token.Metadata is OrderedListItemMetadata orderedListItemMeta)
        {
            var writer = new MarkdownOrderedListItemWriter(ansiConsole, MarkdownStyles.MarkdownOrderedListItemStyles);
            await writer.WriteAsync(orderedListItemMeta);
        }
        else if (token.Metadata is ListItemMetadata listItemMeta)
        {
            var writer = new MarkdownListItemWriter(ansiConsole, MarkdownStyles.MarkdownListItemStyles);
            await writer.WriteAsync(listItemMeta);
        }
        else if (token.Metadata is TableMetadata tableMeta)
        {
            var writer = new MarkdownTableWriter(ansiConsole, MarkdownStyles);
            await writer.WriteAsync(tableMeta);
        }
        else
        {
            WriteMarkdown(liveTarget, token, defaultStyle);
        }
    }

    private void Write(Paragraph? liveTarget, string value, Style? style = null)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }
        Debug.WriteLine($"Writing token: `{value}` with style `[{style?.Foreground}/{style?.Background}]`");

        var text = Markup.Escape(value);
        if (liveTarget is not null)
        {
            liveTarget.Append(text, style);
        }
        else if (style is not null)
        {
            ansiConsole.Write(new Markup(text, style));
        }
        else
        {
            ansiConsole.Write(new Text(text));
        }
    }

    internal void WriteMarkdown(Paragraph? liveTarget, MarkdownToken token, Style? defaultStyle)
    {
        var style = token.TokenType switch
        {
            MarkdownTokenType.Heading => MarkdownStyles.Heading,
            MarkdownTokenType.Bold => MarkdownStyles.Bold,
            MarkdownTokenType.Italic => MarkdownStyles.Italic,
            MarkdownTokenType.HorizontalRule => MarkdownStyles.HorizontalRule,
            MarkdownTokenType.CodeInline => MarkdownStyles.CodeInline,
            MarkdownTokenType.CodeBlock => MarkdownStyles.CodeBlock,
            MarkdownTokenType.Link => MarkdownStyles.Link,
            MarkdownTokenType.Image => MarkdownStyles.Image,
            MarkdownTokenType.Blockquote => MarkdownStyles.Blockquote,
            MarkdownTokenType.UnorderedListItem => MarkdownStyles.UnorderedListItem,
            MarkdownTokenType.OrderedListItem => MarkdownStyles.OrderedListItem,
            MarkdownTokenType.TableCell => MarkdownStyles.TableCell,
            MarkdownTokenType.Emphasis => MarkdownStyles.Emphasis,
            MarkdownTokenType.TypographicReplacement => MarkdownStyles.TypographicReplacement,
            MarkdownTokenType.FootnoteReference => MarkdownStyles.FootnoteReference,
            MarkdownTokenType.FootnoteDefinition => MarkdownStyles.FootnoteDefinition,
            MarkdownTokenType.DefinitionTerm => MarkdownStyles.DefinitionTerm,
            MarkdownTokenType.DefinitionDescription => MarkdownStyles.DefinitionDescription,
            MarkdownTokenType.Abbreviation => MarkdownStyles.Abbreviation,
            MarkdownTokenType.CustomContainer => MarkdownStyles.CustomContainer,
            MarkdownTokenType.HtmlTag => MarkdownStyles.HtmlTag,
            MarkdownTokenType.Subscript => MarkdownStyles.Subscript,
            MarkdownTokenType.Superscript => MarkdownStyles.Superscript,
            MarkdownTokenType.InsertedText => MarkdownStyles.InsertedText,
            MarkdownTokenType.MarkedText => MarkdownStyles.MarkedText,
            MarkdownTokenType.Emoji => MarkdownStyles.Emoji,
            _ => defaultStyle ?? MarkdownStyles.DefaultStyle
        };

        if (token.TokenType == MarkdownTokenType.HorizontalRule)
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

