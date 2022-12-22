using Theta.CodeAnalysis;
using Theta.CodeAnalysis.Evaluation;
using Theta.CodeAnalysis.Text;
using Theta.Transpilers;

namespace Theta.CodeAnalysis.Binding;

public sealed class BoundVariableExpression : BoundExpression
{
    public BoundVariableExpression(VariableSymbol variable, TextSpan span)
    {
        Variable = variable;
        Span = span;
    }

    public override Type Type => Variable.Type;

    public string Name => Variable.Name;
    public VariableSymbol Variable { get; }

    public override TextSpan Span { get; }

    /*
    public override object? Evaluate(Evaluator eval)
    {
        return eval.Vars[Variable];
    }
    */

    public override string Stringify(Transpiler transpiler)
    {
        return Variable.Name;
    }
}