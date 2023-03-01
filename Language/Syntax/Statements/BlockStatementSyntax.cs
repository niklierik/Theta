namespace Theta.Language.Syntax.Statements;

using System;
using System.Collections.Generic;
using Theta.Language.Syntax;

public sealed class BlockStatementSyntax<T> : StatementSyntax where T : SyntaxNode
{
    public BlockStatementSyntax(SyntaxToken open, SyntaxToken close, IEnumerable<T> statements)
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
    public IEnumerable<T> Statements { get; }
    public SyntaxToken Close { get; }
}
