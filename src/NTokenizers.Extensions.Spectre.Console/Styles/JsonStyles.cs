using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Styles;

/// <summary>
/// Represents the styling configuration for JSON token rendering in Spectre.Console.
/// This class defines the visual styles for various JSON tokens including objects, arrays, 
/// property names, values, and literals to enable style-rich console syntax highlighting.
/// </summary>
public sealed class JsonStyles
{
    /// <summary>
    /// Gets the default JSON styles configuration.
    /// </summary>
    public static JsonStyles Default => new();

    /// <summary>
    /// Gets or sets the style for the start of an object token.
    /// </summary>
    public Style StartObject { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    /// Gets or sets the style for the end of an object token.
    /// </summary>
    public Style EndObject { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    /// Gets or sets the style for the start of an array token.
    /// </summary>
    public Style StartArray { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    /// Gets or sets the style for the end of an array token.
    /// </summary>
    public Style EndArray { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    /// Gets or sets the style for property names in JSON.
    /// </summary>
    public Style PropertyName { get; set; } = new Style(Color.DeepSkyBlue3_1);

    /// <summary>
    /// Gets or sets the style for the colon separator in JSON.
    /// </summary>
    public Style Colon { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for comma separators in JSON.
    /// </summary>
    public Style Comma { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for string values in JSON.
    /// </summary>
    public Style StringValue { get; set; } = new Style(Color.DarkSlateGray1);

    /// <summary>
    /// Gets or sets the style for numeric values in JSON.
    /// </summary>
    public Style Number { get; set; } = new Style(Color.Blue);

    /// <summary>
    /// Gets or sets the style for boolean true values in JSON.
    /// </summary>
    public Style True { get; set; } = new Style(Color.Blue);

    /// <summary>
    /// Gets or sets the style for boolean false values in JSON.
    /// </summary>
    public Style False { get; set; } = new Style(Color.Blue);

    /// <summary>
    /// Gets or sets the style for null values in JSON.
    /// </summary>
    public Style Null { get; set; } = new Style(Color.Blue);
}