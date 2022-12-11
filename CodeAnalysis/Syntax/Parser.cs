namespace Theta.CodeAnalysis.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Theta.Utils;

internal sealed class Parser
{
    private List<SyntaxToken> _tokens;

    private int _position;


    public List<string> Diagnostics { get; private set; } = new();

    public Parser(string text)
    {
        var lexer = new Lexer(text);
        _tokens = lexer.Where(x => x.Type != SyntaxType.Whitespace && x.Type != SyntaxType.Invalid).ToList();
        _position = 0;
        Diagnostics.AddRange(lexer.Diagnostics);
    }

    public SyntaxTree Parse()
    {
        var expression = ParseExpression();
        var eof = MatchToken(SyntaxType.EndOfFile);
        return new(Diagnostics)
        {
            Root = expression,
            EOF = eof
        };
    }

    private SyntaxToken NextToken()
    {
        var current = Current;
        _position++;
        return current;
    }

    private SyntaxToken Peek(int offset = 0)
    {
        var index = _position + offset;
        if (index >= _tokens.Count)
        {
            return _tokens.Last();
        }
        return _tokens[index];
    }

    private SyntaxToken Current => Peek();


    private SyntaxToken MatchToken(SyntaxType type)
    {
        if (Current.Type == type)
        {
            return NextToken();
        }
        Diagnostics.Add($"ERROR: Unexpected token <{Current.Type}> expected <{type}>.");
        return new SyntaxToken(type) { Position = _position, Text = "" };
    }


    private ExpressionSyntax ParseExpression(int parentPrecedence = 0)
    {
        ExpressionSyntax left;
        var unaryOperatorPrecedence = Current.Type.GetUnaryOperatorPrecedence();
        if (unaryOperatorPrecedence > 0 && unaryOperatorPrecedence >= parentPrecedence)
        {
            var operatorToken = NextToken();
            var operand = ParseExpression(unaryOperatorPrecedence);
            left = new UnaryExpression
            {
                Operand = operand,
                Operator = operatorToken,
            };
        }
        else
        {
            left = ParsePrimaryExpression();
        }
        while (true)
        {
            var precedence = Current.Type.GetBinaryOperatorPrecedence();
            if (precedence == 0 || precedence <= parentPrecedence)
            {
                break;
            }
            var operatorToken = NextToken();
            var right = ParseExpression(precedence);
            left = new BinaryExpressionSyntax()
            {
                Left = left,
                Right = right,
                Operator = operatorToken
            };
        }
        return left;
    }



    /*
    private ExpressionSyntax ParseTerm()
    {
        var left = ParseFactor();
        while (Current.Type is SyntaxType.Plus or SyntaxType.Minus)
        {
            var operatorToken = NextToken();
            var right = ParseFactor();
            left = new BinaryExpressionSyntax
            {
                Left = left,
                Operator = operatorToken,
                Right = right

            };
        }
        return left;
    }

    private ExpressionSyntax ParseFactor()
    {
        var left = ParsePrimaryExpression();
        while (Current.Type is SyntaxType.Star or SyntaxType.Slash or SyntaxType.Percent)
        {
            var operatorToken = NextToken();
            var right = ParsePrimaryExpression();
            left = new BinaryExpressionSyntax
            {
                Left = left,
                Operator = operatorToken,
                Right = right

            };
        }
        return left;
    }

    private ExpressionSyntax ParseExpression()
    {
        return ParseTerm();
    }
    */

    public ExpressionSyntax ParsePrimaryExpression()
    {
        if (Current.Type == SyntaxType.OpenBracket)
        {
            var left = NextToken();
            var expression = ParseExpression();
            var right = MatchToken(SyntaxType.CloseBracket);
            return new BracketExpression()
            {
                Open = left,
                Close = right,
                Expression = expression
            };
        }
        var literalToken = MatchToken(SyntaxType.Literal);

        return new LiteralExpressionSyntax()
        {
            LiteralToken = literalToken,
        };
    }

}