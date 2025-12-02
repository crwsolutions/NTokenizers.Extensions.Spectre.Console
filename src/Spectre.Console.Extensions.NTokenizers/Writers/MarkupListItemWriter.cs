using NTokenizers.Markup.Metadata;
using Spectre.Console.Extensions.NTokenizers.Styles;

namespace Spectre.Console.Extensions.NTokenizers.Writers;

internal sealed class MarkupListItemWriter(IAnsiConsole ansiConsole, MarkupListItemStyles styles)
{
    internal async Task WriteAsync(ListItemMetadata metadata)
    {
        ansiConsole.Write(new Console.Markup($"{metadata.Marker} ", styles.Marker));
        await metadata.RegisterInlineTokenHandler(
            async token => await MarkupWriter.Create(ansiConsole).WriteAsync(token));
        ansiConsole.WriteLine();
    }
}