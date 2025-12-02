namespace Spectre.Console.Extensions.NTokenizers.Styles;

public sealed class TypescriptStyles
{
    public static TypescriptStyles Default => new();

    public Style OpenParenthesis { get; set; } = new Style(Color.DeepSkyBlue4_1);
    public Style CloseParenthesis { get; set; } = new Style(Color.DeepSkyBlue4_1);
    public Style Comma { get; set; } = new Style(Color.Yellow);
    public Style StringValue { get; set; } = new Style(Color.DarkSlateGray1);
    public Style Number { get; set; } = new Style(Color.Blue);
    public Style Keyword { get; set; } = new Style(Color.Turquoise2);
    public Style Identifier { get; set; } = new Style(Color.White);
    public Style Comment { get; set; } = new Style(Color.Green);
    public Style Operator { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style And { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style Or { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style EqualsStyle { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style NotEquals { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style In { get; set; } = new Style(Color.DeepSkyBlue4_1);
    public Style NotIn { get; set; } = new Style(Color.DeepSkyBlue4_1);
    public Style Like { get; set; } = new Style(Color.DeepSkyBlue4_1);
    public Style NotLike { get; set; } = new Style(Color.DeepSkyBlue4_1);
    public Style Limit { get; set; } = new Style(Color.DeepSkyBlue4_1);
    public Style Match { get; set; } = new Style(Color.DeepSkyBlue4_1);
    public Style SequenceTerminator { get; set; } = new Style(Color.Yellow);
    public Style Dot { get; set; } = new Style(Color.Yellow);
    public Style Whitespace { get; set; } = new Style(Color.Yellow);
    public Style DateTimeValue { get; set; } = new Style(Color.Blue);
    public Style Fingerprint { get; set; } = new Style(Color.DeepSkyBlue3_1);
    public Style Message { get; set; } = new Style(Color.DeepSkyBlue3_1);
    public Style StackFrame { get; set; } = new Style(Color.DeepSkyBlue3_1);
    public Style ExceptionType { get; set; } = new Style(Color.DeepSkyBlue3_1);
    public Style Application { get; set; } = new Style(Color.DeepSkyBlue3_1);
    public Style Between { get; set; } = new Style(Color.DeepSkyBlue4_1);
    public Style NotDefined { get; set; } = new Style();
    public Style Invalid { get; set; } = new Style();
}