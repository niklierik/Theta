namespace Theta.CodeAnalysis.Binding;

public sealed class BoundVariableExpression : BoundExpression
{
    public BoundVariableExpression(string name, Type type)
    {
        Name = name;
        Type = type;
    }

    public override Type Type { get; }

    public override BoundNodeType NodeType => BoundNodeType.VariableExpression;

    public string Name { get; }
}