using Theta.CodeAnalysis;

namespace Theta.CodeAnalysis.Binding;

public sealed class BoundVariableExpression : BoundExpression
{
    public BoundVariableExpression(VariableSymbol variable)
    {
        Variable = variable;
    }

    public override Type Type => Variable.Type;

    public override BoundNodeType NodeType => BoundNodeType.VariableExpression;

    public string Name => Variable.Name;
    public VariableSymbol Variable { get; }
}