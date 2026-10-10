using NTokenizers.Core;
using Spectre.Console;
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

    /// <summary>
    /// Writes a token to the streaming output with the token's style. Used by fenced/indented
    /// code blocks (which stream to the shared console rather than into a live region). The stream
    /// escapes the value, so the raw token value is passed.
    /// </summary>
    /// <param name="stream">The streaming output to write the token to.</param>
    /// <param name="token">The token to write.</param>
    internal virtual void WriteToStream(MarkdownStream stream, TToken token)
    {
        stream.Write(token.Value, GetStyle(token.TokenType));
    }

    internal void WriteTokenInLiveTarget(TToken token)
    {
        WriteToken(_liveParagraph, token);
        _liveDisplayContext?.Refresh();
    }

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
