using NTokenizers.Markup.Metadata;

namespace Spectre.Console.Extensions.NTokenizers.Writers;

internal class MarkupEmojiWriter(IAnsiConsole ansiConsole)
{
    internal void Write(EmojiMetadata emojiMeta)
    {
        ansiConsole.Write(emojiMeta.Name);
    }
}