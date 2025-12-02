using NTokenizers.Xml;
using Spectre.Console.Extensions.NTokenizers.Styles;

namespace Spectre.Console.Extensions.NTokenizers.Writers;

internal sealed class XmlWriter(IAnsiConsole ansiConsole, XmlStyles styles) : BaseInlineWriter<XmlToken, XmlTokenType>(ansiConsole)
{
    protected override Style GetStyle(XmlTokenType token) => token switch
    {
        XmlTokenType.ElementName => styles.ElementName,
        XmlTokenType.Text => styles.Text,
        XmlTokenType.Comment => styles.Comment,
        XmlTokenType.ProcessingInstruction => styles.ProcessingInstruction,
        XmlTokenType.DocumentTypeDeclaration => styles.DocumentTypeDeclaration,
        XmlTokenType.CData => styles.CData,
        XmlTokenType.Whitespace => styles.Whitespace,
        XmlTokenType.EndElement => styles.EndElement,
        XmlTokenType.OpeningAngleBracket => styles.OpeningAngleBracket,
        XmlTokenType.ClosingAngleBracket => styles.ClosingAngleBracket,
        XmlTokenType.AttributeName => styles.AttributeName,
        XmlTokenType.AttributeEquals => styles.AttributeEquals,
        XmlTokenType.AttributeValue => styles.AttributeValue,
        XmlTokenType.AttributeQuote => styles.AttributeQuote,
        XmlTokenType.SelfClosingSlash => styles.SelfClosingSlash,
        _ => styles.DefaultStyle,
    };
}