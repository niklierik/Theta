using Theta.CodeAnalysis;
using Theta.CodeAnalysis.Evaluation;
using Theta.CodeAnalysis.Text;

namespace Theta.CodeAnalysis.Binding;

public sealed class BoundAssignmentExpression : BoundExpression
{
    public BoundAssignmentExpression(string name, BoundExpression? expression, TextSpan span)
    {
        Name = name;
        Expression = expression;
        Span = span;
    }

    public override Type Type => Expression?.Type ?? typeof(void);

    public override BoundNodeType NodeType => BoundNodeType.AssignmentExpression;

    public string Name { get; }
    public BoundExpression? Expression { get; }

    public override TextSpan Span { get; }

    public override object? Evaluate(Evaluator eval)
    {
        var value = eval.EvaluateExpression(Expression);
        eval.Vars[new VariableSymbol(Name, Type)] = value;
        return value;
    }
}
