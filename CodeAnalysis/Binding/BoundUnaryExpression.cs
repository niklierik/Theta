namespace Theta.CodeAnalysis.Binding;

using System;

internal sealed class BoundUnaryExpression : BoundExpression
{
    public BoundUnaryExpression(BoundExpression operand, BoundUnaryOperatorType operatorType)
    {
        Operand = operand;
        OperatorType = operatorType;
    }

    public override Type Type => Operand.Type;

    public override BoundNodeType NodeType => BoundNodeType.UnaryExpression;

    public BoundExpression Operand { get; }
    public BoundUnaryOperatorType OperatorType { get; }
}