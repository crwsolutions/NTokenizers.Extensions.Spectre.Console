using NTokenizers.Markup;
using NTokenizers.Markup.Metadata;
using Spectre.Console.Extensions.NTokenizers.Styles;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.NTokenizers.Writers;

internal sealed class MarkupHeadingWriter(IAnsiConsole ansiConsole, MarkupHeadingStyles styles) : BaseInlineWriter<MarkupToken, MarkupTokenType>(ansiConsole)
{
    private Style _style = default!;
    private int _lenght = 0;

    protected override Style GetStyle(MarkupTokenType token) => _style;

    protected override async Task StartedAsync(InlineMarkupMetadata<MarkupToken> metadata)
    {
        _liveParagraph.Append("\n");
        if (metadata is HeadingMetadata meta)
        {
            _style = meta.Level switch
            {
                1 => styles.Level1,
                >= 2 and <= 4 => styles.Level2To4,
                _ => styles.Level5AndAbove
            };
            if (meta.Level == 1)
            {
                await WriteToken("** ");
            }
        }
    }

    protected override async Task FinalizeAsync(InlineMarkupMetadata<MarkupToken> metadata)
    {
        if (metadata is HeadingMetadata meta)
        {
            if (meta.Level == 1)
            {
                await WriteToken(" **");
            }
            if (meta.Level < 3)
            {
                await WriteToken($"\n{new string('=', _lenght)}");
            }
            else if (meta.Level < 5)
            {
                await WriteToken($"\n{new string('-', _lenght)}");
            }

            //WriteToken("\n");
        }
    }

    private async Task WriteToken(string text)
    {
        await WriteTokenAsync(_liveParagraph, new MarkupToken(MarkupTokenType.Text, text));
    }

    protected override async Task WriteTokenAsync(Paragraph liveParagraph, MarkupToken token)
    {
        _lenght += token.Value.Length;
        await MarkupWriter.Create(ansiConsole).WriteAsync(liveParagraph, token, _style);
    }

    protected override IRenderable GetIRendable() => _liveParagraph;
}
