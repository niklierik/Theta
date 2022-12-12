using System.Numerics;
using System.Reflection.Emit;
using Theta.CodeAnalysis.Syntax;

namespace Theta.CodeAnalysis.Binding;

internal sealed class Binder
{
    public List<string> Diagnostics { get; private set; } = new();

    public BoundExpression? BindExpression(ExpressionSyntax? syntax)
    {
        if (syntax is null)
        {
            return null;
        }
        switch (syntax.Type)
        {
            case SyntaxType.LiteralExpression:
                return BindLiteralExpression((LiteralExpressionSyntax) syntax);
            case SyntaxType.BinaryExpression:
                BinaryExpressionSyntax binary = (BinaryExpressionSyntax) syntax;
                return BindBinaryExpression(binary);
            case SyntaxType.UnaryExpression:
                UnaryExpressionSyntax unary = (UnaryExpressionSyntax) syntax;
                return BindUnaryExpression(unary);
            default:
                Diagnostics.Add($"ERROR: Unexpected syntax {syntax.Type}.");
                return null;
        }
    }

    private BoundExpression BindLiteralExpression(LiteralExpressionSyntax literal)
    {
        var value = literal.LiteralToken.Value;
        return new BoundLiteralExpression(value);
    }

    private BoundExpression? BindBinaryExpression(BinaryExpressionSyntax binary)
    {
        var left = BindExpression(binary.Left);
        if (left is null)
        {
            Diagnostics.Add($"ERROR: Cannot bind binary expression.");
            return null;
        }
        var right = BindExpression(binary.Right);
        if (right is null)
        {
            Diagnostics.Add($"ERROR: Cannot bind binary expression.");
            return null;
        }
        var _type = BindBinaryType(binary.Operator.Type, left.Type, right.Type);
        if (!_type.HasValue)
        {
            Diagnostics.Add($"ERROR: Cannot bind binary expression.");
            return null;
        }
        var type = _type.Value;
        return new BoundBinaryExpression(left, type, right);
    }

    private BoundExpression? BindUnaryExpression(UnaryExpressionSyntax unary)
    {
        var boundOperand = BindExpression(unary.Operand);
        if (boundOperand is null)
        {
            Diagnostics.Add($"ERROR: Cannot bind unary expression.");
            return null;
        }
        var _type = BindUnaryType(unary.Operator.Type, boundOperand.Type);
        if (!_type.HasValue)
        {
            Diagnostics.Add($"ERROR: Cannot bind unary expression.");
            return null;
        }
        var type = _type.Value;
        return new BoundUnaryExpression(boundOperand, type);
    }

    private BoundUnaryOperatorType? BindUnaryType(SyntaxType type, Type operandType)
    {
        if ((typeof(double) == operandType || typeof(long) == operandType))
        {
            switch (type)
            {
                case SyntaxType.Plus:
                    return BoundUnaryOperatorType.Plus;
                case SyntaxType.Minus:
                    return BoundUnaryOperatorType.Minus;
            }
        }
        Diagnostics.Add($"ERROR: Invalid unary operator type {type} for operand {operandType}.");
        return null;
    }

    private BoundBinaryOperatorType? BindBinaryType(SyntaxType type, Type leftOperandType, Type rightOperandType)
    {
        if ((typeof(double) == leftOperandType || typeof(long) == leftOperandType) && (typeof(double) == rightOperandType || typeof(long) == rightOperandType))
        {
            switch (type)
            {
                case SyntaxType.Plus:
                    return BoundBinaryOperatorType.Add;
                case SyntaxType.Minus:
                    return BoundBinaryOperatorType.Subtract;
                case SyntaxType.Star:
                    return BoundBinaryOperatorType.Multiply;
                case SyntaxType.Slash:
                    return BoundBinaryOperatorType.Divide;
                case SyntaxType.Percent:
                    return BoundBinaryOperatorType.Modulo;
                case SyntaxType.Hat:
                    return BoundBinaryOperatorType.Pow;
            }
        }
        Diagnostics.Add($"ERROR: Invalid binary operator type {type} for operands {leftOperandType} and {rightOperandType}.");
        return null;
    }
}