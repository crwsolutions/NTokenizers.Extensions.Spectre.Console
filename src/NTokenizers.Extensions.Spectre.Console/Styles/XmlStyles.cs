using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Styles;

/// <summary>
/// Represents the styling configuration for XML tokens in the Spectre.Console rendering extensions.
/// This class provides predefined styles for various XML elements including tags, attributes, text, comments,
/// and other XML-specific syntax components to enable style-rich console syntax highlighting.
/// </summary>
public sealed class XmlStyles
{
    /// <summary>
    /// Gets the default XML styles configuration.
    /// </summary>
    public static XmlStyles Default => new XmlStyles();
    
    /// <summary>
    /// Gets or sets the style for XML element names.
    /// </summary>
    public Style ElementName { get; set; } = new Style(Color.DeepSkyBlue3_1);
    
    /// <summary>
    /// Gets or sets the style for XML text content.
    /// </summary>
    public Style Text { get; set; } = new Style(Color.DarkSlateGray1);
    
    /// <summary>
    /// Gets or sets the style for XML comments.
    /// </summary>
    public Style Comment { get; set; } = new Style(Color.Green);
    
    /// <summary>
    /// Gets or sets the style for XML processing instructions.
    /// </summary>
    public Style ProcessingInstruction { get; set; } = new Style(Color.Turquoise2);
    
    /// <summary>
    /// Gets or sets the style for XML document type declarations.
    /// </summary>
    public Style DocumentTypeDeclaration { get; set; } = new Style(Color.Turquoise2);
    
    /// <summary>
    /// Gets or sets the style for XML CDATA sections.
    /// </summary>
    public Style CData { get; set; } = new Style(Color.Magenta1);
    
    /// <summary>
    /// Gets or sets the style for XML whitespace characters.
    /// </summary>
    public Style Whitespace { get; set; } = new Style(Color.Yellow);
    
    /// <summary>
    /// Gets or sets the style for XML end element tags.
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
    /// Gets or sets the style for XML attribute names.
    /// </summary>
    public Style AttributeName { get; set; } = new Style(Color.Turquoise2);
    
    /// <summary>
    /// Gets or sets the style for equals signs in XML attributes.
    /// </summary>
    public Style AttributeEquals { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for XML attribute values.
    /// </summary>
    public Style AttributeValue { get; set; } = new Style(Color.White);
    
    /// <summary>
    /// Gets or sets the style for quotes around XML attribute values.
    /// </summary>
    public Style AttributeQuote { get; set; } = new Style(Color.DeepSkyBlue3_1);
    
    /// <summary>
    /// Gets or sets the style for self-closing slashes (/) in XML tags.
    /// </summary>
    public Style SelfClosingSlash { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the default style for XML content that doesn't match any specific token type.
    /// </summary>
    public Style DefaultStyle { get; set; } = new Style();
}