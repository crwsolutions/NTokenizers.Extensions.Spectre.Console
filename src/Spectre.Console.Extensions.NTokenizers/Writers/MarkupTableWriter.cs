using NTokenizers.Markup;
using NTokenizers.Markup.Metadata;
using Spectre.Console.Extensions.NTokenizers.Extensions;
using Spectre.Console.Extensions.NTokenizers.Styles;
using System.Diagnostics;

namespace Spectre.Console.Extensions.NTokenizers.Writers;

internal class MarkupTableWriter(IAnsiConsole ansiConsole, MarkupStyles markupStyles)
{
    internal async Task WriteAsync(TableMetadata metadata)
    {
        var spectreTable = new Table();

        var column = -1;
        TableRow? currentRow = null;
        var cellParagraphs = new List<Paragraph>();
        var liveParagraph = new Paragraph();
        await ansiConsole.Live(spectreTable)
        .StartAsync(async ctx =>
        {
            await metadata.RegisterInlineTokenHandler(async inlineToken =>
            {
                if (inlineToken.TokenType == MarkupTokenType.TableAlignments)
                {
                    HandleAlignments(spectreTable, metadata);
                }
                else if (inlineToken.TokenType == MarkupTokenType.TableRow)
                {
                    //Handle new row
                    column = -1;

                    if (spectreTable.Columns.Count > 0)
                    { 
                        cellParagraphs = Enumerable.Range(0, spectreTable.Columns.Count).Select(_ => new Paragraph()).ToList();
                        currentRow = new TableRow(cellParagraphs);
                        spectreTable.AddRow(currentRow);
                    }
                }
                else if (inlineToken.TokenType == MarkupTokenType.TableCell)
                {
                    column++;
                    if (spectreTable.Rows.Count == 0)
                    {
                        liveParagraph = new Paragraph();
                        spectreTable.AddColumn(new TableColumn(liveParagraph));
                    }
                    else
                    {
                        if (column < cellParagraphs.Count)
                        {
                            liveParagraph = cellParagraphs[column];
                        }
                    }
                }
                else //Write cell content
                {
                    await WriteTokenAsync(liveParagraph, inlineToken);
                }

                ctx.Refresh();
            });

            ctx.Refresh();
        });
    }

    private void HandleAlignments(Table spectreTable, TableMetadata metadata)
    {
        if (metadata.Alignments == null || metadata.Alignments.Count == 0)
        {
            return;
        }

        var aligns = metadata.Alignments;

        if (spectreTable.Columns.Count == 0)
        {
            foreach (var justify in aligns)
            {
                var col = new TableColumn("")
                {
                    Alignment = justify.ToSpectreJustify()
                };
                spectreTable.AddColumn(col);
            }
        }
        else
        {
            // Case B: Columns already exist → update only
            for (int i = 0; i < spectreTable.Columns.Count; i++)
            {
                if (i < aligns.Count)
                {
                    // Alignment provided → apply it
                    spectreTable.Columns[i].Alignment = aligns[i].ToSpectreJustify();
                }
            }

            // Case C: More alignments than columns → append new columns
            for (int i = spectreTable.Columns.Count; i < aligns.Count; i++)
            {
                var col = new TableColumn("")
                {
                    Alignment = aligns[i].ToSpectreJustify()
                };
                spectreTable.AddColumn(col);
            }
        }
    }

    private async Task WriteTokenAsync(Paragraph liveParagraph, MarkupToken token)
    {
        if (string.IsNullOrEmpty(token.Value))
        {
            return;
        }
        Debug.WriteLine($"Writing token: `{token.Value}` of type `{token.TokenType}`");
        await MarkupWriter.Create(ansiConsole).WriteAsync(liveParagraph, token, markupStyles.TableCell);
    }
}
