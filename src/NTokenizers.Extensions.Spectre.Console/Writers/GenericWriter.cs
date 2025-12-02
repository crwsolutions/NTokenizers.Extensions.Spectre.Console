using NTokenizers.Markup;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class GenericWriter(IAnsiConsole ansiConsole) : BaseInlineWriter<MarkupToken, MarkupTokenType>(ansiConsole);