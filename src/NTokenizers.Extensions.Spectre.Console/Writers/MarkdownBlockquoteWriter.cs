using NTokenizers.Markdown;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class MarkdownBlockquoteWriter(IAnsiConsole ansiConsole) : BaseInlineWriter<MarkdownToken, MarkdownTokenType>(ansiConsole)
{
    protected override async Task WriteTokenAsync(Paragraph? liveParagraph, MarkdownToken token, LiveDisplayContext? ctx) =>
        await MarkdownWriter.Create(ansiConsole).WriteAsync(liveParagraph, token, GetStyle(token.TokenType));
}
