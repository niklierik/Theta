using System.Numerics;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
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
        var _type = BoundBinaryExpression.BindBinaryType(binary.Operator.Type, left.Type, right.Type, Diagnostics);
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
        var _type = BoundUnaryExpression.BindUnaryType(unary.Operator.Type, boundOperand.Type, Diagnostics);
        if (!_type.HasValue)
        {
            Diagnostics.Add($"ERROR: Cannot bind unary expression.");
            return null;
        }
        var type = _type.Value;
        return new BoundUnaryExpression(boundOperand, type);
    }

    

    
}