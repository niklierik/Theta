namespace Theta.CodeAnalysis.Binding;

using System;
using Theta.CodeAnalysis.Syntax;

public sealed class BoundUnaryExpression : BoundExpression
{
    public BoundUnaryExpression(BoundExpression operand, BoundUnaryOperator op)
    {
        Operand = operand;
        Operator = op;
    }

    public override Type Type => Operator.ResultType;

    public override BoundNodeType NodeType => BoundNodeType.UnaryExpression;

    public BoundExpression Operand { get; }
    public BoundUnaryOperator Operator { get; }

}