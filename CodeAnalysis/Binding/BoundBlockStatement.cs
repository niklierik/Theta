﻿using Theta.CodeAnalysis.Evaluation;
using Theta.Transpilers;

namespace Theta.CodeAnalysis.Binding;

public sealed class BoundBlockStatement : BoundStatement
{
    public BoundBlockStatement(IEnumerable<BoundStatement> statements)
    {
        Statements = statements;
    }

    public IEnumerable<BoundStatement> Statements { get; }

    public override void Transpile(Transpiler transpiler, int indentation = 0)
    {
        transpiler.TranspileBlockStatement(this, indentation);
    }
}
