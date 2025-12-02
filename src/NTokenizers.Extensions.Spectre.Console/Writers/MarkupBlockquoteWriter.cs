using NTokenizers.Markup;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class MarkupBlockquoteWriter(IAnsiConsole ansiConsole) : BaseInlineWriter<MarkupToken, MarkupTokenType>(ansiConsole)
{
    protected override async Task WriteTokenAsync(Paragraph liveParagraph, MarkupToken token) =>
        await MarkupWriter.Create(ansiConsole).WriteAsync(liveParagraph, token, GetStyle(token.TokenType));
}
