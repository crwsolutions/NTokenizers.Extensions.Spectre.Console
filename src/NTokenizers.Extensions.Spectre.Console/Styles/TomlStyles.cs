using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Styles;

/// <summary>
/// Represents the styling configuration for TOML token rendering in Spectre.Console.
/// This class defines the visual styles for various TOML tokens including identifiers, 
/// strings, numbers, booleans, date-times, and structural elements to enable style-rich console syntax highlighting.
/// </summary>
public sealed class TomlStyles
{
    /// <summary>
    /// Gets the default TOML styles configuration.
    /// </summary>
    public static TomlStyles Default => new();

    /// <summary>
    /// Gets or sets the style for whitespace tokens.
    /// </summary>
    public Style Whitespace { get; set; } = Style.Plain;

    /// <summary>
    /// Gets or sets the style for comment tokens.
    /// </summary>
    public Style Comment { get; set; } = new Style(Color.Gray);

    /// <summary>
    /// Gets or sets the style for identifier tokens (keys and table names).
    /// </summary>
    public Style Identifier { get; set; } = new Style(Color.DeepSkyBlue3_1);

    /// <summary>
    /// Gets or sets the style for dot separators in dotted keys.
    /// </summary>
    public Style Dot { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for the equal sign (key-value separator).
    /// </summary>
    public Style Equal { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for comma separators.
    /// </summary>
    public Style Comma { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for open bracket tokens.
    /// </summary>
    public Style OpenBracket { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    /// Gets or sets the style for close bracket tokens.
    /// </summary>
    public Style CloseBracket { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    /// Gets or sets the style for open brace tokens.
    /// </summary>
    public Style OpenBrace { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    /// Gets or sets the style for close brace tokens.
    /// </summary>
    public Style CloseBrace { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    /// Gets or sets the style for string quote tokens.
    /// </summary>
    public Style StringQuote { get; set; } = new Style(Color.White);

    /// <summary>
    /// Gets or sets the style for string value tokens.
    /// </summary>
    public Style StringValue { get; set; } = new Style(Color.White);

    /// <summary>
    /// Gets or sets the style for numeric value tokens.
    /// </summary>
    public Style Number { get; set; } = new Style(Color.DarkSlateGray1);

    /// <summary>
    /// Gets or sets the style for boolean value tokens.
    /// </summary>
    public Style Boolean { get; set; } = new Style(Color.Blue);

    /// <summary>
    /// Gets or sets the style for date-time value tokens.
    /// </summary>
    public Style DateTime { get; set; } = new Style(Color.Magenta);
}
