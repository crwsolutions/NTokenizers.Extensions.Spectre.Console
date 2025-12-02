using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Styles;

/// <summary>
///     Provides predefined styles for SQL token highlighting in Spectre.Console.
///     This class defines color styles for various SQL token types to enable
///     style-rich console syntax highlighting for SQL content.
/// </summary>
public sealed class SqlStyles
{
    /// <summary>
    ///     Gets the default SQL styles.
    /// </summary>
    public static SqlStyles Default => new SqlStyles();

    /// <summary>
    ///     Gets or sets the style for numeric literals in SQL.
    /// </summary>
    public Style Number { get; set; } = new Style(Color.DeepSkyBlue3_1);
    
    /// <summary>
    ///     Gets or sets the style for string values in SQL.
    /// </summary>
    public Style StringValue { get; set; } = new Style(Color.DarkSlateGray1);
    
    /// <summary>
    ///     Gets or sets the style for comments in SQL.
    /// </summary>
    public Style Comment { get; set; } = new Style(Color.Green);
    
    /// <summary>
    ///     Gets or sets the style for SQL keywords.
    /// </summary>
    public Style Keyword { get; set; } = new Style(Color.Turquoise2);
    
    /// <summary>
    ///     Gets or sets the style for identifiers (table names, column names, etc.) in SQL.
    /// </summary>
    public Style Identifier { get; set; } = new Style(Color.White);
    
    /// <summary>
    ///     Gets or sets the style for operators in SQL.
    /// </summary>
    public Style Operator { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    ///     Gets or sets the style for commas in SQL.
    /// </summary>
    public Style Comma { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    ///     Gets or sets the style for dots (periods) in SQL.
    /// </summary>
    public Style Dot { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    ///     Gets or sets the style for open parentheses in SQL.
    /// </summary>
    public Style OpenParenthesis { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    ///     Gets or sets the style for close parentheses in SQL.
    /// </summary>
    public Style CloseParenthesis { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    ///     Gets or sets the style for sequence terminators in SQL.
    /// </summary>
    public Style SequenceTerminator { get; set; } = new Style(Color.DeepSkyBlue4_2);
    
    /// <summary>
    ///     Gets or sets the style for undefined or unknown tokens in SQL.
    /// </summary>
    public Style NotDefined { get; set; } = new Style(Color.DeepSkyBlue4_2);
}