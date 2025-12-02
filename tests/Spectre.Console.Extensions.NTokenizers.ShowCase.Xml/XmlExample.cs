namespace Spectre.Console.Extensions.NTokenizers.ShowCase.Xml;

internal static class XmlExample
{
    internal static string GetSampleXml() =>
        """
        <?xml version="1.0"?>
        <glossary>
          <title>example glossary</title>
          <GlossDiv><title>S</title>
          <GlossList>
           <!-- GlossEntry -->
           <GlossEntry ID="SGML" SortAs="SGML">
            <GlossTerm>Standard Generalized Markup Language</GlossTerm>
            <Acronym>SGML</Acronym>
            <Abbrev>ISO 8879:1986</Abbrev>
            <GlossDef>
             <para>A meta-markup language, used to create markup languages such as DocBook.</para>
             <GlossSeeAlso OtherTerm="GML" >
             <GlossSeeAlso OtherTerm="XML" >
             <![CDATA[This is CDATA content.]]>
            </GlossDef>
            <GlossSee OtherTerm="markup"/>
           </GlossEntry>
          </GlossList>
         </GlossDiv>
        </glossary>
        """;
}
