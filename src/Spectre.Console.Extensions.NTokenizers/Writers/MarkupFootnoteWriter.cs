using NTokenizers.Markup.Metadata;

namespace Spectre.Console.Extensions.NTokenizers.Writers;

internal class MarkupFootnoteWriter(IAnsiConsole ansiConsole)
{
    internal void Write(FootnoteMetadata footnoteMeta)
    {
        ansiConsole.Write($"[^{footnoteMeta.Id}]");
    }
}