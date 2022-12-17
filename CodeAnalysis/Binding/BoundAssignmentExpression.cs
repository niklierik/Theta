namespace Theta.CodeAnalysis.Binding;

public sealed class BoundAssignmentExpression : BoundExpression
{
    public BoundAssignmentExpression(string name, BoundExpression? expression)
    {
        Name = name;
        Expression = expression;
    }

    public override Type Type => Expression?.Type ?? typeof(void);

    public override BoundNodeType NodeType => BoundNodeType.AssignmentExpression;

    public string Name { get; }
    public BoundExpression? Expression { get; }
}
