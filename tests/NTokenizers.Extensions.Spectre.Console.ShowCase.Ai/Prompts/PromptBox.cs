using Spectre.Console;
using Spectre.Console.Rendering;
using TextCopy;
using SystemConsole = System.Console;

namespace NTokenizers.Extensions.Spectre.Console.ShowCase.Ai.Prompts;

public sealed class PromptBox : IPrompt<string>
{
    /// <summary>
    /// Gets or sets the style for the prompt text.
    /// </summary>
    public Style PromptStyle { get; set; } = Color.Green;

    /// <summary>
    /// Gets or sets the style for the regular text.
    /// </summary>
    public Style TextStyle { get; set; } = Style.Plain;

    /// <summary>
    /// Gets or sets the style for the wrap glyph.
    /// </summary>
    public Style WrapGlyphStyle { get; set; } = Color.Yellow;

    /// <summary>
    /// Gets or sets the box border style for the panel.
    /// </summary>
    public BoxBorder BoxBorder { get; set; } = BoxBorder.Rounded;

    /// <summary>
    /// Gets or sets the border style for the panel.
    /// </summary>
    public Style BorderStyle { get; set; } = Color.Green1;

    /// <summary>
    /// Gets or sets the placeholder text to display when the input is empty.
    /// </summary>
    public string Placeholder { get; set; } = "";

    /// <summary>
    /// Gets or sets the style for the placeholder text.
    /// </summary>
    public Style PlaceholderStyle { get; set; } = new Color(119, 119, 119);

    private const string WRAP_STRING = " ↩ ";
    private const string PROMPT = "> ";
    private readonly List<string> _lines = [""];
    private int _currentLine = 0;
    private readonly int startLeft = SystemConsole.CursorLeft;
    private int startTop = SystemConsole.CursorTop;

    public string Show(IAnsiConsole console) => ShowAsync(console, CancellationToken.None).GetAwaiter().GetResult();

    public async Task<string> ShowAsync(IAnsiConsole console, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(console);

        return await console.RunExclusive<Task<string>>(async () =>
        {
            return Read(console);
        });
    }

    private string Read(IAnsiConsole console)
    {
        int cursorPos = 0;
        while (true)
        {
            // Redraw
            Redraw(cursorPos);

            // Key handling
            if (console.Input.ReadKey(true) is ConsoleKeyInfo keyInfo)
            {
                var (newCursorPos, inputIsCompleted) = keyInfo.Key switch
                {
                    ConsoleKey.Enter when (keyInfo.Modifiers & ConsoleModifiers.Shift) != 0 => HandleSoftEnter(cursorPos), // Insert line break without completing input
                    ConsoleKey.Enter => (cursorPos, true), // Complete input
                    ConsoleKey.Backspace when cursorPos > 0 => RemoveCharacter(cursorPos), // Remove character before cursor
                    ConsoleKey.Backspace when _currentLine > 0 => RemoveLine(cursorPos), // Merge with previous line
                    ConsoleKey.Delete when cursorPos < _lines[_currentLine].Length => DeleteCharacter(cursorPos), // Delete character at cursor
                    ConsoleKey.Delete when _currentLine < _lines.Count - 1 => MergeNextLine(cursorPos), // Merge with next line
                    ConsoleKey.LeftArrow when cursorPos > 0 => (cursorPos - 1, false), // Move cursor left
                    ConsoleKey.LeftArrow when _currentLine > 0 => (_lines[--_currentLine].Length, false), // Move to end of previous line
                    ConsoleKey.RightArrow when cursorPos < _lines[_currentLine].Length => (cursorPos + 1, false), // Move cursor right
                    ConsoleKey.RightArrow when _currentLine < _lines.Count - 1 => MoveToNextLineStart(cursorPos), // Move to start of next line
                    ConsoleKey.UpArrow when _currentLine > 0 => (Math.Min(cursorPos, _lines[--_currentLine].Length), false), // Move up preserving column
                    ConsoleKey.DownArrow when _currentLine < _lines.Count - 1 => (Math.Min(cursorPos, _lines[++_currentLine].Length), false), // Move down preserving column
                    ConsoleKey.Home => (0, false), // Move to start of line
                    ConsoleKey.End => (_lines[_currentLine].Length, false), // Move to end of line
                    ConsoleKey.B when (keyInfo.Modifiers & ConsoleModifiers.Control) != 0 => Paste(cursorPos), // Paste from clipboard
                    _ when !char.IsControl(keyInfo.KeyChar) => InsertCharacter(cursorPos, keyInfo.KeyChar), // Insert printable character
                    _ => (cursorPos, false) // No-op
                };

                cursorPos = newCursorPos;

                if (inputIsCompleted)
                {
                    break;
                }
            }
        }

        var visualLines = Redraw(cursorPos, false);
        var boxEnd = startTop + visualLines + 2;
        if (boxEnd < SystemConsole.BufferHeight)
        {
            SystemConsole.SetCursorPosition(0, boxEnd);
        }
        return string.Join(Environment.NewLine, _lines);
    }

    private (int CursorPos, bool IsComplete) RemoveLine(int cursorPos)
    {
        int newCursorPos = _lines[_currentLine - 1].Length;
        _lines[_currentLine - 1] += _lines[_currentLine];
        _lines.RemoveAt(_currentLine);
        _currentLine--;
        return (newCursorPos, false);
    }

    private (int CursorPos, bool IsComplete) RemoveCharacter(int cursorPos)
    {
        _lines[_currentLine] = _lines[_currentLine].Remove(cursorPos - 1, 1);
        return (cursorPos - 1, false);
    }

