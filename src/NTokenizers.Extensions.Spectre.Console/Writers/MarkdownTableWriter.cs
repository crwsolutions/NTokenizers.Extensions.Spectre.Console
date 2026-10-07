using NTokenizers.Markdown;
using NTokenizers.Markdown.Metadata;
using NTokenizers.Extensions.Spectre.Console.Extensions;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

/// <summary>
/// Builds a Spectre <see cref="Table"/> from a <see cref="TableMetadata"/> sub-document,
/// forward-only: the inline-token handler is registered (and returns immediately, so the
/// tokenizer — which gates on token dispatch — never waits on a blocked writer) and the
/// finished table is handed back to the caller's <see cref="WriteTo"/> completion callback
/// exactly when the table content is complete.
/// </summary>
internal class MarkdownTableWriter(MarkdownStyles markdownStyles)
{
    /// <summary>
    /// Registers the table's inline-token handler on the given metadata. The handler fills the
    /// given <paramref name="table"/>; <paramref name="onCompleted"/> is invoked (by the parser)
    /// when the table content is complete.
    /// </summary>
    /// <param name="table">The table to fill with the parsed rows and cells.</param>
    /// <param name="metadata">The table metadata whose inline tokens stream the content.</param>
    /// <param name="onCompleted">Callback invoked when the table content is complete.</param>
    internal void WriteTo(Table table, TableMetadata metadata, Action onCompleted)
    {
        var column = -1;
        var cellParagraphs = new List<Paragraph>();
        Paragraph? currentCell = null;

        _ = metadata.RegisterInlineTokenHandler(inlineToken =>
        {
            switch (inlineToken.TokenType)
            {
                case MarkdownTokenType.TableAlignments:
                    HandleAlignments(table, metadata);
                    break;

                case MarkdownTokenType.TableRow:
                    // Handle new row
                    column = -1;
                    currentCell = null;

                    if (table.Columns.Count > 0)
                    {
                        cellParagraphs = Enumerable.Range(0, table.Columns.Count).Select(_ => new Paragraph()).ToList();
                        table.AddRow(new TableRow(cellParagraphs));
                    }

                    break;

                case MarkdownTokenType.TableCell:
                    column++;
                    if (table.Rows.Count == 0)
                    {
                        // Header row: one column per cell.
                        currentCell = new Paragraph();
                        table.AddColumn(new TableColumn(currentCell));
                    }
                    else
                    {
                        if (column < cellParagraphs.Count)
                        {
                            currentCell = cellParagraphs[column];
                        }
                    }

                    break;

                default:
                    // Cell content.
                    if (currentCell is not null && !string.IsNullOrEmpty(inlineToken.Value))
                    {
                        currentCell.Append(inlineToken.Value, markdownStyles.GetStyleForToken(inlineToken.TokenType));
                    }

                    break;
            }
        }, onCompleted);
    }

    private static void HandleAlignments(Table spectreTable, TableMetadata metadata)
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
            for (var i = 0; i < spectreTable.Columns.Count; i++)
            {
                if (i < aligns.Count)
                {
                    // Alignment provided → apply it
                    spectreTable.Columns[i].Alignment = aligns[i].ToSpectreJustify();
                }
            }

            // Case C: More alignments than columns → append new columns
            for (var i = spectreTable.Columns.Count; i < aligns.Count; i++)
            {
                var col = new TableColumn("")
                {
                    Alignment = aligns[i].ToSpectreJustify()
                };
                spectreTable.AddColumn(col);
            }
        }
    }
}
