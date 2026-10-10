using NTokenizers.Markdown;
using NTokenizers.Markdown.Metadata;
using NTokenizers.Extensions.Spectre.Console.Extensions;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

/// <summary>
/// Renders a markdown table in a live region that grows as the tokenizer streams the table's
/// content: every token is applied to the table and the region is refreshed, so the table
/// restructures in place as new rows and cells arrive. The caller must not wait for
/// <see cref="WriteAsync"/> to complete from the token callback (the tokenizer only streams the
/// table content after the callback returns); the region closes itself when the content completes.
/// </summary>
internal class MarkdownTableWriter(IAnsiConsole ansiConsole, MarkdownStyles markdownStyles)
{
    internal async Task WriteAsync(TableMetadata metadata)
    {
        var spectreTable = new Table();

        var column = -1;
        var cellParagraphs = new List<Paragraph>();
        Paragraph liveParagraph = new();

        await ansiConsole.Live(spectreTable)
        .StartAsync(async ctx =>
        {
            await metadata.RegisterInlineTokenHandler(inlineToken =>
            {
                if (inlineToken.TokenType == MarkdownTokenType.TableAlignments)
                {
                    HandleAlignments(spectreTable, metadata);
                }
                else if (inlineToken.TokenType == MarkdownTokenType.TableRow)
                {
                    // Handle new row
                    column = -1;

                    if (spectreTable.Columns.Count > 0)
                    {
                        cellParagraphs = Enumerable.Range(0, spectreTable.Columns.Count).Select(_ => new Paragraph()).ToList();
                        spectreTable.AddRow(new TableRow(cellParagraphs));
                    }
                }
                else if (inlineToken.TokenType == MarkdownTokenType.TableCell)
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
                else // Write cell content
                {
                    WriteCell(liveParagraph, inlineToken);
                }

                ctx.Refresh();
            });

            ctx.Refresh();
        });
    }

    private void WriteCell(Paragraph liveParagraph, MarkdownToken token)
    {
        if (string.IsNullOrEmpty(token.Value))
        {
            return;
        }

        var style = token.TokenType switch
        {
            MarkdownTokenType.Bold => markdownStyles.Bold,
            MarkdownTokenType.Italic => markdownStyles.Italic,
            MarkdownTokenType.CodeInline => markdownStyles.CodeInline,
            MarkdownTokenType.Link => markdownStyles.Link,
            _ => markdownStyles.TableCell
        };

        liveParagraph.Append(Markup.Escape(token.Value), style);
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
            // Case B: Columns already exist -> update only
            for (var i = 0; i < spectreTable.Columns.Count; i++)
            {
                if (i < aligns.Count)
                {
                    // Alignment provided -> apply it
                    spectreTable.Columns[i].Alignment = aligns[i].ToSpectreJustify();
                }
            }

            // Case C: More alignments than columns -> append new columns
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
