using NTokenizers.Markdown;
using NTokenizers.Markdown.Metadata;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console.Rendering;
using Spectre.Console;
using NTokenizers.Core;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class MarkdownHeadingWriter : BaseInlineWriter<MarkdownToken, MarkdownTokenType>
{
    private readonly MarkdownHeadingStyles _styles;
    private Style _style = default;
    private int _lenght = 0;

    internal MarkdownHeadingWriter(IAnsiConsole ansiConsole, MarkdownHeadingStyles styles) : base(ansiConsole)
    {
        _styles = styles;
    }

    protected override Style GetStyle(MarkdownTokenType token) => _style;

    protected override async Task StartedAsync(InlineMetadata<MarkdownToken> metadata)
    {
        _liveParagraph.Append("\n");
        if (metadata is HeadingMetadata meta)
        {
            _style = meta.Level switch
            {
                1 => _styles.Level1,
                >= 2 and <= 4 => _styles.Level2To4,
                _ => _styles.Level5AndAbove
            };
            if (meta.Level == 1)
            {
                await WriteToken("** ");
            }
        }
    }

    protected override async Task FinalizeAsync(InlineMetadata<MarkdownToken> metadata)
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
        await WriteTokenAsync(_liveParagraph, new MarkdownToken(MarkdownTokenType.Text, text), null);
    }

    protected override async Task WriteTokenAsync(Paragraph? liveParagraph, MarkdownToken token, LiveDisplayContext? ctx)
    {
        _lenght += token.Value.Length;
        await MarkdownWriter.Create(_ansiConsole).WriteAsync(liveParagraph, token, _style);
    }

    protected override IRenderable GetIRendable() => _liveParagraph;
}
