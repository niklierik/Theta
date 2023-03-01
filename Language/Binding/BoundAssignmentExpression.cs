using Theta.Language;
using Theta.Language.Evaluation;
using Theta.Language.Objects.Types;
using Theta.Language.Text;
using Theta.Transpilers;

namespace Theta.Language.Binding;

public sealed class BoundAssignmentExpression : BoundExpression
{
    public BoundAssignmentExpression(VariableSymbol var, BoundExpression? expression, TextSpan span)
    {
        Var = var;
        Expression = expression;
        Span = span;
    }

    public override TypeIdentifier Type => Expression?.Type ?? Types.Object;

    // public override BoundNodeType NodeType => BoundNodeType.AssignmentExpression;

    public VariableSymbol Var { get; }
    public BoundExpression? Expression { get; }

    public override TextSpan Span { get; }

    /*
    public override object? Evaluate(Evaluator eval)
    {
        var value = eval.EvaluateExpression(Expression);
        // eval.Vars[Var] = value;
        return value;
    }
    */

    public override string Stringify(Transpiler transpiler, int indentation = 0)
    {
        return $"({Var.Name} = {transpiler.StringifyExpression(Expression)})";
    }
}
