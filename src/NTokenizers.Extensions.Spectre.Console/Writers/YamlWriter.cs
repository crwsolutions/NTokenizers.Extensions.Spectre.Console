using NTokenizers.Yaml;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class YamlWriter(IAnsiConsole ansiConsole, YamlStyles styles) : BaseInlineWriter<YamlToken, YamlTokenType>(ansiConsole)
{
    protected override Style GetStyle(YamlTokenType token) => token switch
    {
        YamlTokenType.Directive => styles.Directive,
        YamlTokenType.DirectiveKey => styles.DirectiveKey,
        YamlTokenType.DirectiveValue => styles.DirectiveValue,
        YamlTokenType.DocumentStart => styles.DocumentStart,
        YamlTokenType.DocumentEnd => styles.DocumentEnd,
        YamlTokenType.Comment => styles.Comment,
        YamlTokenType.Key => styles.Key,
        YamlTokenType.Colon => styles.Colon,
        YamlTokenType.Value => styles.Value,
        YamlTokenType.Quote => styles.Quote,
        YamlTokenType.String => styles.String,
        YamlTokenType.Anchor => styles.Anchor,
        YamlTokenType.Alias => styles.Alias,
        YamlTokenType.Tag => styles.Tag,
        YamlTokenType.FlowSeqStart => styles.FlowSeqStart,
        YamlTokenType.FlowSeqEnd => styles.FlowSeqEnd,
        YamlTokenType.FlowMapStart => styles.FlowMapStart,
        YamlTokenType.FlowMapEnd => styles.FlowMapEnd,
        YamlTokenType.FlowEntry => styles.FlowEntry,
        YamlTokenType.BlockSeqEntry => styles.BlockSeqEntry,
        YamlTokenType.Whitespace => styles.Whitespace,
        _ => new Style()
    };
}