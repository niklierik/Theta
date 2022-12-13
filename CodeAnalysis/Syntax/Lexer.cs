namespace Theta.CodeAnalysis.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.Utils;

public sealed class Lexer : IEnumerable<SyntaxToken>
{
    private readonly string _text;
    private int _pos;
    public List<string> Diagnostics { get; private set; } = new();

    public char CharAt(int pos)
    {
        if (pos < 0)
        {
            return '\0';
        }
        if (pos >= _text.Length)
        {
            return '\0';
        }
        return _text[pos];
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
            substr = _text.Substring(_pos, text.Length);
        }
        catch { }
        return substr == text;
    }


    public Lexer(string text)
    {
        _text = text;
        _pos = 0;
    }


    public void Continue()
    {
        _pos++;
    }

    public SyntaxToken Lex()
    {
        if (_pos >= _text.Length)
        {
            return new SyntaxToken(SyntaxType.EndOfFile) { Position = _text.Length, Text = "" };
        }
        if (char.IsDigit(Current))
        {
            var start = _pos;
            var integer = true;
            while (char.IsDigit(Current) || ((Current is '.' or '_') && char.IsDigit(Next)))
            {
                if (Current == '.')
                {
                    integer = false;
                }
                _pos++;
            }
            var length = _pos - start;
            var text = _text.Substring(start, length);
            if (integer)
            {
                if (!long.TryParse(text.Replace("_", ""), out var res))
                {
                    Diagnostics.Add($"ERROR: {text} cannot be interpreted as an integer.");
                }
                return new SyntaxToken(SyntaxType.Literal) { Position = start, Text = text, Value = res };
            }
            else
            {
                if (!double.TryParse(text.Replace("_", ""), out var res))
                {
                    Diagnostics.Add($"ERROR: {text} cannot be interpreted as a floating number.");
                }
                return new SyntaxToken(SyntaxType.Literal) { Position = start, Text = text, Value = res };
            }
        }
        if (char.IsWhiteSpace(Current))
        {
            var start = _pos;
            while (char.IsWhiteSpace(Current))
            {
                _pos++;
            }
            var length = _pos - start;
            var text = _text.Substring(start, length);
            return new SyntaxToken(SyntaxType.Whitespace) { Position = start, Text = text };
        }
        if (char.IsLetter(Current) || Current == '_')
        {
            var start = _pos;
            while (char.IsLetter(Current) || char.IsNumber(Current) || (Current is '.' or '_'))
            {
                _pos++;
            }
            var length = _pos - start;
            var text = _text.Substring(start, length);
            var type = SyntaxUtils.GetKeywordType(text);
            return new SyntaxToken(type) { Position = start, Text = text };
        }
        switch (Current)
        {
            case '+':
                return new SyntaxToken(SyntaxType.PlusToken) { Position = _pos++, Text = "+" };
            case '-':
                return new SyntaxToken(SyntaxType.MinusToken) { Position = _pos++, Text = "-" };
            case '*':
                return new SyntaxToken(SyntaxType.StarToken) { Position = _pos++, Text = "*" };
            case '/':
                return new SyntaxToken(SyntaxType.SlashToken) { Position = _pos++, Text = "/" };
            case '%':
                return new SyntaxToken(SyntaxType.PercentToken) { Position = _pos++, Text = "%" };
            case '^':
                return new SyntaxToken(SyntaxType.HatToken) { Position = _pos++, Text = "^" };
            case '(':
                return new SyntaxToken(SyntaxType.OpenGroup) { Position = _pos++, Text = "(" };
            case ')':
                return new SyntaxToken(SyntaxType.CloseGroup) { Position = _pos++, Text = ")" };
            case '!':
                return new SyntaxToken(SyntaxType.BangToken) { Position = _pos++, Text = "!" };
            case '&':
                if (Next == '&')
                {
                    return new SyntaxToken(SyntaxType.AmpersandAmpersandToken) { Position = _pos += 2, Text = "&&" };
                }
                break;
            case '|':
                if (Next == '|')
                {
                    return new SyntaxToken(SyntaxType.PipePipeToken) { Position = _pos += 2, Text = "||" };
                }
                break;
        }
        Diagnostics.Add($"ERROR: Invalid character input: {Current}.");
        return new SyntaxToken(SyntaxType.Invalid) { Position = _pos++, Text = _text.Substring(_pos - 1, 1) };
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
}