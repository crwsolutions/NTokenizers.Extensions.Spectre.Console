using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Styles;

/// <summary>
/// Represents the styling configuration for CSS token rendering in Spectre.Console.
/// This class defines the visual styles for various CSS tokens including selectors, properties, values, and comments
/// to enable style-rich console syntax highlighting.
/// </summary>
public sealed class CssStyles
{
    /// <summary>
    /// Gets the default CSS styles configuration.
    /// </summary>
    public static CssStyles Default => new();

    /// <summary>
    /// Gets or sets the style for CSS rule set start markers ({).
    /// </summary>
    public Style StartRuleSet { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    /// Gets or sets the style for CSS rule set end markers (}).
    /// </summary>
    public Style EndRuleSet { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    /// Gets or sets the style for CSS selectors.
    /// </summary>
    public Style Selector { get; set; } = new Style(Color.DeepSkyBlue3_1);

    /// <summary>
    /// Gets or sets the style for CSS pseudo-elements (::before, ::after).
    /// </summary>
    public Style PseudoElement { get; set; } = new Style(Color.Magenta);

    /// <summary>
    /// Gets or sets the style for CSS property names.
    /// </summary>
    public Style PropertyName { get; set; } = new Style(Color.DeepSkyBlue3_1);

    /// <summary>
    /// Gets or sets the style for CSS string values (quoted strings).
    /// </summary>
    public Style StringValue { get; set; } = new Style(Color.White);

    /// <summary>
    /// Gets or sets the style for CSS string quotes
    /// </summary>
    public Style Quote { get; set; } = new Style(Color.DeepSkyBlue3_1);

    /// <summary>
    /// Gets or sets the style for CSS number values (integer, float).
    /// </summary>
    public Style Number { get; set; } = new Style(Color.DeepSkyBlue3_1);

    /// <summary>
    /// Gets or sets the style for CSS units (px, em, %, rem, pt, etc.).
    /// </summary>
    public Style Unit { get; set; } = new Style(Color.DarkTurquoise);

    /// <summary>
    /// Gets or sets the style for CSS functions (url(), calc(), var(), etc.).
    /// </summary>
    public Style Function { get; set; } = new Style(Color.Cyan1);

    /// <summary>
    /// Gets or sets the style for opening parentheses in CSS functions.
    /// </summary>
    public Style OpenParen { get; set; } = new Style(Color.Cyan);

    /// <summary>
    /// Gets or sets the style for closing parentheses in CSS functions.
    /// </summary>
    public Style CloseParen { get; set; } = new Style(Color.Cyan);

    /// <summary>
    /// Gets or sets the style for CSS comments.
    /// </summary>
    public Style Comment { get; set; } = new Style(Color.Green);

    /// <summary>
    /// Gets or sets the style for CSS colon separators (:).
    /// </summary>
    public Style Colon { get; set; } = new Style(Color.DeepSkyBlue4_2);

    /// <summary>
    /// Gets or sets the style for CSS semicolon separators (;).
    /// </summary>
    public Style Semicolon { get; set; } = new Style(Color.DeepSkyBlue4_2);

    /// <summary>
    /// Gets or sets the style for CSS comma separators (,).
    /// </summary>
    public Style Comma { get; set; } = new Style(Color.DeepSkyBlue4_2);

    /// <summary>
    /// Gets or sets the style for whitespace characters in CSS.
    /// </summary>
    public Style Whitespace { get; set; } = new Style(Color.Default);

    /// <summary>
    /// Gets or sets the style for CSS at-rules (@media, @import, @keyframes, etc.).
    /// </summary>
    public Style AtRule { get; set; } = new Style(Color.Turquoise2);

    /// <summary>
    /// Gets or sets the style for CSS identifiers (class names, IDs, property names).
    /// </summary>
    public Style Identifier { get; set; } = new Style(Color.DarkSlateGray1);

    /// <summary>
    /// Gets or sets the style for CSS operators (+, -, *, /).
    /// </summary>
    public Style Operator { get; set; } = new Style(Color.DeepSkyBlue4_2);

    /// <summary>
    /// Gets or sets the style for CSS dot class selectors (.class).
    /// </summary>
    public Style DotClass { get; set; } = new Style(Color.DeepSkyBlue3_1);

    /// <summary>
    /// Gets or sets the style for attribute selector opening brackets ([).
    /// </summary>
    public Style LeftBracket { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    /// Gets or sets the style for attribute selector closing brackets (]).
    /// </summary>
    public Style RightBracket { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    /// Gets or sets the style for CSS equality operators (=).
    /// </summary>
    public Style EqualsStyle { get; set; } = new Style(Color.DeepSkyBlue4_2);
}
