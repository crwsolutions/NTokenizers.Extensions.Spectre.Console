using NTokenizers.Markup.Metadata;
using Spectre.Console.Extensions.NTokenizers.Styles;

namespace Spectre.Console.Extensions.NTokenizers.Writers;

internal sealed class MarkupOrderedListItemWriter(IAnsiConsole ansiConsole, MarkupOrderedListItemStyles styles)
{
    internal async Task WriteAsync(OrderedListItemMetadata metadata)
    {
        ansiConsole.Write(new Console.Markup($"{metadata.Number}. ", styles.Number));
        await metadata.RegisterInlineTokenHandler(
            async token => await MarkupWriter.Create(ansiConsole).WriteAsync(token));
        ansiConsole.WriteLine();
    }
}
