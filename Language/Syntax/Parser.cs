namespace Theta.Language.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using Theta.Language.Messages;
using Theta.Language;
using Theta.Language.Text;
using Theta.Language.Syntax.Statements;

internal sealed class Parser
{
    private readonly List<SyntaxToken> _tokens;

    private int _position;


    public Parser(SourceText text)
    {
        var lexer = new Lexer(text);
        _tokens = lexer.ToList();
        _tokens = _tokens.Where(x => x.Type is not SyntaxType.Whitespace).ToList();
        if (Diagnostics.HasError || _tokens.Any(x => x.Type == SyntaxType.InvalidToken))
        {
            throw new HasErrorException();
        }
        _position = 0;
        Text = text;
    }


    public CompilationUnitSyntax ParseCompilationUnit()
    {
        var statements = ParseStatements();
        var eof = MatchToken(SyntaxType.EndOfFile);
        return new CompilationUnitSyntax(statements, eof);
    }

    private List<StatementSyntax> ParseStatements()
    {
        var list = new List<StatementSyntax>();
        while (Current.Type != SyntaxType.EndOfFile)
        {
            list.Add(ParseStatement());
        }
        return list;
    }

    /*

    public SyntaxTree Parse()
    {
        var expression = ParseExpression();
        var eof = MatchToken(SyntaxType.EndOfFile);
        return new SyntaxTree
        {
            Root = expression,
            EOF = eof,
            Src = Text
        };
    }
    */

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

    private SyntaxToken MatchToken(params SyntaxType[] types)
    {
        if (types.Contains(Current.Type))
        {
            return NextToken();
        }
        Diagnostics.ReportUnexpectedToken(Current.Span, Current.Type, types);
        return new SyntaxToken(Current.Type) { Position = _position, Text = "" };
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
        return Current.Type switch
        {
            SyntaxType.OpenGroupToken => ParseGroupExpression(),
            SyntaxType.TrueKeyword or SyntaxType.FalseKeyword => ParseBooleanExpression(),
            SyntaxType.NullKeyword => ParseNull(),
            SyntaxType.NumberToken => ParseNumberLiteral(),
            SyntaxType.StringToken => ParseStringLiteral(),
            SyntaxType.CharToken => ParseCharLiteral(),
            SyntaxType.LetKeyword or SyntaxType.ConstKeyword => ParseVariableDeclaration(),
            _ => ParseNamedExpression(),
        };
    }

    private ExpressionSyntax ParseVariableDeclaration()
    {
        var isConst = NextToken().Type == SyntaxType.ConstKeyword;
        var name = MatchToken(SyntaxType.IdentifierToken);
        var token = Peek(0);
        SyntaxToken? equalsToken = null;
        ExpressionSyntax? expression = null;
        if (token.Type == SyntaxType.EqualsToken)
        {
            equalsToken = NextToken();
            expression = ParseExpression();
        }
        return new VariableDeclarationSyntax(isConst, name, equalsToken, expression);
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
    private ExpressionSyntax ParseStringLiteral()
    {
        return ParseLiteral(SyntaxType.StringToken);
    }

    private ExpressionSyntax ParseCharLiteral()
    {
        return ParseLiteral(SyntaxType.CharToken);
    }

    private ExpressionSyntax ParseNull()
    {
        NextToken();
        return new LiteralExpressionSyntax(Current.Span) { Value = null };
    }

    private NamedExpressionSyntax ParseNamedExpression()
    {
        var tokens = new List<SyntaxToken>();
        var identifierToken = NextToken();
        if (identifierToken.Type != SyntaxType.IdentifierToken)
        {
            Diagnostics.ReportInvalidName(identifierToken);
        }
        tokens.Add(identifierToken);
        SyntaxToken? dot = null;
        SyntaxToken? name = null;
        while ((dot = Peek(0)).Type == SyntaxType.DotToken && (name = Peek(1)).Type == SyntaxType.IdentifierToken)
        {
            tokens.Add(dot);
            tokens.Add(name);
            NextToken();
            NextToken();
        }
        return new NamedExpressionSyntax(tokens.ToArray());
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
        var right = MatchToken(SyntaxType.CloseGroupToken);
        return new BracketExpression()
        {
            Open = left,
            Close = right,
            Expression = expression
        };
    }

    private AliasStatement ParseAliasStatement()
    {
        var aliasKeyword = NextToken();
        var name = ParseNamedExpression();
        if (Current.Type == SyntaxType.SemicolonToken)
        {
            return new AliasStatement()
            {
                AliasToken = aliasKeyword,
                OldName = name,
                EqualsToken = null,
                NewName = null,
                SemicolonToken = NextToken()
            };
        }
        return new AliasStatement()
        {
            AliasToken = aliasKeyword,
            NewName = name,
            EqualsToken = MatchToken(SyntaxType.EqualsToken),
            OldName = ParseNamedExpression(),
            SemicolonToken = MatchToken(SyntaxType.SemicolonToken)
        };
    }

    private StatementSyntax ParseStatement()
    {
        return Current.Type switch
        {
            SyntaxType.OpenBlockToken => ParseBlockStatement(),
            SyntaxType.AliasKeyword => ParseAliasStatement(),
            SyntaxType.NamespaceKeyword => ParseNamespaceStatement(),
            _ => ParseExpressionStatement(),
        };
    }

    private NamespaceStatement ParseNamespaceStatement()
    {
        return new NamespaceStatement()
        {
            NamespaceToken = MatchToken(SyntaxType.NamespaceKeyword),
            Name = ParseNamedExpression(),
            SemicolonToken = ParseSemicolon()
        };
    }

    private SyntaxToken ParseSemicolon()
    {
        return MatchToken(SyntaxType.SemicolonToken);
    }

    private ExpressionStatement ParseExpressionStatement()
    {
        var expression = ParseExpression();
        // NextToken();
        var semicolon = ParseSemicolon();
        return new ExpressionStatement(expression, semicolon);
    }

    private BlockStatementSyntax<StatementSyntax> ParseBlockStatement()
    {
        return ParseBlockStatement(ParseStatement);
    }

    private BlockStatementSyntax<T> ParseBlockStatement<T>(Func<T> parser) where T : SyntaxNode
    {
        var statements = new List<T>();

        var open = MatchToken(SyntaxType.OpenBlockToken);
        while (Current.Type != SyntaxType.EndOfFile && Current.Type != SyntaxType.CloseBlockToken)
        {
            var statement = parser();
            statements.Add(statement);
        }
        var close = MatchToken(SyntaxType.CloseBlockToken);
        return new BlockStatementSyntax<T>(open, close, statements);
    }
}