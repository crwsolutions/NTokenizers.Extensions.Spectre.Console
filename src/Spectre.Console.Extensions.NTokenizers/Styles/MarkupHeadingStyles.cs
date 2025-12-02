namespace Spectre.Console.Extensions.NTokenizers.Styles;

public class MarkupHeadingStyles
{
    public static MarkupHeadingStyles Default => new();

    public Style Level1 { get; set; } = Color.Yellow2;
    public Style Level2To4 { get; set; } = Color.DarkOliveGreen1_1;
    public Style Level5AndAbove { get; set; } = Color.DarkSeaGreen1_1;
}
