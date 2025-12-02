using NTokenizers.Markup.Metadata;
using NTokenizers.Extensions.Spectre.Console.Styles;
using SpectreLib = Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class MarkupListItemWriter(SpectreLib.IAnsiConsole ansiConsole, MarkupListItemStyles styles)
{
    internal async Task WriteAsync(ListItemMetadata metadata)
    {
        ansiConsole.Write(new SpectreLib.Markup($"{metadata.Marker} ", styles.Marker));
        await metadata.RegisterInlineTokenHandler(
            async token => await MarkupWriter.Create(ansiConsole).WriteAsync(token));
        ansiConsole.Write(new SpectreLib.Text("\n")); //HACK
    }
}