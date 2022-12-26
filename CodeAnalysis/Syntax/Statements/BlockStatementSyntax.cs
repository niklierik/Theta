namespace Theta.CodeAnalysis.Syntax.Statements;

using System;
using System.Collections.Generic;
using Theta.CodeAnalysis.Syntax;

public sealed class BlockStatementSyntax : StatementSyntax
{
    public BlockStatementSyntax(SyntaxToken open, SyntaxToken close, IEnumerable<StatementSyntax> statements)
    {
        Open = open;
        Close = close;
        Statements = statements;
    }

    public override SyntaxType Type => SyntaxType.BlockStatement;

    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            yield return Open;
            foreach (var statement in Statements)
            {
                yield return statement;
            }
            yield return Close;
        }
    }

    public SyntaxToken Open { get; }
    public IEnumerable<StatementSyntax> Statements { get; }
    public SyntaxToken Close { get; }
}
