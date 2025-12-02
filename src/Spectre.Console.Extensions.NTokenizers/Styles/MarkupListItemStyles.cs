namespace Spectre.Console.Extensions.NTokenizers.Styles;
/// <summary>
/// Represents the styling options for markup list items in the NTokenizers console extensions.
/// This class defines the visual appearance of list markers used when rendering markup content
/// with syntax highlighting support for various token types including XML, JSON, TypeScript, C#, and SQL.
/// </summary>
public class MarkupListItemStyles
{
    /// <summary>
    /// Gets the default styling configuration for markup list items.
    /// </summary>
    public static MarkupListItemStyles Default => new();

    /// <summary>
    /// Gets or sets the style applied to the marker character of markup list items.
    /// This style controls the visual appearance of the list marker (e.g., bullet points)
    /// when rendering markup content with syntax highlighting.
    /// </summary>
    public Style Marker { get; set; } = new Style(Color.Turquoise2);
}
