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

    public override Type Type => Operand.Type;

    public override BoundNodeType NodeType => BoundNodeType.UnaryExpression;

    public BoundExpression Operand { get; }
    public BoundUnaryOperator Operator { get; }

    public static BoundUnaryOperatorType? BindUnaryType(SyntaxType type, Type operandType, List<string> diagnostics)
    {
        if ((typeof(double) == operandType || typeof(long) == operandType))
        {
            switch (type)
            {
                case SyntaxType.PlusToken:
                    return BoundUnaryOperatorType.Plus;
                case SyntaxType.MinusToken:
                    return BoundUnaryOperatorType.Minus;
            }
        }
        if (typeof(bool) == operandType)
        {
            switch (type)
            {
                case SyntaxType.BangToken:
                    return BoundUnaryOperatorType.Not;
            }
        }
        diagnostics.Add($"ERROR: Invalid unary operator type {type} for operand {operandType}.");
        return null;
    }
}