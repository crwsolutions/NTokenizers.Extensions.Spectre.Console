using System.ComponentModel;

namespace Spectre.Console.Extensions.NTokenizers.Styles;

/// <summary>
///     Provides predefined styles for TypeScript token highlighting in Spectre.Console.
///     This class defines color-coded styles for various TypeScript language elements
///     to enable style-rich console syntax highlighting for TypeScript content.
/// </summary>
public sealed class TypescriptStyles
{
    /// <summary>
    ///     Gets the default TypeScript styles.
    /// </summary>
    public static TypescriptStyles Default => new();

    /// <summary>
    ///     Gets or sets the style for open parentheses.
    /// </summary>
    public Style OpenParenthesis { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    ///     Gets or sets the style for close parentheses.
    /// </summary>
    public Style CloseParenthesis { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    ///     Gets or sets the style for commas.
    /// </summary>
    public Style Comma { get; set; } = new Style(Color.Yellow);

    /// <summary>
    ///     Gets or sets the style for string values.
    /// </summary>
    public Style StringValue { get; set; } = new Style(Color.DarkSlateGray1);

    /// <summary>
    ///     Gets or sets the style for numeric values.
    /// </summary>
    public Style Number { get; set; } = new Style(Color.Blue);

    /// <summary>
    ///     Gets or sets the style for keywords.
    /// </summary>
    public Style Keyword { get; set; } = new Style(Color.Turquoise2);

    /// <summary>
    ///     Gets or sets the style for identifiers.
    /// </summary>
    public Style Identifier { get; set; } = new Style(Color.White);

    /// <summary>
    ///     Gets or sets the style for comments.
    /// </summary>
    public Style Comment { get; set; } = new Style(Color.Green);

    /// <summary>
    ///     Gets or sets the style for operators.
    /// </summary>
    public Style Operator { get; set; } = new Style(Color.DeepSkyBlue4_2);

    /// <summary>
    ///     Gets or sets the style for the 'and' operator.
    /// </summary>
    public Style And { get; set; } = new Style(Color.DeepSkyBlue4_2);

    /// <summary>
    ///     Gets or sets the style for the 'or' operator.
    /// </summary>
    public Style Or { get; set; } = new Style(Color.DeepSkyBlue4_2);

    /// <summary>
    ///     Gets or sets the style for equals expressions.
    /// </summary>
    public Style EqualsStyle { get; set; } = new Style(Color.DeepSkyBlue4_2);

    /// <summary>
    ///     Gets or sets the style for not-equals expressions.
    /// </summary>
    public Style NotEquals { get; set; } = new Style(Color.DeepSkyBlue4_2);

    /// <summary>
    ///     Gets or sets the style for the 'in' keyword.
    /// </summary>
    public Style In { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    ///     Gets or sets the style for the 'not in' keyword.
    /// </summary>
    public Style NotIn { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    ///     Gets or sets the style for the 'like' keyword.
    /// </summary>
    public Style Like { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    ///     Gets or sets the style for the 'not like' keyword.
    /// </summary>
    public Style NotLike { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    ///     Gets or sets the style for the 'limit' keyword.
    /// </summary>
    public Style Limit { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    ///     Gets or sets the style for the 'match' keyword.
    /// </summary>
    public Style Match { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    ///     Gets or sets the style for sequence terminators.
    /// </summary>
    public Style SequenceTerminator { get; set; } = new Style(Color.Yellow);

    /// <summary>
    ///     Gets or sets the style for dots.
    /// </summary>
    public Style Dot { get; set; } = new Style(Color.Yellow);

    /// <summary>
    ///     Gets or sets the style for whitespace.
    /// </summary>
    public Style Whitespace { get; set; } = new Style(Color.Yellow);

    /// <summary>
    ///     Gets or sets the style for date/time values.
    /// </summary>
    public Style DateTimeValue { get; set; } = new Style(Color.Blue);

    /// <summary>
    ///     Gets or sets the style for fingerprints.
    /// </summary>
    public Style Fingerprint { get; set; } = new Style(Color.DeepSkyBlue3_1);

    /// <summary>
    ///     Gets or sets the style for messages.
    /// </summary>
    public Style Message { get; set; } = new Style(Color.DeepSkyBlue3_1);

    /// <summary>
    ///     Gets or sets the style for stack frames.
    /// </summary>
    public Style StackFrame { get; set; } = new Style(Color.DeepSkyBlue3_1);

    /// <summary>
    ///     Gets or sets the style for exception types.
    /// </summary>
    public Style ExceptionType { get; set; } = new Style(Color.DeepSkyBlue3_1);

    /// <summary>
    ///     Gets or sets the style for applications.
    /// </summary>
    public Style Application { get; set; } = new Style(Color.DeepSkyBlue3_1);

    /// <summary>
    ///     Gets or sets the style for the 'between' keyword.
    /// </summary>
    public Style Between { get; set; } = new Style(Color.DeepSkyBlue4_1);

    /// <summary>
    ///     Gets or sets the style for undefined values.
    /// </summary>
    public Style NotDefined { get; set; } = new Style();

    /// <summary>
    ///     Gets or sets the style for invalid tokens.
    /// </summary>
    public Style Invalid { get; set; } = new Style();
}