namespace Spectre.Console.Extensions.NTokenizers.Styles;
public class MarkupListItemStyles
{
    public static MarkupListItemStyles Default => new();

    public Style Marker { get; set; } = new Style(Color.Turquoise2);
}
