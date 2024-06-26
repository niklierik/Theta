﻿namespace Theta.Language.Text;

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
        return Lines.FindIndex(line => line.SpanIncludingLineBreak.In(position));
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

    public bool IsEmpty => string.IsNullOrWhiteSpace(_text);

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
        if (position >= lineStart)
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

    public static SourceText FromText(string text)
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

    public static SourceText FromConsole(bool multiline = true)
    {
        Console.ResetColor();
        if (!multiline)
        {
            return SourceText.FromText(Console.ReadLine() ?? "");
        }
        string input = "";
        string? line;
        while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
        {
            input += line + Environment.NewLine;
            " | ".Log(ConsoleColor.DarkGray, false);
            Console.ResetColor();
        }
        return SourceText.FromText(input);
    }

    public static SourceText? FromFile(string path)
    {
        try
        {
            using var reader = new StreamReader(path);
            var text = reader.ReadToEnd();
            return SourceText.FromText(text);
        }
        catch (Exception ex)
        {
            $"Unable to load file '{path}':".Log(ConsoleColor.Red);
            ex.Log(ConsoleColor.Red);
            return null;
        }
    }
}
