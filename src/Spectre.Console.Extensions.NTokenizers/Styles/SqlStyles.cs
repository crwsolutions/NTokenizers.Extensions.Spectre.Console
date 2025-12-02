using Spectre.Console;

namespace Spectre.Console.Extensions.NTokenizers.Styles;

public sealed class SqlStyles
{
    public static SqlStyles Default => new SqlStyles();

    public Style Number { get; set; } = new Style(Color.DeepSkyBlue3_1);
    public Style StringValue { get; set; } = new Style(Color.DarkSlateGray1);
    public Style Comment { get; set; } = new Style(Color.Green);
    public Style Keyword { get; set; } = new Style(Color.Turquoise2);
    public Style Identifier { get; set; } = new Style(Color.White);
    public Style Operator { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style Comma { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style Dot { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style OpenParenthesis { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style CloseParenthesis { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style SequenceTerminator { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style NotDefined { get; set; } = new Style(Color.DeepSkyBlue4_2);
}