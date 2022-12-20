namespace Theta.CodeAnalysis.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Theta.CodeAnalysis.Diagnostics;
using Theta.CodeAnalysis;
using Theta.CodeAnalysis.Text;

internal sealed class Parser
{
    private List<SyntaxToken> _tokens;

    private int _position;


    public DiagnosticBag Diagnostics { get; private set; } = new();

    public Parser(SourceText text)
    {
        var lexer = new Lexer(text);
        _tokens = lexer.Where(x => x.Type != SyntaxType.Whitespace && x.Type != SyntaxType.InvalidToken).ToList();
        _position = 0;
        Diagnostics.InsertAll(lexer.Diagnostics);
        Text = text;
    }

    public SyntaxTree Parse()
    {
        var expression = ParseExpression();
        var eof = MatchToken(SyntaxType.EndOfFile);
        return new(Diagnostics, Text)
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

    public SourceText Text { get; }

    private SyntaxToken MatchToken(SyntaxType type)
    {
        if (Current.Type == type)
        {
            return NextToken();
        }
        Diagnostics.ReportUnexpectedToken(Current.Span, Current.Type, type);
        return new SyntaxToken(type) { Position = _position, Text = "" };
    }
    private ExpressionSyntax ParseExpression()
    {
        return ParseAssignmentExpression();
    }

    private ExpressionSyntax ParseAssignmentExpression()
    {
        if (Peek(0).Type == SyntaxType.IdentifierToken && Peek(1).Type == SyntaxType.EqualsToken)
        {
            var identifierToken = NextToken();
            var operatorToken = NextToken();
            var right = ParseAssignmentExpression();
            return new AssignmentExpressionSyntax(identifierToken, operatorToken, right);
        }

        return ParseBinaryExpression();
    }



    private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
    {
        ExpressionSyntax left;
        var unaryOperatorPrecedence = Current.Type.GetUnaryOperatorPrecedence();
        if (unaryOperatorPrecedence > 0 && unaryOperatorPrecedence >= parentPrecedence)
        {
            var operatorToken = NextToken();
            var operand = ParseBinaryExpression(unaryOperatorPrecedence);
            left = new UnaryExpressionSyntax
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
            var right = ParseBinaryExpression(precedence);
            left = new BinaryExpressionSyntax()
            {
                Left = left,
                Right = right,
                Operator = operatorToken
            };
        }
        return left;
    }

    public ExpressionSyntax ParsePrimaryExpression()
    {
        switch (Current.Type)
        {
            case SyntaxType.OpenGroup:
                return ParseGroupExpression();

            case SyntaxType.TrueKeyword:
            case SyntaxType.FalseKeyword:
                return ParseBooleanExpression();

            case SyntaxType.NullKeyword:
                return ParseNull();

            case SyntaxType.NumberToken:
                return ParseNumberLiteral();
            
            case SyntaxType.IdentifierToken:
            default:
                return ParseNamedExpression();
        }
    }

    private ExpressionSyntax ParseLiteral(SyntaxType type)
    {
        var literalToken = MatchToken(type);

        return new LiteralExpressionSyntax(literalToken.Span)
        {
            Value = literalToken.Value,
        };
    }

    private ExpressionSyntax ParseNumberLiteral()
    {
        return ParseLiteral(SyntaxType.NumberToken);
    }

    private ExpressionSyntax ParseNull()
    {
        NextToken();
        return new LiteralExpressionSyntax(Current.Span) { Value = null };
    }

    private ExpressionSyntax ParseNamedExpression()
    {
        var identifierToken = NextToken();
        return new NamedExpressionSyntax(identifierToken);
    }

    private ExpressionSyntax ParseBooleanExpression()
    {
        var value = Current.Type == SyntaxType.TrueKeyword;
        NextToken();
        return new LiteralExpressionSyntax(Current.Span)
        {
            Value = value
        };
    }

    private ExpressionSyntax ParseGroupExpression()
    {
        var left = NextToken();
        var expression = ParseExpression();
        var right = MatchToken(SyntaxType.CloseGroup);
        return new BracketExpression()
        {
            Open = left,
            Close = right,
            Expression = expression
        };
    }
}