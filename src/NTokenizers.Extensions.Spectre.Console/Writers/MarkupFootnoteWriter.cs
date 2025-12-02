using NTokenizers.Markup.Metadata;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal class MarkupFootnoteWriter(IAnsiConsole ansiConsole)
{
    internal void Write(FootnoteMetadata footnoteMeta)
    {
        ansiConsole.Write($"[^{footnoteMeta.Id}]");
    }
}