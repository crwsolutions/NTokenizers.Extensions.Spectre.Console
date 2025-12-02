using NTokenizers.Core;
using NTokenizers.Markup.Metadata;
using SpectreLib = Spectre.Console;
using System.Diagnostics;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal abstract class BaseInlineWriter<TToken, TTokentype>(SpectreLib.IAnsiConsole ansiConsole) where TToken : IToken<TTokentype> where TTokentype : Enum
{
    protected virtual SpectreLib.Style GetStyle(TTokentype token) => SpectreLib.Style.Plain;

    protected readonly SpectreLib.Paragraph _liveParagraph = new("");

    internal void WriteToken(TToken token)
    {
        ansiConsole.Write(new SpectreLib.Markup(SpectreLib.Markup.Escape(token.Value), GetStyle(token.TokenType)));
    }

    internal async Task WriteAsync(InlineMarkupMetadata<TToken> metadata)
    {
        var liveDisplay = new SpectreLib.LiveDisplay(ansiConsole, GetIRendable());
        await liveDisplay
        .StartAsync(async ctx =>
        {
            await StartedAsync(metadata);
            await metadata.RegisterInlineTokenHandler(async inlineToken =>
            {
                await WriteTokenAsync(_liveParagraph, inlineToken);
                ctx.Refresh();
            });

            await FinalizeAsync(metadata);
            ctx.Refresh();
        });
    }

    protected virtual SpectreLib.Rendering.IRenderable GetIRendable()
    {
        return new SpectreLib.Panel(_liveParagraph)
            .Border(new LeftBoxBorder())
            .BorderStyle(new SpectreLib.Style(SpectreLib.Color.Green))
            ;
    }

    protected virtual Task StartedAsync(InlineMarkupMetadata<TToken> metadata) => Task.CompletedTask;

    protected virtual Task FinalizeAsync(InlineMarkupMetadata<TToken> metadata) => Task.CompletedTask;

    protected virtual Task WriteTokenAsync(SpectreLib.Paragraph liveParagraph, TToken token)
    {
        if (!string.IsNullOrEmpty(token.Value))
        {
            Debug.WriteLine($"Writing token: `{token.Value}` of type `{token.TokenType}`");
            liveParagraph.Append(token.Value, GetStyle(token.TokenType));
        }
        return Task.CompletedTask;
    }
}
