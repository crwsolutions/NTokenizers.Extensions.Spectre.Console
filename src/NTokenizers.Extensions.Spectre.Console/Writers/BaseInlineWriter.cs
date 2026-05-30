using NTokenizers.Core;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Diagnostics;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal abstract class BaseInlineWriter<TToken, TTokentype> where TToken : IToken<TTokentype> where TTokentype : Enum
{
    internal protected readonly IAnsiConsole _ansiConsole;
    internal protected readonly LiveDisplayContext? _liveDisplayContext;
    internal protected readonly Paragraph _liveParagraph;

    internal BaseInlineWriter(IAnsiConsole ansiConsole)
    {
        _ansiConsole = ansiConsole;
        _liveDisplayContext = null;
        _liveParagraph = new("");
    }

    internal BaseInlineWriter(IAnsiConsole ansiConsole, Paragraph? liveParagraph, LiveDisplayContext? ctx)
    {
        _ansiConsole = ansiConsole;
        _liveParagraph = liveParagraph ?? new("");
        _liveDisplayContext = ctx;
    }

    protected virtual Style GetStyle(TTokentype token) => Style.Plain;

    internal void WriteToken(TToken token)
    {
        _ansiConsole.Write(new Markup(Markup.Escape(token.Value), GetStyle(token.TokenType)));
    }

    internal void WriteTokenInLiveTarget(TToken token)
    {
        WriteToken(_liveParagraph, token);
        _liveDisplayContext?.Refresh();
    }

    internal async Task WriteAsync(InlineMetadata<TToken> metadata)
    {
        var liveDisplay = new LiveDisplay(_ansiConsole, GetIRendable());
        await liveDisplay
        .StartAsync(async ctx =>
        {
            await StartedAsync(metadata);
            await metadata.RegisterInlineTokenHandler(async inlineToken =>
            {
                await WriteTokenAsync(_liveParagraph, inlineToken, ctx);
                ctx.Refresh();
            });

            await FinalizeAsync(metadata);
            ctx.Refresh();
        });
    }

    protected virtual IRenderable GetIRendable() => 
        new Panel(_liveParagraph)
            .Border(new LeftBoxBorder())
            .BorderStyle(new Style(Color.Green));

    protected virtual Task StartedAsync(InlineMetadata<TToken> metadata) => Task.CompletedTask;

    protected virtual Task FinalizeAsync(InlineMetadata<TToken> metadata) => Task.CompletedTask;

    protected virtual Task WriteTokenAsync(Paragraph? liveParagraph, TToken token, LiveDisplayContext? ctx)
    {
        WriteToken(liveParagraph, token);
        return Task.CompletedTask;
    }

    protected virtual void WriteToken(Paragraph? liveParagraph, TToken token)
    {
        if (token.Value is not null)
        {
            Debug.WriteLine($"Writing token: `{token.Value}` of type `{token.TokenType}`");

            if (liveParagraph is null)
            {
                _ansiConsole.Write(new Markup(Markup.Escape(token.Value), GetStyle(token.TokenType)));
            }
            else
            {
                liveParagraph.Append(token.Value, GetStyle(token.TokenType));
            }
        }
    }
}
