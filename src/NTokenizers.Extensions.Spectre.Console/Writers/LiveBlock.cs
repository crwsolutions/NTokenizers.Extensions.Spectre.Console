using Spectre.Console;
using Spectre.Console.Rendering;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

/// <summary>
/// The kind of visual decoration a <see cref="LiveBlock"/> applies to its rows.
/// </summary>
internal enum LiveBlockKind
{
    /// <summary>No decoration: rows are rendered as-is (headings, HR, code labels).</summary>
    Bare,

    /// <summary>A left border in the quote color plus one space of padding (blockquote).</summary>
    Quote,

    /// <summary>A left border in the code color plus one space of padding (fenced/indented code).</summary>
    Code,

    /// <summary>No box, but a left gutter: marker rows carry a prefix and continuation rows are
    /// padded to the widest prefix (list).</summary>
    List
}

/// <summary>
/// A growable <see cref="IRenderable"/> that renders a markdown block as a list of rows, each
/// row a set of styled content segments plus an optional list prefix. It applies one level of
/// decoration per instance (<see cref="LiveBlockKind"/>), and nests to any depth by containing
/// other <see cref="LiveBlock"/> instances as rows. The width budget (border + padding + gutter)
/// is derived from the render width on every measure/render, so an unbounded nesting
/// depth stays within the terminal width.
/// </summary>
/// <remarks>
/// Rows are mutated in place (paragraphs grow, rows are appended) and the owning
/// <c>LiveDisplayContext</c> is refreshed; the block is re-rendered on each refresh.
/// </remarks>
internal sealed class LiveBlock : IRenderable
{
    private sealed class BlockRow
    {
        internal IRenderable? Content;

        /// <summary>The list marker prefix (null for non-marker rows).</summary>
        internal string? Prefix;
    }

    private readonly List<BlockRow> _rows = new();

    private LiveBlock(LiveBlockKind kind, Style borderStyle, Style markerStyle, Style gutterStyle)
    {
        Kind = kind;
        BorderStyle = borderStyle;
        MarkerStyle = markerStyle;
        GutterStyle = gutterStyle;
    }

    /// <summary>Creates a plain (un-decorated) block.</summary>
    internal static LiveBlock CreateBare() => new(LiveBlockKind.Bare, new Style(), new Style(), new Style());

    /// <summary>Creates a blockquote block (left border in the given style).</summary>
    internal static LiveBlock CreateQuote(Style borderStyle) => new(LiveBlockKind.Quote, borderStyle, new Style(), new Style());

    /// <summary>Creates a code block (left border in the given style).</summary>
    internal static LiveBlock CreateCode(Style borderStyle) => new(LiveBlockKind.Code, borderStyle, new Style(), new Style());

    /// <summary>Creates a list block (gutter, no box).</summary>
    internal static LiveBlock CreateList(Style markerStyle, Style gutterStyle) => new(LiveBlockKind.List, new Style(), markerStyle, gutterStyle);

    /// <summary>Gets the decoration kind of this block.</summary>
    internal LiveBlockKind Kind { get; }

    /// <summary>Gets the quote/code border style.</summary>
    internal Style BorderStyle { get; }

    /// <summary>Gets the list marker style.</summary>
    internal Style MarkerStyle { get; }

    /// <summary>Gets the list gutter (padding space) style.</summary>
    internal Style GutterStyle { get; }

    /// <summary>
    /// Appends a content row (an <see cref="IRenderable"/> such as a <see cref="Paragraph"/> or
    /// a nested <see cref="LiveBlock"/>). Used for paragraphs, headings, HR, labels, and nested
    /// sub-blocks.
    /// </summary>
    internal void AddRow(IRenderable content) => _rows.Add(new BlockRow { Content = content });

    /// <summary>
    /// Appends a styled text row (e.g. a code language label, a heading underline).
    /// </summary>
    internal void AddText(string value, Style style) => _rows.Add(new BlockRow { Content = new Text(value, style) });

    /// <summary>
    /// Appends a list marker row: the marker prefix (styled) followed by the row's content.
    /// The prefix is padded to the block's gutter (widest prefix) at render time; continuation
    /// lines of the content are padded with the gutter (spaces only).
    /// </summary>
    internal void AddItemRow(string prefix, IRenderable content) => _rows.Add(new BlockRow { Content = content, Prefix = prefix });

