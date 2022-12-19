namespace Theta.CodeAnalysis.Binding;

using System;
using Theta.CodeAnalysis.Evaluation;

public abstract class BoundExpression : BoundNode
{
    public abstract Type Type { get; }

    public abstract object? Evaluate(Evaluator eval);
}