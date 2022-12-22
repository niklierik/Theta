using Theta.CodeAnalysis.Evaluation;
using Theta.Transpilers;

namespace Theta.CodeAnalysis.Binding;

public sealed class BoundExpressionStatement : BoundStatement
{
    public BoundExpressionStatement(BoundExpression expression)
    {
        Expression = expression;
    }

    public BoundExpression Expression { get; }

    public override void Transpile(Transpiler transpiler, int indentation = 0)
    {
        transpiler.TranspileExpressionStatement(this, indentation);
    }
}