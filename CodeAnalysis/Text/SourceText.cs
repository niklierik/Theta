namespace Theta.CodeAnalysis.Text;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class SourceText
{
    private readonly string _text;

    public List<TextLine> Lines { get; } = new();

    private SourceText(string text)
    {
        _text = text;
        ParseLines();
    }

    public int GetLineIndex(int position)
    {
        var lower = 0;
        var upper = Lines.Count - 1;
        while (lower <= upper)
        {
            var index = lower + (upper - lower) / 2;
            var start = Lines[index].Start;
            var span = Lines[index].SpanIncludingLineBreak;
            if (span.In(position))
            {
                return index;
            }
            if (position < start)
            {
                upper = lower - 1;
            }
            else
            {
                lower = upper + 1;
            }
        }
        return -1;
    }

    public char this[int index]
    {
        get
        {
            if (index < 0 || index >= _text.Length)
            {
                return '\0';
            }
            return _text[index];
        }
    }

    public int Length => _text.Length;
    public int Count => _text.Length;

    public string Substring(int start, int length)
    {
        return ToString(start, length);
    }

    private void ParseLines()
    {
        var position = 0;
        var lineStart = 0;
        while (position < _text.Length)
        {
            var lineBreakWidth = GetLineBreakWidth(position);
            if (lineBreakWidth == 0)
            {
                position++;
                continue;
            }
            ParseLine(position, lineStart, lineBreakWidth);
            position += lineBreakWidth;
            lineStart = position;

        }
        if (position > lineStart)
        {
            ParseLine(position, lineStart, 0);
        }
    }

    private void ParseLine(int position, int lineStart, int lineBreakWidth)
    {
        var lineLength = position - lineStart;
        var line = new TextLine(this, lineStart, lineLength, lineLength + lineBreakWidth);
        Lines.Add(line);
    }

    private int GetLineBreakWidth(int position)
    {
        var c = this[position];
        var l = this[position + 1];
        if (c == '\r' && l == '\n')
        {
            return 2;
        }
        if (c == '\r' || c == '\n')
        {
            return 1;
        }
        return 0;
    }

    public static SourceText From(string text)
    {
        return new SourceText(text);
    }

    public override string ToString()
    {
        return _text;
    }

    public string ToString(int start, int length) => _text.Substring(start, length);
    public string ToString(TextSpan span) => ToString(span.Start, span.Length);

    public string ToLower()
    {
        return _text.ToLower();
    }
}

public sealed class TextLine
{
    public SourceText Text { get; }
    public int Start { get; }
    public int Length { get; }
    public int LengthIncludingLineBreak { get; }

    public TextSpan Span => new(Start, Length);
    public TextSpan SpanIncludingLineBreak => new(Start, LengthIncludingLineBreak);

    public int End => Start + Length;

    public TextLine(SourceText text, int start, int length, int lengthIncludingLineBreak)
    {
        Text = text;
        Start = start;
        Length = length;
        LengthIncludingLineBreak = lengthIncludingLineBreak;
    }


    public override string ToString()
    {
        return Text.ToString(Span);
    }

    public string ToString(int start, int length) => ToString().Substring(start, length);
    public string ToString(TextSpan span) => ToString(span.Start, span.Length);

}