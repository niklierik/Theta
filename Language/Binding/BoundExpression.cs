namespace Theta.Language.Binding;

using System;
using Theta.Language.Evaluation;
using Theta.Language.Text;
using Theta.Transpilers;
using Theta.Language.Objects.Types;

public abstract class BoundExpression : BoundNode
{
    public abstract TypeIdentifier Type { get; }

    public abstract TextSpan Span { get; }

    // public abstract object? Evaluate(Evaluator eval);
    public abstract string Stringify(Transpiler transpiler, int indentation = 0);
}
