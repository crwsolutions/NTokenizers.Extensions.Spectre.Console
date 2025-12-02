using NTokenizers.Markup;

namespace Spectre.Console.Extensions.NTokenizers.Writers;

internal sealed class GenericWriter(IAnsiConsole ansiConsole) : BaseInlineWriter<MarkupToken, MarkupTokenType>(ansiConsole);