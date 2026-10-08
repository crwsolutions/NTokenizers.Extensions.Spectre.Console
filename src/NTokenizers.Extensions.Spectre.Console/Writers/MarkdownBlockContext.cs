using NTokenizers.Core;
using NTokenizers.Markdown;
using NTokenizers.Markdown.Metadata;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

/// <summary>
/// The kind of block a <see cref="MarkdownBlockContext"/> owns. It determines the left-side frame
/// the context pushes when it opens (and pops when it completes).
/// </summary>
internal enum BlockKind
{
    /// <summary>No left-side frame (a root block, a heading, or a flat list).</summary>
    None,

    /// <summary>A blockquote: a green left border on every line of its content.</summary>
    Quote,

    /// <summary>A fenced/indented code block: a cyan left border on every line of its content.</summary>
    Code,

    /// <summary>A list item: a marker on the item's first line, gutter spaces thereafter.</summary>
    Item
}

/// <summary>
/// One rendering context per markdown nesting level. A context owns the paragraph state, the
/// pending block separation, the heading state, and (per <see cref="BlockKind"/>) the left-side
/// frame it pushes onto the shared <see cref="MarkdownStream"/>. All levels of a root block share
/// one <see cref="MarkdownStream"/>: a block pushes its frame when it opens and pops it when it
/// completes, so the composed left side always reflects the active nesting. Every token is written
/// to the console as it arrives (forward-only), so content appears as fast as the tokenizer
/// streams it.
/// </summary>
/// <remarks>
/// Sub-document blocks (blockquote, fenced/indented code, heading, list item) register a child
/// context as the metadata's inline-token handler. The child context pushes its frame in its
/// constructor, dispatches the sub-document tokens, and pops the frame on <see cref="Commit"/>.
/// The root block's stream is finished exactly once, when the outermost block completes
/// (a sub-document's commit, a flat list's <c>ListEnd</c>, or a root horizontal rule).
///
/// Lists are flat (their <c>ListStart</c>/<c>ListEnd</c> tokens are not sub-documents): the owning
/// context handles <c>ListStart</c> (marker only) and <c>ListEnd</c> inline; each item is a child
/// context that owns its marker frame.
/// </remarks>
internal sealed class MarkdownBlockContext
{
    private readonly MarkdownWriter _owner;
    private readonly MarkdownStream _stream;
    private readonly BlockKind _kind;
    private readonly string? _itemMarker;
    private readonly Style? _itemMarkerStyle;

    // The region-close signal. Non-null on the root context (region owner) and, after handoff, on
    // the outermost block's child context. Fired exactly once when the outermost block completes.
    private Action? _regionClose;

    // True only on the root context (the region owner). It may hand _regionClose to the
    // outermost sub-document block, or keep it to fire on a flat block (list / horizontal rule).
    private bool _ownsRegion;

    // Per-level paragraph / separation state.
    private bool _inParagraph;
    private bool _pendingBlockBreak;

    // True once a list item's own paragraph has closed and no list of this context is open: the
    // next ListStart is a nested list continuing the item, not a sibling block.
    private bool _awaitingNestedList;

    // Heading state (active only on the heading's own context).
    private bool _headingActive;
    private int _headingLevel;
    private Style _headingStyle = new();
    private int _headingTextLength;

    internal MarkdownBlockContext(
        MarkdownWriter owner,
        MarkdownStream stream,
        Action? regionClose = null,
        bool ownsRegion = false,
        BlockKind kind = BlockKind.None,
        string? itemMarker = null,
        Style? itemMarkerStyle = null)
    {
        _owner = owner;
        _stream = stream;
        _regionClose = regionClose;
        _ownsRegion = ownsRegion;
        _kind = kind;
        _itemMarker = itemMarker;
        _itemMarkerStyle = itemMarkerStyle;

        // Push the block's left-side frame when it opens.
        switch (kind)
        {
            case BlockKind.Quote:
                _stream.PushBorder(owner.MarkdownStyles.QuoteBorder, '│');
                break;
            case BlockKind.Code:
                _stream.PushBorder(owner.MarkdownStyles.CodeBorder, '│');
                break;
            case BlockKind.Item when itemMarker is not null:
                _stream.PushItem(itemMarkerStyle ?? new(), itemMarker);
                break;
        }
    }

