using NTokenizers.Kotlin;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Styles;

/// <summary>
/// Provides predefined styles for Kotlin syntax highlighting in Spectre.Console.
/// </summary>
public sealed class KotlinStyles
{
    /// <summary>
    /// Gets the default Kotlin styles.
    /// </summary>
    public static KotlinStyles Default => new();

    /// <summary>
    /// Gets or sets the style for tokens that do not fit any defined category.
    /// </summary>
    public Style NotDefined { get; set; } = new Style(Color.Gray);

    /// <summary>
    /// Gets or sets the style for operators.
    /// </summary>
    public Style Operator { get; set; } = new Style(Color.DeepSkyBlue4_2);

    /// <summary>
    /// Gets or sets the style for opening parentheses '('.
    /// </summary>
    public Style OpenParenthesis { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for closing parentheses ')'.
    /// </summary>
    public Style CloseParenthesis { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for opening braces '{'.
    /// </summary>
    public Style OpenBrace { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for closing braces '}'.
    /// </summary>
    public Style CloseBrace { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for opening brackets '['.
    /// </summary>
    public Style OpenBracket { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for closing brackets ']'.
    /// </summary>
    public Style CloseBracket { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for comma ',' characters.
    /// </summary>
    public Style Comma { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for dot '.' characters.
    /// </summary>
    public Style Dot { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for semicolons ';' - the statement terminator.
    /// </summary>
    public Style SequenceTerminator { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for colon ':' characters.
    /// </summary>
    public Style Colon { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for double colon '::' characters.
    /// </summary>
    public Style DoubleColon { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for question mark '?' characters.
    /// </summary>
    public Style QuestionMark { get; set; } = new Style(Color.Yellow);

    /// <summary>
    /// Gets or sets the style for at sign '@' characters.
    /// </summary>
    public Style At { get; set; } = new Style(Color.Magenta);

    /// <summary>
    /// Gets or sets the style for pound sign '#' characters.
    /// </summary>
    public Style Pound { get; set; } = new Style(Color.Magenta);

    /// <summary>
    /// Gets or sets the style for string values enclosed in double quotes.
    /// </summary>
    public Style StringValue { get; set; } = new Style(Color.DarkSlateGray1);

    /// <summary>
    /// Gets or sets the style for char values enclosed in single quotes.
    /// </summary>
    public Style CharValue { get; set; } = new Style(Color.DarkSlateGray1);

    /// <summary>
    /// Gets or sets the style for numeric values.
    /// </summary>
    public Style Number { get; set; } = new Style(Color.Blue);

    /// <summary>
    /// Gets or sets the style for boolean values (true, false).
    /// </summary>
    public Style Boolean { get; set; } = new Style(Color.Blue);

    /// <summary>
    /// Gets or sets the style for null values.
    /// </summary>
    public Style Null { get; set; } = new Style(Color.Blue);

    /// <summary>
    /// Gets or sets the style for identifiers including variable names, function names, etc.
    /// </summary>
    public Style Identifier { get; set; } = new Style(Color.White);

    /// <summary>
    /// Gets or sets the style for Kotlin keywords.
    /// </summary>
    public Style Keyword { get; set; } = new Style(Color.Turquoise2);

    /// <summary>
    /// Gets or sets the style for comments (// or /* */).
    /// </summary>
    public Style Comment { get; set; } = new Style(Color.Green);

    /// <summary>
    /// Gets or sets the style for whitespace characters.
    /// </summary>
    public Style Whitespace { get; set; } = new Style(Color.White);
}
