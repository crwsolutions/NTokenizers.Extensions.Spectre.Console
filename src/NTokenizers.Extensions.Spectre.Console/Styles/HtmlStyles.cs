using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Styles;

/// <summary>
/// Represents the styling configuration for HTML tokens in the Spectre.Console rendering extensions.
/// This class provides predefined styles for various HTML elements including tags, attributes, text, comments,
/// and other HTML-specific syntax components to enable style-rich console syntax highlighting.
/// </summary>
public sealed class HtmlStyles
{
    /// <summary>
    /// Gets the default HTML styles configuration.
    /// </summary>
    public static HtmlStyles Default => new();
    
    /// <summary>
    /// Gets or sets the style for HTML element names.
    /// </summary>
    public Style ElementName { get; set; } = new Style(Color.DeepSkyBlue3_1);
    
    /// <summary>
    /// Gets or sets the style for HTML text content.
    /// </summary>
    public Style Text { get; set; } = new Style(Color.DarkSlateGray1);
    
    /// <summary>
    /// Gets or sets the style for HTML comments.
    /// </summary>
    public Style Comment { get; set; } = new Style(Color.Green);
    
    /// <summary>
    /// Gets or sets the style for HTML processing instructions.
    /// </summary>
    public Style ProcessingInstruction { get; set; } = new Style(Color.Turquoise2);
    
    /// <summary>
    /// Gets or sets the style for HTML document type declarations.
    /// </summary>
    public Style DocumentTypeDeclaration { get; set; } = new Style(Color.Turquoise2);
    
    /// <summary>
    /// Gets or sets the style for HTML CDATA sections.
    /// </summary>
    public Style CData { get; set; } = new Style(Color.Magenta1);
    
    /// <summary>
    /// Gets or sets the style for HTML whitespace characters.
    /// </summary>
    public Style Whitespace { get; set; } = new Style(Color.Yellow);
    
    /// <summary>
    /// Gets or sets the style for HTML end element tags.
    /// </summary>
    public Style EndElement { get; set; } = new Style(Color.DeepSkyBlue3_1);
    
    /// <summary>
    /// Gets or sets the style for opening angle brackets (&lt;).
    /// </summary>
    public Style OpeningAngleBracket { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for closing angle brackets (&gt;).
    /// </summary>
    public Style ClosingAngleBracket { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for HTML attribute names.
    /// </summary>
    public Style AttributeName { get; set; } = new Style(Color.Turquoise2);
    
    /// <summary>
    /// Gets or sets the style for equals signs in HTML attributes.
    /// </summary>
    public Style AttributeEquals { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for HTML attribute values.
    /// </summary>
    public Style AttributeValue { get; set; } = new Style(Color.White);
    
    /// <summary>
    /// Gets or sets the style for quotes around HTML attribute values.
    /// </summary>
    public Style AttributeQuote { get; set; } = new Style(Color.DeepSkyBlue3_1);
    
    /// <summary>
    /// Gets or sets the style for self-closing slashes (/) in HTML tags.
    /// </summary>
    public Style SelfClosingSlash { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the default style for HTML content that doesn't match any specific token type.
    /// </summary>
    public Style DefaultStyle { get; set; } = new Style();

    /// <summary>
    /// Gets the TypeScript styles used for rendering TypeScript code in markdown content.
    /// </summary>
    public TypescriptStyles TypescriptStyles { get; } = TypescriptStyles.Default;

    /// <summary>
    /// Gets the CSS styles used for rendering Css code in markdown content.
    /// </summary>
    public CssStyles CssStyles { get; } = CssStyles.Default;
}