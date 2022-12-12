namespace Theta.CodeAnalysis.Binding;

using System;
using Theta.CodeAnalysis.Syntax;

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