using Theta.Language;
using Theta.Language.Evaluation;
using Theta.Language.Objects.Types;
using Theta.Language.Text;
using Theta.Transpilers;

namespace Theta.Language.Binding;

public sealed class BoundVariableExpression : BoundExpression
{
    public BoundVariableExpression(VariableSymbol variable, TextSpan span)
    {
        Variable = variable;
        Span = span;
    }

    public override TypeIdentifier Type => Variable.Type;

    public string Name => Variable.Name;
    public VariableSymbol Variable { get; }

    public override TextSpan Span { get; }

    /*
    public override object? Evaluate(Evaluator eval)
    {
        return eval.Vars[Variable];
    }
    */

    public override string Stringify(Transpiler transpiler, int indentation = 0)
    {
        return Variable.Name;
    }
}