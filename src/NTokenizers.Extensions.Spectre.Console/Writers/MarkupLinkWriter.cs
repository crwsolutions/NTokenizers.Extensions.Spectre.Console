using NTokenizers.Markup.Metadata;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal class MarkupLinkWriter(IAnsiConsole ansiConsole)
{
    internal void Write(LinkMetadata linkMeta)
    {
        ansiConsole.Write($"[{linkMeta.Title}]({linkMeta.Url})");
    }
}