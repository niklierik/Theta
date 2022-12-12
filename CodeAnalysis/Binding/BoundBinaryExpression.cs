using Theta.CodeAnalysis.Syntax;

namespace Theta.CodeAnalysis.Binding;

internal sealed class BoundBinaryExpression : BoundExpression
{
    public BoundBinaryExpression(BoundExpression left, BoundBinaryOperatorType operatorType, BoundExpression right)
    {
        Left = left;
        OperatorType = operatorType;
        Right = right;
    }

    public BoundExpression Left { get; }
    public BoundExpression Right { get; }
    public BoundBinaryOperatorType OperatorType { get; }

    public override Type Type => Left.Type;

    public override BoundNodeType NodeType => BoundNodeType.BinaryExpression;
    public static BoundBinaryOperatorType? BindBinaryType(SyntaxType type, Type leftOperandType, Type rightOperandType, List<string> diagnostics)
    {
        if ((typeof(double) == leftOperandType || typeof(long) == leftOperandType) && (typeof(double) == rightOperandType || typeof(long) == rightOperandType))
        {
            switch (type)
            {
                case SyntaxType.PlusToken:
                    return BoundBinaryOperatorType.Add;
                case SyntaxType.MinusToken:
                    return BoundBinaryOperatorType.Subtract;
                case SyntaxType.StarToken:
                    return BoundBinaryOperatorType.Multiply;
                case SyntaxType.SlashToken:
                    return BoundBinaryOperatorType.Divide;
                case SyntaxType.PercentToken:
                    return BoundBinaryOperatorType.Modulo;
                case SyntaxType.HatToken:
                    return BoundBinaryOperatorType.Pow;
            }
        }
        if (typeof(bool) == leftOperandType && typeof(bool) == rightOperandType)
        {
            switch (type)
            {
                case SyntaxType.AmpersandAmpersandToken:
                    return BoundBinaryOperatorType.BoolAnd;
                case SyntaxType.PipePipeToken:
                    return BoundBinaryOperatorType.BoolOr;
            }
        }
        diagnostics.Add($"ERROR: Invalid binary operator type {type} for operands {leftOperandType} and {rightOperandType}.");
        return null;
    }

}
