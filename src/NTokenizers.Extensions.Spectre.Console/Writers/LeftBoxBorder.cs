using Spectre.Console;
using Spectre.Console.Rendering;

internal sealed class LeftBoxBorder : BoxBorder
{
    public override string GetPart(BoxBorderPart part) => part switch
    {
        BoxBorderPart.TopLeft => "│",
        BoxBorderPart.Left => "│",
        BoxBorderPart.BottomLeft => "│",
        _ => " "
    };
}