    /// <summary>
    /// Dispatches a markdown token, routing block tokens to dedicated handling (registering a
    /// child context for sub-document blocks) and inline/content tokens directly to the stream.
    /// </summary>
    internal void WriteToken(MarkdownToken token)
    {
        // Write the block separation that follows a closed paragraph, before the next token. It is
        // intentionally omitted when the paragraph is the last token (end of content, the break is
        // dropped by Commit) and, in a list item, when the closed paragraph is followed by a nested
        // list: the list's start is a continuation of the item, not a sibling block. The
        // whitespace Text tokens between the item's text and the nested list's start carry no
        // content, so the pending break is deferred to the nested list's start, which ends the
        // item's line without emitting the gutter-only blank line.
        if (_pendingBlockBreak)
        {
            var type = token.TokenType;
            var deferToNestedList = _kind is BlockKind.Item &&
                type is MarkdownTokenType.Text &&
                string.IsNullOrWhiteSpace(token.Value);
            if (!deferToNestedList)
            {
                var nestedListContinuation = _awaitingNestedList && type is MarkdownTokenType.ListStart;
                _pendingBlockBreak = false;
                _awaitingNestedList = false;
                if (nestedListContinuation)
                {
                    // End the item's line (its marker frame moves to its gutter form) without
                    // emitting the gutter-only blank line: the nested list continues the item.
                    _stream.EnsureNewLine();
                }
                else
                {
                    _stream.BlankLine();
                }
            }
        }

        switch (token.TokenType)
        {
            case MarkdownTokenType.ParagraphBlockStart:
                _inParagraph = true;
                break;

            case MarkdownTokenType.ParagraphBlockEnd:
                if (_inParagraph)
                {
                    _inParagraph = false;
                    _pendingBlockBreak = true;
                    if (_kind is BlockKind.Item)
                    {
                        // A list that follows the item's own paragraph (no list of this context
                        // was open when it closed) is a nested list: a continuation of the item.
                        _awaitingNestedList = true;
                    }
                }

                break;

            case MarkdownTokenType.Text:
                // Newline/whitespace text outside a paragraph is block separation, not content:
                // it must never open an (empty) paragraph.
                if (!_inParagraph)
                {
                    if (string.IsNullOrWhiteSpace(token.Value))
                    {
                        break;
                    }

                    // A list item in a quote streams inline-only content (no ParagraphBlockStart);
                    // open a paragraph lazily so the content is tracked as paragraph content.
                    _inParagraph = true;
                }

                if (_headingActive)
                {
                    _headingTextLength += token.Value.Length;
                }

                _stream.Write(token.Value, StyleForContent());
                break;

            case MarkdownTokenType.Heading:
                WriteHeading(token);
                break;

            case MarkdownTokenType.HorizontalRule:
                _stream.Finish();
                _stream.Write(new string('─', Math.Max(1, _stream.AvailableWidth)), _owner.MarkdownStyles.HorizontalRule);
                _stream.Finish();
                CloseRegionIfOwned();
                break;

            case MarkdownTokenType.Blockquote:
                WriteBlockquote(token);
                break;

            case MarkdownTokenType.ListStart:
                // A list is flat: ListStart is a marker only; the items (sub-documents) own their
                // marker frames. A list following the item's own paragraph is a continuation; any
                // other list (e.g. after a nested list ended) is a sibling block and keeps its
                // blank-line separation, flushed above.
                _awaitingNestedList = false;
                break;

            case MarkdownTokenType.ListEnd:
                _pendingBlockBreak = true;
                // A flat list completes at ListEnd: close the region when the list is the outermost.
                CloseRegionIfOwned();
                break;

            case MarkdownTokenType.UnorderedListItem:
                WriteListItem(token, ordered: false);
                break;

            case MarkdownTokenType.OrderedListItem:
                WriteListItem(token, ordered: true);
                break;

            case MarkdownTokenType.CodeBlock:
                WriteFencedCodeBlock(token);
                break;

            case MarkdownTokenType.IndentedCodeBlock:
                WriteIndentedCodeBlock(token);
                break;

            case MarkdownTokenType.Table:
                WriteTable(token);
                break;

            default:
                // Inline tokens (bold, italic, link, ...) inside a paragraph/heading: the value is
                // written directly to the stream with the token's style.
                _stream.Write(token.Value, _owner.MarkdownStyles.GetStyleForToken(token.TokenType));
                break;
        }
    }