    private int DecorWidth => Kind switch
    {
        LiveBlockKind.Quote or LiveBlockKind.Code => 2, // border char + one padding space
        _ => 0
    };

    /// <summary>
    /// Measures the block. <see cref="Measurement.Min"/> is the smallest width that avoids
    /// wrapping; <see cref="Measurement.Max"/> carries the rendered row count for the given width
    /// so the live region can budget vertical space.
    /// </summary>
    public Measurement Measure(RenderOptions options, int maxWidth)
    {
        int min = 0;
        foreach (var row in _rows)
        {
            if (row.Content is not null)
            {
                int w = row.Content.Measure(options, int.MaxValue).Min;
                if (w > min)
                {
                    min = w;
                }
            }

            if (row.Prefix is not null && row.Prefix.Length > min)
            {
                min = row.Prefix.Length;
            }
        }

        return new Measurement(min + DecorWidth, WrappedRowCount(options, maxWidth));
    }

    private int WrappedRowCount(RenderOptions options, int maxWidth)
    {
        int contentWidth = maxWidth - DecorWidth;
        if (contentWidth < 1)
        {
            contentWidth = 1;
        }

        int total = 0;
        foreach (var row in _rows)
        {
            if (row.Content is null)
            {
                total++;
                continue;
            }

            var lines = Segment.SplitLines(row.Content.Render(InnerOptions(options, contentWidth), contentWidth), contentWidth);
            total += lines.Count == 0 ? 1 : lines.Count;
        }

        return total;
    }

    /// <summary>
    /// Renders the block into a flat segment sequence, applying the per-kind decoration (border,
    /// gutter) and wrapping each content row to the width budget.
    /// </summary>
    public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        if (_rows.Count == 0)
        {
            yield break;
        }

        int gutter = 0;
        if (Kind == LiveBlockKind.List)
        {
            foreach (var row in _rows)
            {
                if (row.Prefix is not null && row.Prefix.Length > gutter)
                {
                    gutter = row.Prefix.Length;
                }
            }
        }

        int contentWidth = maxWidth - DecorWidth;
        if (contentWidth < 1)
        {
            contentWidth = 1;
        }

        var border = new Segment("│", BorderStyle);
        var borderPad = new Segment(" ", new Style());

        foreach (var row in _rows)
        {
            var firstPrefix = BuildPrefix(row, firstLine: true, gutter, border, borderPad);
            var continuationPrefix = BuildPrefix(row, firstLine: false, gutter, border, borderPad);

            var lines = row.Content is null
                ? new List<SegmentLine> { new() }
                : Segment.SplitLines(row.Content.Render(InnerOptions(options, contentWidth), contentWidth), contentWidth);
            if (lines.Count == 0)
            {
                lines = new List<SegmentLine> { new() };
            }

            // Drop a trailing empty line (a trailing line break with no content after it).
            if (lines.Count > 1 && lines[lines.Count - 1].Count == 0)
            {
                lines.RemoveAt(lines.Count - 1);
            }

            for (int i = 0; i < lines.Count; i++)
            {
                var prefix = i == 0 ? firstPrefix : continuationPrefix;
                foreach (var segment in prefix)
                {
                    yield return segment;
                }

                foreach (var segment in lines[i])
                {
                    yield return segment;
                }

                yield return Segment.LineBreak;
            }
        }
    }

    private List<Segment> BuildPrefix(BlockRow row, bool firstLine, int gutter, Segment border, Segment borderPad)
    {
        var prefix = new List<Segment>();
        if (Kind is LiveBlockKind.Quote or LiveBlockKind.Code)
        {
            prefix.Add(border);
            prefix.Add(borderPad);
        }
        else if (Kind == LiveBlockKind.List)
        {
            if (row.Prefix is not null && firstLine)
            {
                prefix.Add(new Segment(row.Prefix, MarkerStyle));
                int pad = gutter - row.Prefix.Length;
                if (pad > 0)
                {
                    prefix.Add(new Segment(new string(' ', pad), GutterStyle));
                }
            }
            else if (gutter > 0)
            {
                prefix.Add(new Segment(new string(' ', gutter), GutterStyle));
            }
        }

        return prefix;
    }

    private RenderOptions InnerOptions(RenderOptions options, int width) => new(options.Capabilities, new Size(width, 0));
}
