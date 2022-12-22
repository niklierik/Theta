namespace Theta.CodeAnalysis.Binding;

using System;
using Theta.CodeAnalysis.Evaluation;
using Theta.CodeAnalysis.Text;

public abstract class BoundExpression : BoundNode
{
    public abstract Type Type { get; }

    public abstract TextSpan Span { get; }

   // public abstract object? Evaluate(Evaluator eval);
    public abstract string Stringify(StatementProcessor evaluator);
}
