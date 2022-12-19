namespace Theta.CodeAnalysis.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.CodeAnalysis.Diagnostics;
using Theta.CodeAnalysis;
using System.Globalization;

public sealed class Lexer : IEnumerable<SyntaxToken>
{
    private readonly string _input;
    private int _pos;


    private SyntaxType _type;
    private int _start;
    private object? _value;
    private string _substr;


    public DiagnosticBag Diagnostics { get; private set; } = new();

    public char CharAt(int pos)
    {
        if (pos < 0)
        {
            return '\0';
        }
        if (pos >= _input.Length)
        {
            return '\0';
        }
        return _input[pos];
    }

    public char Prev => CharAt(_pos - 1);
    public char Current => CharAt(_pos);
    public char Next => CharAt(_pos + 1);

    public char Peek(int pos = 0)
    {
        return CharAt(_pos + pos);
    }

    public bool Match(string text)
    {
        string substr = "";
        try
        {
            substr = _input.Substring(_pos, text.Length);
        }
        catch
        {
            return false;
        }
        return substr == text;
    }


    public Lexer(string text)
    {
        _input = text;
        _pos = 0;
        _substr = "";
    }


    public void Continue()
    {
        _pos++;
    }

    private void CheckInvariantCulture()
    {
        if (CultureInfo.CurrentCulture != CultureInfo.InvariantCulture)
        {
            throw new Exception("Lexer can only work if the current culture is invariant.");
        }
    }

    private SyntaxToken? ReadNumber()
    {
        if (char.IsDigit(Current) || (Current == '.' && char.IsDigit(Next)))
        {
            var integer = Current != '.';
            while (char.IsDigit(Current) || ((Current is '.' or '_') && char.IsDigit(Next)))
            {
                if (Current == '.')
                {
                    integer = false;
                }
                _pos++;
            }
            var length = _pos - _start;
            _substr = _input.Substring(_start, length);
            _type = SyntaxType.NumberToken;
            if (integer)
            {
                if (!long.TryParse(_substr.Replace("_", ""), out var res))
                {
                    Diagnostics.ReportInvalidInt64(_substr, new(_start, length));
                }
                _value = res;
                return new SyntaxToken(_type) { Position = _start, Text = _substr, Value = _value };
            }
            else
            {
                if (!double.TryParse(_substr.Replace("_", ""), out var res))
                {
                    Diagnostics.ReportInvalidDouble(_substr, new(_start, length));
                }
                _value = res;
                return new SyntaxToken(_type) { Position = _start, Text = _substr, Value = _value };
            }
        }
        return null;
    }

    private SyntaxToken? ReadWhitespaces()
    {
        if (char.IsWhiteSpace(Current))
        {
            while (char.IsWhiteSpace(Current))
            {
                _pos++;
            }
            var length = _pos - _start;
            _substr = _input.Substring(_start, length);
            return new SyntaxToken(SyntaxType.Whitespace) { Position = _start, Text = _substr };
        }
        return null;
    }

    private SyntaxToken? ReadWord()
    {
        if (char.IsLetter(Current) || Current == '_')
        {
            var start = _pos;
            while (char.IsLetter(Current) || char.IsNumber(Current) || (Current is '.' or '_'))
            {
                _pos++;
            }
            var length = _pos - start;
            _substr = _input.Substring(start, length);
            var type = SyntaxUtils.GetKeywordType(_substr);
            return new SyntaxToken(type) { Position = start, Text = _substr };
        }
        return null;
    }

    private SyntaxToken? ReadFixText(SyntaxType type, string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }
        if (Match(text))
        {
            _pos += text.Length;
            return new SyntaxToken(type) { Position = _start, Text = text };
        }
        return null;
    }

    // Tries reading tokens in this order.
    // If it finds one, immeadietly returns it (therefore, => has to come first than = or >)
    private List<SyntaxType> ReadTypes { get; init; } = new()
    {
        SyntaxType.AmpersandAmpersandToken,
        SyntaxType.PipePipeToken,
        SyntaxType.LessEqualsGreaterToken,
        SyntaxType.ThinArrowToken,
        SyntaxType.ThickArrowToken,
        SyntaxType.GreaterOrEqualsToken,
        SyntaxType.LessOrEqualsToken,
        SyntaxType.TripleEqualsToken,
        SyntaxType.BangDoubleEqualsToken,
        SyntaxType.DoubleEqualsToken,
        SyntaxType.BangEqualsToken,
        SyntaxType.PlusToken,
        SyntaxType.MinusToken,
        SyntaxType.StarToken,
        SyntaxType.SlashToken,
        SyntaxType.PercentToken,
        SyntaxType.HatToken,
        SyntaxType.BangToken,
        SyntaxType.LessToken,
        SyntaxType.GreaterToken,
        SyntaxType.EqualsToken,
        SyntaxType.OpenGroup,
        SyntaxType.CloseGroup,
        SyntaxType.OpenBlock,
        SyntaxType.CloseBlock,
        SyntaxType.OpenArray,
        SyntaxType.CloseArray,
    };

    private SyntaxToken? ReadConcreteTokens()
    {
        foreach (var type in ReadTypes)
        {
            var token = ReadFixText(type, type.GetSyntaxText() ?? "");
            if (token is not null)
            {
                return token;
            }
        }
        return null;
    }

    public SyntaxToken Lex()
    {
        CheckInvariantCulture();
        _start = _pos;
        var token = ReadNumber();
        token ??= ReadWhitespaces();
        token ??= ReadWord();
        token ??= ReadConcreteTokens();
        token ??= Current == '\0' ? new SyntaxToken(SyntaxType.EndOfFile) { Position = _start, Text = "" } : null;
        if (token is null)
        {
            Diagnostics.ReportInvalidCharacter(Current, _pos);
        }
        return token ?? new SyntaxToken(SyntaxType.InvalidToken) { Position = _start, Text = Current.ToString() };
    }

    public IEnumerator<SyntaxToken> GetEnumerator()
    {
        while (true)
        {
            var token = Lex();
            yield return token;
            if (token.Type == SyntaxType.EndOfFile)
            {
                break;
            }
        }
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}