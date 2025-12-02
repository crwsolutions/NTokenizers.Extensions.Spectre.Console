using NTokenizers.CSharp;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Styles;

/// <summary>
/// Provides predefined styles for C# syntax highlighting in Spectre.Console.
/// This class defines color styles for various C# language tokens including keywords, literals, comments, and operators.
/// </summary>
public sealed class CSharpStyles
{
    /// <summary>
    /// Gets the default C# styles.
    /// </summary>
    public static CSharpStyles Default => new();

    /// <summary>
    /// Gets or sets the style for C# keywords such as 'class', 'public', 'static', etc.
    /// </summary>
    public Style Keyword { get; set; } = new Style(Color.Turquoise2);
    
    /// <summary>
    /// Gets or sets the style for numeric literals including integers and floating-point numbers.
    /// </summary>
    public Style Number { get; set; } = new Style(Color.Blue);
    
    /// <summary>
    /// Gets or sets the style for string literals enclosed in double quotes.
    /// </summary>
    public Style StringValue { get; set; } = new Style(Color.DarkSlateGray2);
    
    /// <summary>
    /// Gets or sets the style for single-line and multi-line comments.
    /// </summary>
    public Style Comment { get; set; } = new Style(Color.Green);
    
    /// <summary>
    /// Gets or sets the style for identifiers including variable names, method names, and class names.
    /// </summary>
    public Style Identifier { get; set; } = new Style(Color.White);
    
    /// <summary>
    /// Gets or sets the style for the 'and' operator.
    /// </summary>
    public Style And { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for the 'or' operator.
    /// </summary>
    public Style Or { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for the 'not' operator.
    /// </summary>
    public Style Not { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for the '==' operator.
    /// </summary>
    public Style EqualsStyle { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for the '!=' operator.
    /// </summary>
    public Style NotEquals { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for the '>' operator.
    /// </summary>
    public Style GreaterThan { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for the '&lt;' operator.
    /// </summary>
    public Style LessThan { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for the '>=' operator.
    /// </summary>
    public Style GreaterThanOrEqual { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for the '&lt;=' operator.
    /// </summary>
    public Style LessThanOrEqual { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for the '+' operator.
    /// </summary>
    public Style Plus { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for the '-' operator.
    /// </summary>
    public Style Minus { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for the '*' operator.
    /// </summary>
    public Style Multiply { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for the '/' operator.
    /// </summary>
    public Style Divide { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for the '%' operator.
    /// </summary>
    public Style Modulo { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    /// Gets or sets the style for other operators not specifically categorized.
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
    /// Gets or sets the style for comma ',' characters.
    /// </summary>
    public Style Comma { get; set; } = new Style(Color.Yellow);
    
    /// <summary>
    /// Gets or sets the style for dot '.' characters used for member access.
    /// </summary>
    public Style Dot { get; set; } = new Style(Color.Yellow);
    
    /// <summary>
    /// Gets or sets the style for sequence terminators such as semicolons ';'.
    /// </summary>
    public Style SequenceTerminator { get; set; } = new Style(Color.Yellow);
    
    /// <summary>
    /// Gets or sets the style for whitespace characters including spaces and tabs.
    /// </summary>
    public Style Whitespace { get; set; } = new Style(Color.White);
}