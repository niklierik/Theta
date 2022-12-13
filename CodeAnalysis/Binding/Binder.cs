using System.Numerics;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using Theta.CodeAnalysis.Syntax;

namespace Theta.CodeAnalysis.Binding;

public sealed class Binder
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
            case SyntaxType.GroupExpression:
                BracketExpression bracketExpression = (BracketExpression) syntax;
                return BindExpression(bracketExpression.Expression);
            default:
                Diagnostics.Add($"ERROR: Unexpected syntax {syntax.Type}.");
                return null;
        }
    }

    private BoundExpression BindLiteralExpression(LiteralExpressionSyntax literal)
    {
        var value = literal.Value;
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
        var op = BoundBinaryOperator.Bind(binary.Operator.Type, left.Type, right.Type);
        if (op is null)
        {
            Diagnostics.Add($"ERROR: Binary expression does not exist for {binary.Operator.Type} with operands {left.Type} and {right.Type}.");
            return null;
        }
        return new BoundBinaryExpression(left, op, right);
    }

    private BoundExpression? BindUnaryExpression(UnaryExpressionSyntax unary)
    {
        var boundOperand = BindExpression(unary.Operand);
        if (boundOperand is null)
        {
            Diagnostics.Add($"ERROR: Cannot bind unary expression.");
            return null;
        }
        var op = BoundUnaryOperator.Bind(unary.Operator.Type, boundOperand.Type);
        if (op is null)
        {
            Diagnostics.Add($"ERROR: Unary operator does not exist for {unary.Operator.Type} with operand type {boundOperand.Type}.");
            return null;
        }
        return new BoundUnaryExpression(boundOperand, op);
    }




}