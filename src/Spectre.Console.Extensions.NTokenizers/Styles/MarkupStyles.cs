namespace Spectre.Console.Extensions.NTokenizers.Styles;

public class MarkupStyles
{
    public static MarkupStyles Default => new();

    public Style Heading { get; set; } = new Style(Color.Yellow);
    public Style Bold { get; set; } = new Style(Color.Blue, decoration: Decoration.Bold);
    public Style Italic { get; set; } = new Style(Color.Green, decoration: Decoration.Italic);
    public Style HorizontalRule { get; set; } = new Style(Color.Grey);
    public Style CodeInline { get; set; } = new Style(Color.Cyan);
    public Style CodeBlock { get; set; } = new Style(Color.Magenta);
    public Style Link { get; set; } = new Style(Color.Blue, decoration: Decoration.Underline);
    public Style Image { get; set; } = new Style(Color.Purple);
    public Style Blockquote { get; set; } = new Style(Color.Orange1);
    public Style UnorderedListItem { get; set; } = new Style(Color.Red);
    public Style OrderedListItem { get; set; } = new Style(Color.Red);
    public Style TableCell { get; set; } = new Style(Color.Lime);
    public Style Emphasis { get; set; } = new Style(Color.Yellow, decoration: Decoration.Italic);
    public Style TypographicReplacement { get; set; } = new Style(Color.Grey);
    public Style FootnoteReference { get; set; } = new Style(Color.Pink1);
    public Style FootnoteDefinition { get; set; } = new Style(Color.Pink1);
    public Style DefinitionTerm { get; set; } = new Style(decoration: Decoration.Bold);
    public Style DefinitionDescription { get; set; } = new Style(decoration: Decoration.Italic);
    public Style Abbreviation { get; set; } = new Style(decoration: Decoration.Underline);
    public Style CustomContainer { get; set; } = new Style(Color.Teal);
    public Style HtmlTag { get; set; } = new Style(Color.Orange1);
    public Style Subscript { get; set; } = new Style(Color.Grey);
    public Style Superscript { get; set; } = new Style(Color.White);
    public Style InsertedText { get; set; } = new Style(Color.Green);
    public Style MarkedText { get; set; } = new Style(Color.Yellow);
    public Style Emoji { get; set; } = new Style(Color.Yellow);
    public Style DefaultStyle { get; set; } = new Style();

    public CSharpStyles CSharpStyles { get; } = CSharpStyles.Default;
    public JsonStyles JsonStyles { get; } = JsonStyles.Default;
    public XmlStyles XmlStyles { get; } = XmlStyles.Default;
    public TypescriptStyles TypescriptStyles { get; } = TypescriptStyles.Default;
    public SqlStyles SqlStyles { get; } = SqlStyles.Default;
    public MarkupHeadingStyles MarkupHeadingStyles { get; } = MarkupHeadingStyles.Default;
    public MarkupListItemStyles MarkupListItemStyles { get; } = MarkupListItemStyles.Default;
    public MarkupOrderedListItemStyles MarkupOrderedListItemStyles { get; } = MarkupOrderedListItemStyles.Default;
}