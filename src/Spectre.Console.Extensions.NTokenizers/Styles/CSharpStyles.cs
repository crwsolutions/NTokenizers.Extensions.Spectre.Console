using NTokenizers.CSharp;
using Spectre.Console;

namespace Spectre.Console.Extensions.NTokenizers.Styles;

public sealed class CSharpStyles
{
    public static CSharpStyles Default => new();

    public Style Keyword { get; set; } = new Style(Color.Turquoise2);
    public Style Number { get; set; } = new Style(Color.Blue);
    public Style StringValue { get; set; } = new Style(Color.DarkSlateGray2);
    public Style Comment { get; set; } = new Style(Color.Green);
    public Style Identifier { get; set; } = new Style(Color.White);
    public Style And { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style Or { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style Not { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style EqualsStyle { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style NotEquals { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style GreaterThan { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style LessThan { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style GreaterThanOrEqual { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style LessThanOrEqual { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style Plus { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style Minus { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style Multiply { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style Divide { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style Modulo { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style Operator { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style OpenParenthesis { get; set; } = new Style(Color.Yellow);
    public Style CloseParenthesis { get; set; } = new Style(Color.Yellow);
    public Style Comma { get; set; } = new Style(Color.Yellow);
    public Style Dot { get; set; } = new Style(Color.Yellow);
    public Style SequenceTerminator { get; set; } = new Style(Color.Yellow);
    public Style Whitespace { get; set; } = new Style(Color.White);
}