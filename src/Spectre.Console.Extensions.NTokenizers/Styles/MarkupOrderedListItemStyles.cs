namespace Spectre.Console.Extensions.NTokenizers.Styles;

public class MarkupOrderedListItemStyles
{
    public static MarkupOrderedListItemStyles Default => new();

    public Style Number { get; set; } = new Style(Color.Aqua);
}