using NTokenizers.Markup.Metadata;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal class MarkupEmojiWriter(IAnsiConsole ansiConsole)
{
    internal void Write(EmojiMetadata emojiMeta)
    {
        ansiConsole.Write(emojiMeta.Name);
    }
}