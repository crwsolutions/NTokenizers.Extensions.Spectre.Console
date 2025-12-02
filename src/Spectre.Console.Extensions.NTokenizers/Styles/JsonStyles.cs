namespace Spectre.Console.Extensions.NTokenizers.Styles;

public sealed class JsonStyles
{
    public static JsonStyles Default => new();

    public Style StartObject { get; set; } = new Style(Color.DeepSkyBlue4_1);
    public Style EndObject { get; set; } = new Style(Color.DeepSkyBlue4_1);
    public Style StartArray { get; set; } = new Style(Color.DeepSkyBlue4_1);
    public Style EndArray { get; set; } = new Style(Color.DeepSkyBlue4_1);
    public Style PropertyName { get; set; } = new Style(Color.DeepSkyBlue3_1);
    public Style Colon { get; set; } = new Style(Color.Yellow);
    public Style Comma { get; set; } = new Style(Color.Yellow);
    public Style StringValue { get; set; } = new Style(Color.DarkSlateGray1);
    public Style Number { get; set; } = new Style(Color.Blue);
    public Style True { get; set; } = new Style(Color.Blue);
    public Style False { get; set; } = new Style(Color.Blue);
    public Style Null { get; set; } = new Style(Color.Blue);
}