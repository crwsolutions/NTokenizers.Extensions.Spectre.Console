using Spectre.Console;
using NTokenizersMarkup = NTokenizers.Markup.Metadata;

namespace NTokenizers.Extensions.Spectre.Console.Extensions;
internal static class JustifyExtensions
{
    internal static Justify ToSpectreJustify(this NTokenizersMarkup.Justify justify) => justify switch
    {
        NTokenizersMarkup.Justify.Left => Justify.Left,
        NTokenizersMarkup.Justify.Center => Justify.Center,
        NTokenizersMarkup.Justify.Right => Justify.Right,
        _ => Justify.Left // fallback
    };
}