    private (int CursorPos, bool IsComplete) HandleSoftEnter(int cursorPos)
    {
        string current = _lines[_currentLine];
        string left = current[..cursorPos];
        string right = current[cursorPos..];

        _lines[_currentLine] = left;
        _lines.Insert(_currentLine + 1, right);
        _currentLine++;
        return (0, false);
    }

    private (int CursorPos, bool IsComplete) DeleteCharacter(int cursorPos)
    {
        _lines[_currentLine] = _lines[_currentLine].Remove(cursorPos, 1);
        return (cursorPos, false);
    }

    private (int CursorPos, bool IsComplete) MergeNextLine(int cursorPos)
    {
        _lines[_currentLine] += _lines[_currentLine + 1];
        _lines.RemoveAt(_currentLine + 1);
        return (cursorPos, false);
    }

    private (int CursorPos, bool IsComplete) MoveToNextLineStart(int cursorPos)
    {
        _currentLine++;
        return (0, false);
    }

    private (int CursorPos, bool IsComplete) InsertCharacter(int cursorPos, char c)
    {
        _lines[_currentLine] = _lines[_currentLine].Insert(cursorPos, c.ToString());
        return (cursorPos + 1, false);
    }

    private (int CursorPos, bool IsComplete) Paste(int cursorPos)
    {
        string pasted = ClipboardService.GetText() ?? "";
        pasted = pasted.Replace("\r\n", "\n").Replace("\r", "\n");

        var pastedLines = pasted.Split('\n');

        string current = _lines[_currentLine];
        string left = current[..cursorPos];
        string right = current[cursorPos..];

        _lines[_currentLine] = left + pastedLines[0];

        for (int i = 1; i < pastedLines.Length; i++)
        {
            _lines.Insert(_currentLine + i, pastedLines[i]);
        }

        _currentLine += pastedLines.Length - 1;
        _lines[_currentLine] += right;

        return (pastedLines[^1].Length, false);
    }

    private int Redraw(int cursorPos, bool hasPrompt = true)
    {
        var boxWidth = SystemConsole.WindowWidth;
        var prefixWidth = hasPrompt ? 4 : 2;
        var contentWidth = SystemConsole.WindowWidth - startLeft - prefixWidth - 2;

        // Build visual lines
        var visualLines = BuildVisualLines(_lines, contentWidth);

        // Clear previous box area
        var endBox = startTop + visualLines.Count + 2;
        if (endBox < SystemConsole.BufferHeight)
        {
            SystemConsole.SetCursorPosition(0, endBox);
            SystemConsole.Write(new string(' ', boxWidth));
        }

        // Draw top border
        if (startTop > 0)
        {
            SystemConsole.SetCursorPosition(startLeft, startTop);
        }

        var renderables = new List<IRenderable>();
        var overflow = endBox + 1 - SystemConsole.BufferHeight;
        if (overflow > 0)
        {
            startTop -= overflow;
        }

        if (_lines.Count == 1 && _lines[0].Length == 0 && hasPrompt)
        {
            var text = new Paragraph(PROMPT, PromptStyle);
            text.Append(Placeholder, PlaceholderStyle);
            renderables.Add(text);
        }
        else
        {
            // Draw content
            for (int i = 0; i < visualLines.Count; i++)
            {
                var v = visualLines[i];

                var start = hasPrompt ? i == 0 ? PROMPT : "  " : "";
                var text = new Paragraph(start, PromptStyle);

                // Show regular content
                text.Append(v.Text, TextStyle);
                if (v.IsWrapped)
                {
                    text.Append(WRAP_STRING, WrapGlyphStyle);
                }

                renderables.Add(text);
            }
        }

        var rows = new Rows(renderables);

        // Put the rows inside a panel
        var panel = new Panel(rows)
        {
            Border = BoxBorder,
            BorderStyle = BorderStyle,
            Expand = true
        };

        AnsiConsole.Write(panel);

        // Map cursor to visual position
        int vLine = 0, vOffset = 0;
        for (int i = 0; i < visualLines.Count; i++)
        {
            var v = visualLines[i];
            if (v.LogicalLine == _currentLine &&
                cursorPos >= v.StartIndex &&
                cursorPos <= v.StartIndex + v.Text.Length)
            {
                vLine = i;
                vOffset = cursorPos - v.StartIndex;
                if (v.IsWrapped && vOffset > v.Text.Length - WRAP_STRING.Length)
                {
                    vOffset = v.Text.Length - WRAP_STRING.Length; // beperk cursor
                }

                break;
            }
        }
        var hPos = Math.Min(startTop + 1 + vLine, SystemConsole.BufferHeight - 3);
        if (hPos < SystemConsole.BufferHeight)
        {
            SystemConsole.SetCursorPosition(startLeft + prefixWidth + vOffset, hPos);
        }

        return visualLines.Count;
    }

    private List<VisualLine> BuildVisualLines(List<string> lines, int maxWidth)
    {
        var result = new List<VisualLine>();

        for (int i = 0; i < lines.Count; i++)
        {
            string line = lines[i];
            int index = 0;

            if (line.Length == 0)
            {
                result.Add(new VisualLine { LogicalLine = i, StartIndex = 0, Text = " " });
                continue;
            }

            while (index < line.Length)
            {
                int remaining = line.Length - index;
                bool wrapped = remaining > (maxWidth - WRAP_STRING.Length);

                int take = wrapped ? Math.Max(0, maxWidth - WRAP_STRING.Length) : Math.Min(maxWidth, remaining);
                string part = line.Substring(index, take);

                result.Add(new VisualLine { LogicalLine = i, StartIndex = index, Text = part, IsWrapped = wrapped });
                index += take;
            }
        }

        return result;
    }

    private sealed record class VisualLine
    {
        public int LogicalLine { get; init; }
        public int StartIndex { get; init; }
        public string Text { get; init; } = "";
        public bool IsWrapped { get; init; } = false;
    }
}