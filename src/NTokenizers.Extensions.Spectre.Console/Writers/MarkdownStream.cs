using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

/// <summary>
/// One frame in the streaming prefix stack (the "left side" of a block). A frame contributes a
/// per-line segment: a border (blockquote/code) contributes the same segment on every line; a list
/// item contributes its marker on the item's first line and gutter spaces on continuation lines.
/// </summary>
internal sealed class BlockFrame
{
    internal Style Style = new();
    internal string FirstLine = string.Empty;
    internal string Continuation = string.Empty;
    internal bool FirstLineDone;
}

/// <summary>
/// A forward-only, per-token markdown line writer. It keeps a stack of <see cref="BlockFrame"/>
/// contributions (the left side of a block, composed from the active nesting levels) and writes
/// each token to the console as it arrives, breaking to a new line (rebuilding the left side from
/// the active frames) when a token no longer fits the line or the token itself contains a newline.
/// There is no buffering and no cursor repositioning, so there is no flicker and no (0,0) class of
/// bugs: output is emitted exactly once, in document order.
/// </summary>
internal sealed class MarkdownStream
{
    private readonly IAnsiConsole _console;
    private readonly List<BlockFrame> _frames = new();

    private bool _prefixed;
    private int _column;
    private int _content;

    internal MarkdownStream(IAnsiConsole console) => _console = console;

    private int? _widthCache;

    // The effective console width, cached. When output is redirected (or no console is available)
    // we fall back to a sensible default so rendering still works and wrapping stays predictable.
    private int Width
    {
        get
        {
            if (_widthCache is null)
            {
                int w;
                try
                {
                    w = System.Console.IsOutputRedirected ? 80 : System.Console.WindowWidth;
                }
                catch
                {
                    w = 80;
                }
                _widthCache = w > 0 ? w : 80;
            }
            return _widthCache.Value;
        }
    }

    // The width of the left-side prefix for the current nesting.
    private int PrefixWidth
    {
        get
        {
            int w = 0;
            foreach (var f in _frames)
            {
                w += (f.FirstLineDone ? f.Continuation : f.FirstLine).Length;
            }

            return w;
        }
    }

    // The width available for content on the current line (total width minus the left side).
    internal int AvailableWidth => Width - PrefixWidth;

    internal void PushBorder(Style style, char border)
    {
        var segment = $"{border} ";
        _frames.Add(new BlockFrame { Style = style, FirstLine = segment, Continuation = segment, FirstLineDone = false });
    }

    internal void PushItem(Style style, string marker)
    {
        _frames.Add(new BlockFrame
        {
            Style = style,
            FirstLine = marker,
            Continuation = new string(' ', marker.Length),
            FirstLineDone = false
        });
    }

    internal void Pop()
    {
        if (_frames.Count > 0)
        {
            _frames.RemoveAt(_frames.Count - 1);
        }
    }

    // The width of the left side currently composed (borders and the open item marker).
    internal int CurrentPrefixWidth => PrefixWidth;

    // Writes the composed left-side prefix for the current line if it has not been written yet.
    private void EnsurePrefixed()
    {
        if (_prefixed)
        {
            return;
        }

        foreach (var f in _frames)
        {
            var segment = f.FirstLineDone ? f.Continuation : f.FirstLine;
            if (segment.Length > 0)
            {
                _console.Write(new Markup(Markup.Escape(segment), f.Style));
            }
        }

        _column = PrefixWidth;
        _prefixed = true;
    }

    // Ends the current line: every active frame moves to its continuation (gutter) form and the
    // cursor advances to a fresh line.
    private void EndLine()
    {
        foreach (var f in _frames)
        {
            f.FirstLineDone = true;
        }

        _console.Write("\n");
        _prefixed = false;
        _column = 0;
        _content = 0;
    }

    // Ends the current line only if it already carries content or a written prefix.
    internal void EnsureNewLine()
    {
        if (_prefixed || _content > 0)
        {
            EndLine();
        }
    }

    // True when the cursor is at the start of an empty line (no left side or content written yet).
    internal bool IsLineEmpty => !_prefixed && _content == 0;

    // Emits a line that carries only the left side (a blank content line): the previous content
    // line is ended, then one left-side-only line is written. Used for the block separation and
    // for the root spacing between consecutive elements.
    internal void BlankLine()
    {
        EnsureNewLine();
        EnsurePrefixed();
        EndLine();
    }

    // Ends the final line of a block so the block's output terminates with a newline.
    internal void Finish()
    {
        if (_prefixed || _content > 0)
        {
            EndLine();
        }
    }

    internal void Write(string text, Style style)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        // Each part except the last is a terminated line (the final empty part is the artifact of
        // the text's trailing newline). A blank interior line (an empty, terminated part) still
        // carries the composed left side, so blank lines inside a bordered or guttered block
        // render the border/gutter.
        var parts = text.Split('\n');
        for (var i = 0; i < parts.Length; i++)
        {
            var part = parts[i].Replace("\r", string.Empty);
            var isTerminatedLine = i < parts.Length - 1;

            if (part.Length > 0)
            {
                EnsurePrefixed();

                // Wrap: if this segment would overflow and the line already carries content, start
                // a fresh line (rebuilding the left side) first. A segment wider than the whole
                // line simply overflows (it is not split).
                if (_content > 0 && _column + part.Length > Width)
                {
                    EndLine();
                    EnsurePrefixed();
                }

                _console.Write(new Markup(Markup.Escape(part), style));
                _column += part.Length;
                _content += part.Length;
            }
            else if (isTerminatedLine)
            {
                // A blank line within the text: carry the left side on the empty line.
                EnsurePrefixed();
            }

            if (isTerminatedLine)
            {
                EndLine();
            }
        }
    }
}
