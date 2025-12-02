using NTokenizers.Markup.Metadata;
using NTokenizers.Extensions.Spectre.Console.Styles;
using SpectreLib = Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class MarkupOrderedListItemWriter(SpectreLib.IAnsiConsole ansiConsole, MarkupOrderedListItemStyles styles)
{
    internal async Task WriteAsync(OrderedListItemMetadata metadata)
    {
        ansiConsole.Write(new SpectreLib.Markup($"{metadata.Number}. ", styles.Number));
        await metadata.RegisterInlineTokenHandler(
            async token => await MarkupWriter.Create(ansiConsole).WriteAsync(token));
        ansiConsole.Write(new SpectreLib.Text("\n")); //HACK
    }
}
