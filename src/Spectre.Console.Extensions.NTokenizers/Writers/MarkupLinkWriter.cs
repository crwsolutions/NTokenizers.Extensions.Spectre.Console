using NTokenizers.Markup.Metadata;

namespace Spectre.Console.Extensions.NTokenizers.Writers;

internal class MarkupLinkWriter(IAnsiConsole ansiConsole)
{
    internal void Write(LinkMetadata linkMeta)
    {
        ansiConsole.Write($"[{linkMeta.Title}]({linkMeta.Url})");
    }
}