    /// <summary>
    /// Commits the context when its container's inline content is complete: finalizes any heading,
    /// pops this context's left-side frame, and drops the pending block separation (omitted at end
    /// of content). Mirrors <c>BlockquoteHtmlWriter</c>'s <c>onInlinesCompleted</c> callback.
    /// </summary>
    internal void Commit()
    {
        if (_headingActive)
        {
            FinalizeHeading();
        }

        // End the last line, then pop this context's left-side frame (if any).
        if (_kind is BlockKind.Quote or BlockKind.Code or BlockKind.Item)
        {
            _stream.Finish();
            _stream.Pop();
        }

        _pendingBlockBreak = false;
        // A sub-document block completes on commit: close the region when this is the outermost.
        _regionClose?.Invoke();
        _regionClose = null;
    }

    // Closes the region when this context owns it as the region owner handling a flat block
    // (list end / horizontal rule). The outermost sub-document instead closes via Commit.
    private void CloseRegionIfOwned()
    {
        if (_ownsRegion)
        {
            _regionClose?.Invoke();
            _regionClose = null;
        }
    }

    // The style for plain Text content: the heading style while a heading is active, the code
    // style in a code block, otherwise the default style.
    private Style StyleForContent()
    {
        if (_headingActive)
        {
            return _headingStyle;
        }

        if (_kind == BlockKind.Code)
        {
            return _owner.MarkdownStyles.CodeBlock;
        }

        return _owner.MarkdownStyles.DefaultStyle;
    }

    // A heading is a sub-document: the heading text streams through the registered handler. The
    // heading renders directly in the stream; its underline (and level-1 bold decoration) is
    // written on commit, when the full text length is known.
    private void WriteHeading(MarkdownToken token)
    {
        if (token.Metadata is not HeadingMetadata meta)
        {
            return;
        }

        var styles = _owner.MarkdownStyles.MarkdownHeadingStyles;
        var headingStyle = meta.Level switch
        {
            1 => styles.Level1,
            >= 2 and <= 4 => styles.Level2To4,
            _ => styles.Level5AndAbove
        };

        var context = new MarkdownBlockContext(_owner, _stream, TakeRegionClose());
        context.InitializeHeading(meta.Level, headingStyle);

        meta.RegisterInlineTokenHandler(
            sub => context.WriteToken(sub),
            () => context.Commit());
    }

    // Internal initializer so the heading state (private fields) can be set by the owning context
    // that just created the heading's child context. The level-1 bold prefix is written now,
    // before the heading text streams.
    internal void InitializeHeading(int level, Style style)
    {
        _headingActive = true;
        _headingLevel = level;
        _headingStyle = style;
        _inParagraph = true;

        if (level == 1)
        {
            _stream.Write("** ", style);
        }
    }

    private void FinalizeHeading()
    {
        _headingActive = false;

        if (_headingLevel == 1)
        {
            _stream.Write(" **", _headingStyle);
        }

        _stream.Finish();

        // Level 1 is underlined with '=' (spans the text plus the "** " decoration); level 2-4
        // with '-'; level 5+ have no underline (mirrors MarkdownHeadingWriter).
        if (_headingLevel == 1)
        {
            _stream.Write(new string('=', _headingTextLength + 6), _headingStyle);
        }
        else if (_headingLevel is >= 2 and <= 4)
        {
            _stream.Write(new string('-', _headingTextLength), _headingStyle);
        }

        _stream.Finish();
    }

