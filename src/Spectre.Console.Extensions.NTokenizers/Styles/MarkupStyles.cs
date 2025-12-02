using System;

namespace Spectre.Console.Extensions.NTokenizers.Styles;

/// <summary>
/// Represents the collection of styles used for rendering markup tokens in the console.
/// This class provides predefined styles for various markup elements such as headings, 
/// code blocks, links, and more to enable rich-text console output.
/// </summary>
public class MarkupStyles
{
    /// <summary>
    /// Gets the default instance of <see cref="MarkupStyles"/>.
    /// </summary>
    public static MarkupStyles Default => new();

    /// <summary>
    /// Gets or sets the style for headings in markup content.
    /// </summary>
    public Style Heading { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for bold text in markup content.
    /// </summary>
    public Style Bold { get; set; } = new Style(Color.Blue, decoration: Decoration.Bold);

    /// <summary>
    /// Gets or sets the style for italic text in markup content.
    /// </summary>
    public Style Italic { get; set; } = new Style(Color.Green, decoration: Decoration.Italic);

    /// <summary>
    /// Gets or sets the style for horizontal rules in markup content.
    /// </summary>
    public Style HorizontalRule { get; set; } = new Style(Color.Grey);

    /// <summary>
    /// Gets or sets the style for inline code in markup content.
    /// </summary>
    public Style CodeInline { get; set; } = new Style(Color.Cyan);

    /// <summary>
    /// Gets or sets the style for code blocks in markup content.
    /// </summary>
    public Style CodeBlock { get; set; } = new Style(Color.Magenta);

    /// <summary>
    /// Gets or sets the style for links in markup content.
    /// </summary>
    public Style Link { get; set; } = new Style(Color.Blue, decoration: Decoration.Underline);

    /// <summary>
    /// Gets or sets the style for images in markup content.
    /// </summary>
    public Style Image { get; set; } = new Style(Color.Purple);

    /// <summary>
    /// Gets or sets the style for blockquotes in markup content.
    /// </summary>
    public Style Blockquote { get; set; } = new Style(Color.Orange1);

    /// <summary>
    /// Gets or sets the style for unordered list items in markup content.
    /// </summary>
    public Style UnorderedListItem { get; set; } = new Style(Color.Red);

    /// <summary>
    /// Gets or sets the style for ordered list items in markup content.
    /// </summary>
    public Style OrderedListItem { get; set; } = new Style(Color.Red);

    /// <summary>
    /// Gets or sets the style for table cells in markup content.
    /// </summary>
    public Style TableCell { get; set; } = new Style(Color.Lime);

    /// <summary>
    /// Gets or sets the style for emphasized text in markup content.
    /// </summary>
    public Style Emphasis { get; set; } = new Style(Color.Yellow, decoration: Decoration.Italic);

    /// <summary>
    /// Gets or sets the style for typographic replacements in markup content.
    /// </summary>
    public Style TypographicReplacement { get; set; } = new Style(Color.Grey);

    /// <summary>
    /// Gets or sets the style for footnote references in markup content.
    /// </summary>
    public Style FootnoteReference { get; set; } = new Style(Color.Pink1);

    /// <summary>
    /// Gets or sets the style for footnote definitions in markup content.
    /// </summary>
    public Style FootnoteDefinition { get; set; } = new Style(Color.Pink1);

    /// <summary>
    /// Gets or sets the style for definition terms in markup content.
    /// </summary>
    public Style DefinitionTerm { get; set; } = new Style(decoration: Decoration.Bold);

    /// <summary>
    /// Gets or sets the style for definition descriptions in markup content.
    /// </summary>
    public Style DefinitionDescription { get; set; } = new Style(decoration: Decoration.Italic);

    /// <summary>
    /// Gets or sets the style for abbreviations in markup content.
    /// </summary>
    public Style Abbreviation { get; set; } = new Style(decoration: Decoration.Underline);

    /// <summary>
    /// Gets or sets the style for custom containers in markup content.
    /// </summary>
    public Style CustomContainer { get; set; } = new Style(Color.Teal);

    /// <summary>
    /// Gets or sets the style for HTML tags in markup content.
    /// </summary>
    public Style HtmlTag { get; set; } = new Style(Color.Orange1);

    /// <summary>
    /// Gets or sets the style for subscript text in markup content.
    /// </summary>
    public Style Subscript { get; set; } = new Style(Color.Grey);

    /// <summary>
    /// Gets or sets the style for superscript text in markup content.
    /// </summary>
    public Style Superscript { get; set; } = new Style(Color.White);

    /// <summary>
    /// Gets or sets the style for inserted text in markup content.
    /// </summary>
    public Style InsertedText { get; set; } = new Style(Color.Green);

    /// <summary>
    /// Gets or sets the style for marked text in markup content.
    /// </summary>
    public Style MarkedText { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for emojis in markup content.
    /// </summary>
    public Style Emoji { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the default style applied to all markup content when no specific style is defined.
    /// </summary>
    public Style DefaultStyle { get; set; } = new Style();

    /// <summary>
    /// Gets the C# styles used for rendering C# code in markup content.
    /// </summary>
    public CSharpStyles CSharpStyles { get; } = CSharpStyles.Default;

    /// <summary>
    /// Gets the JSON styles used for rendering JSON content in markup content.
    /// </summary>
    public JsonStyles JsonStyles { get; } = JsonStyles.Default;

    /// <summary>
    /// Gets the XML styles used for rendering XML content in markup content.
    /// </summary>
    public XmlStyles XmlStyles { get; } = XmlStyles.Default;

    /// <summary>
    /// Gets the TypeScript styles used for rendering TypeScript code in markup content.
    /// </summary>
    public TypescriptStyles TypescriptStyles { get; } = TypescriptStyles.Default;

    /// <summary>
    /// Gets the SQL styles used for rendering SQL queries in markup content.
    /// </summary>
    public SqlStyles SqlStyles { get; } = SqlStyles.Default;

    /// <summary>
    /// Gets the heading styles used for rendering markup headings.
    /// </summary>
    public MarkupHeadingStyles MarkupHeadingStyles { get; } = MarkupHeadingStyles.Default;

    /// <summary>
    /// Gets the list item styles used for rendering unordered list items in markup content.
    /// </summary>
    public MarkupListItemStyles MarkupListItemStyles { get; } = MarkupListItemStyles.Default;

    /// <summary>
    /// Gets the ordered list item styles used for rendering ordered list items in markup content.
    /// </summary>
    public MarkupOrderedListItemStyles MarkupOrderedListItemStyles { get; } = MarkupOrderedListItemStyles.Default;
}