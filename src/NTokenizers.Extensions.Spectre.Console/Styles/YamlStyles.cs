using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Styles;

/// <summary>
/// Represents the styling configuration for YAML token rendering in Spectre.Console.
/// This class defines the visual styles for various YAML tokens to enable style-rich console syntax highlighting.
/// </summary>
public sealed class YamlStyles
{
    /// <summary>
    /// Gets the default YAML styles configuration.
    /// </summary>
    public static YamlStyles Default => new();

    /// <summary>
    /// Gets or sets the style for directive tokens.
    /// </summary>
    public Style Directive { get; set; } = new Style(Color.Turquoise2);

    /// <summary>
    /// Gets or sets the style for directive keys (YAML or TAG).
    /// </summary>
    public Style DirectiveKey { get; set; } = new Style(Color.Turquoise2);

    /// <summary>
    /// Gets or sets the style for directive values.
    /// </summary>
    public Style DirectiveValue { get; set; } = new Style(Color.White);

    /// <summary>
    /// Gets or sets the style for document start markers (---).
    /// </summary>
    public Style DocumentStart { get; set; } = new Style(Color.Green);

    /// <summary>
    /// Gets or sets the style for document end markers (...).
    /// </summary>
    public Style DocumentEnd { get; set; } = new Style(Color.Green);

    /// <summary>
    /// Gets or sets the style for comments (# ...).
    /// </summary>
    public Style Comment { get; set; } = new Style(Color.Green);

    /// <summary>
    /// Gets or sets the style for keys before a colon.
    /// </summary>
    public Style Key { get; set; } = new Style(Color.DeepSkyBlue3_1);

    /// <summary>
    /// Gets or sets the style for colon separators.
    /// </summary>
    public Style Colon { get; set; } = new Style(Color.DeepSkyBlue3_1);

    /// <summary>
    /// Gets or sets the style for plain values or quoted string values.
    /// </summary>
    public Style Value { get; set; } = new Style(Color.DarkSlateGray1);

    /// <summary>
    /// Gets or sets the style for quote characters (").
    /// </summary>
    public Style Quote { get; set; } = new Style(Color.DeepSkyBlue4_2);

    /// <summary>
    /// Gets or sets the style for content between quotes.
    /// </summary>
    public Style String { get; set; } = new Style(Color.White);

    /// <summary>
    /// Gets or sets the style for anchors.
    /// </summary>
    public Style Anchor { get; set; } = new Style(Color.Magenta);

    /// <summary>
    /// Gets or sets the style for aliases (*alias).
    /// </summary>
    public Style Alias { get; set; } = new Style(Color.Magenta);

    /// <summary>
    /// Gets or sets the style for tags (!tag or !!type).
    /// </summary>
    public Style Tag { get; set; } = new Style(Color.Magenta);

    /// <summary>
    /// Gets or sets the style for flow sequence start markers ([).
    /// </summary>
    public Style FlowSeqStart { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    /// Gets or sets the style for flow sequence end markers (]).
    /// </summary>
    public Style FlowSeqEnd { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    /// Gets or sets the style for flow mapping start markers ({).
    /// </summary>
    public Style FlowMapStart { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    /// Gets or sets the style for flow mapping end markers (}).
    /// </summary>
    public Style FlowMapEnd { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    /// Gets or sets the style for comma separators in flow collections.
    /// </summary>
    public Style FlowEntry { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for block sequence entry markers (-).
    /// </summary>
    public Style BlockSeqEntry { get; set; } = new Style(Color.White);

    /// <summary>
    /// Gets or sets the style for whitespace characters.
    /// </summary>
    public Style Whitespace { get; set; } = new Style(Color.White);
}