    // A blockquote is a sub-document: the quoted content is a full markdown sub-stream. The
    // child context (kind Quote) pushes a border frame, so every line of its content (and its
    // nested blocks) is bordered.
    private void WriteBlockquote(MarkdownToken token)
    {
        if (token.Metadata is not BlockquoteMetadata meta)
        {
            return;
        }

        var context = new MarkdownBlockContext(_owner, _stream, TakeRegionClose(), kind: BlockKind.Quote);
        _pendingBlockBreak = true;

        meta.RegisterInlineTokenHandler(
            sub => context.WriteToken(sub),
            () => context.Commit());
    }

    // A list item is a sub-document: the item's content streams through the registered handler.
    // The child context (kind Item) owns the marker frame (pushed in its constructor, popped on
    // commit), so every line of the item's content (and its nested blocks) is indented to the
    // marker.
    private void WriteListItem(MarkdownToken token, bool ordered)
    {
        string? marker = null;
        Style? markerStyle = null;
        if (ordered && token.Metadata is OrderedListItemMetadata orderedMeta)
        {
            marker = $" {orderedMeta.Number.ToString().PadLeft(2)}{orderedMeta.Marker} ";
            markerStyle = _owner.MarkdownStyles.OrderedListItem;
        }
        else if (!ordered && token.Metadata is ListItemMetadata unorderedMeta)
        {
            marker = $" {unorderedMeta.Marker} ";
            markerStyle = _owner.MarkdownStyles.UnorderedListItem;
        }

        if (marker is null)
        {
            return;
        }

        var context = new MarkdownBlockContext(
            _owner, _stream, kind: BlockKind.Item, itemMarker: marker, itemMarkerStyle: markerStyle);

        (token.Metadata as InlineMetadata<MarkdownToken>)!.RegisterInlineTokenHandler(
            sub => context.WriteToken(sub),
            () => context.Commit());
    }

    // A fenced code block is a sub-document: the code streams through the registered handler. The
    // language label is written (outside the border) before the child context (kind Code) pushes
    // the code border frame, so the code content is bordered.
    private void WriteFencedCodeBlock(MarkdownToken token)
    {
        if (token.Metadata is not ICodeBlockMetadata meta)
        {
            return;
        }

        var language = string.IsNullOrWhiteSpace(meta.Language) ? "code" : meta.Language;

        _stream.Write($"{language}:", _owner.MarkdownStyles.CodeLabel);
        _stream.Finish();

        var context = new MarkdownBlockContext(_owner, _stream, TakeRegionClose(), kind: BlockKind.Code);
        _owner.WireFencedCode(meta, _stream, context);
    }

    // An indented code block is a sub-document: its (plain text) content streams through the
    // registered handler. It renders like a fenced code block with the label "code:".
    private void WriteIndentedCodeBlock(MarkdownToken token)
    {
        if (token.Metadata is not IndentedCodeBlockMetadata meta)
        {
            return;
        }

        _stream.Write("code:", _owner.MarkdownStyles.CodeLabel);
        _stream.Finish();

        var context = new MarkdownBlockContext(_owner, _stream, TakeRegionClose(), kind: BlockKind.Code);

        meta.RegisterInlineTokenHandler(
            sub => context.WriteToken(sub),
            () => context.Commit());
    }

    // A table renders in its own live region that grows as the tokenizer streams the table
    // content. The callback is awaited by the parser before the content streams, so it must
    // return immediately; the live region is kicked off as a fire-and-forget task and closes
    // itself when the content completes.
    private void WriteTable(MarkdownToken token)
    {
        if (token.Metadata is not TableMetadata meta)
        {
            return;
        }

        _pendingBlockBreak = true;
        var writer = new MarkdownTableWriter(_owner.Console, _owner.MarkdownStyles);
        _ = Task.Run(() => writer.WriteAsync(meta));
    }

    // The region owner hands the region-close to the first sub-document block it dispatches (the
    // outermost block) and keeps none for itself. Nested contexts have no signal to hand, so a
    // nested block can never close the region.
    private Action? TakeRegionClose()
    {
        if (!_ownsRegion)
        {
            return null;
        }

        _ownsRegion = false;
        var close = _regionClose;
        _regionClose = null;
        return close;
    }
}
