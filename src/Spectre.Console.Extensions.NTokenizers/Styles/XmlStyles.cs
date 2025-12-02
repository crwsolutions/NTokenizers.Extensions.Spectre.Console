namespace Spectre.Console.Extensions.NTokenizers.Styles;

public sealed class XmlStyles
{
    public static XmlStyles Default => new XmlStyles();
    
    public Style ElementName { get; set; } = new Style(Color.DeepSkyBlue3_1);
    public Style Text { get; set; } = new Style(Color.DarkSlateGray1);
    public Style Comment { get; set; } = new Style(Color.Green);
    public Style ProcessingInstruction { get; set; } = new Style(Color.Turquoise2);
    public Style DocumentTypeDeclaration { get; set; } = new Style(Color.Turquoise2);
    public Style CData { get; set; } = new Style(Color.Magenta1);
    public Style Whitespace { get; set; } = new Style(Color.Yellow);
    public Style EndElement { get; set; } = new Style(Color.DeepSkyBlue3_1);
    public Style OpeningAngleBracket { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style ClosingAngleBracket { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style AttributeName { get; set; } = new Style(Color.Turquoise2);
    public Style AttributeEquals { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style AttributeValue { get; set; } = new Style(Color.White);
    public Style AttributeQuote { get; set; } = new Style(Color.DeepSkyBlue3_1);
    public Style SelfClosingSlash { get; set; } = new Style(Color.DeepSkyBlue4_2);
    public Style DefaultStyle { get; set; } = new Style();
}