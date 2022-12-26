using Theta.CodeAnalysis;
using Theta.CodeAnalysis.Evaluation;
using Theta.CodeAnalysis.Text;
using Theta.Transpilers;

namespace Theta.CodeAnalysis.Binding;

public sealed class BoundAssignmentExpression : BoundExpression
{
    public BoundAssignmentExpression(VariableSymbol var, BoundExpression? expression, TextSpan span)
    {
        Var = var;
        Expression = expression;
        Span = span;
    }

    public override Type Type => Expression?.Type ?? typeof(void);

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
