using NTokenizers.Markdown;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class MarkdownBlockquoteWriter : BaseInlineWriter<MarkdownToken, MarkdownTokenType>
{
    internal MarkdownBlockquoteWriter(IAnsiConsole ansiConsole) : base(ansiConsole) { }

    protected override async Task WriteTokenAsync(Paragraph? liveParagraph, MarkdownToken token, LiveDisplayContext? ctx) =>
        await MarkdownWriter.Create(_ansiConsole).WriteAsync(liveParagraph, token, GetStyle(token.TokenType));
}
