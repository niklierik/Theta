using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using Theta.CodeAnalysis.Diagnostics;
using Theta.CodeAnalysis.Syntax;
using Theta.CodeAnalysis;

namespace Theta.CodeAnalysis.Binding;

public sealed class Binder
{
    public Binder(Dictionary<VariableSymbol, object?> vars)
    {
        Vars = vars;
    }

    public DiagnosticBag Diagnostics { get; private set; } = new();
    public Dictionary<VariableSymbol, object?> Vars { get; }

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
            case SyntaxType.NameExpression:
                NamedExpressionSyntax namedExpression = (NamedExpressionSyntax) syntax;
                return BindNamedExpression(namedExpression);
            case SyntaxType.AssignmentExpression:
                AssignmentExpressionSyntax assignmentExpression = (AssignmentExpressionSyntax) syntax;
                return BindAssignmentExpression(assignmentExpression);
            default:
                throw new Exception($"Cannot continue binding. Unexpected syntax {syntax.Type}.");
        }
    }


    private BoundExpression BindLiteralExpression(LiteralExpressionSyntax literal)
    {
        var value = literal.Value;
        return new BoundLiteralExpression(value, literal.Span);
    }

    private BoundExpression? BindBinaryExpression(BinaryExpressionSyntax binary)
    {
        var left = BindExpression(binary.Left);
        if (left is null)
        {
            Diagnostics.ReportBinderError(binary.Span);
            return null;
        }
        var right = BindExpression(binary.Right);
        if (right is null)
        {
            Diagnostics.ReportBinderError(binary.Span);
            return null;
        }
        var op = BoundBinaryOperator.Bind(binary.Operator.Type, left.Type, right.Type);
        if (op is null)
        {
            // Diagnostics.Add($"ERROR: Binary expression does not exist for {binary.Operator.Type} with operands {left.Type} and {right.Type}.");
            Diagnostics.ReportInvalidBinaryExpression(binary, left, right, binary.Span);
            return null;
        }
        return new BoundBinaryExpression(left, op, right, binary.Span);
    }

    private BoundExpression? BindUnaryExpression(UnaryExpressionSyntax unary)
    {
        var boundOperand = BindExpression(unary.Operand);
        if (boundOperand is null)
        {
            Diagnostics.ReportBinderError(unary.Span);
            return null;
        }
        var op = BoundUnaryOperator.Bind(unary.Operator.Type, boundOperand.Type);
        if (op is null)
        {
            Diagnostics.ReportInvalidUnaryExpression(unary, boundOperand, unary.Span);
            return null;
        }
        return new BoundUnaryExpression(boundOperand, op, unary.Span);
    }


    private BoundExpression? BindNamedExpression(NamedExpressionSyntax namedExpression)
    {
        var name = namedExpression.IdentifierToken.Text;
        var variable = Vars.FirstOrDefault(v => v.Key.Name == name);
        if (variable.Key is null)
        {
            Diagnostics.ReportUndefinedName(name, namedExpression.Span);
            return new BoundLiteralExpression(null, namedExpression.Span);
        }
        return new BoundVariableExpression(variable.Key, namedExpression.Span);
    }

    private BoundExpression? BindAssignmentExpression(AssignmentExpressionSyntax assignmentExpression)
    {
        var name = assignmentExpression.Identifier.Text;

        var expression = BindExpression(assignmentExpression.Expression);
        var variable = Vars.FirstOrDefault(v => v.Key.Name == name);
        if (variable.Key is not null && !variable.Key.Type.IsAssignableFrom(expression?.Type ?? typeof(void)))
        {
            Diagnostics.ReportInvalidCast(variable.Key, expression, assignmentExpression.Span);
            return new BoundLiteralExpression(null, assignmentExpression.Span);
        }
        return new BoundAssignmentExpression(name, expression, assignmentExpression.Span);
    }

}
