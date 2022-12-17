﻿using System.Numerics;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using Theta.CodeAnalysis.Diagnostics;
using Theta.CodeAnalysis.Syntax;

namespace Theta.CodeAnalysis.Binding;

public sealed class Binder
{
    public Binder(Dictionary<string, object> vars)
    {
        Vars = vars;
    }

    public DiagnosticBag Diagnostics { get; private set; } = new();
    public Dictionary<string, object> Vars { get; }

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
        return new BoundLiteralExpression(value);
    }

    private BoundExpression? BindBinaryExpression(BinaryExpressionSyntax binary)
    {
        var left = BindExpression(binary.Left);
        if (left is null)
        {
            Diagnostics.ReportBinderError(binary.Operator.Span);
            return null;
        }
        var right = BindExpression(binary.Right);
        if (right is null)
        {
            Diagnostics.ReportBinderError(binary.Operator.Span);
            return null;
        }
        var op = BoundBinaryOperator.Bind(binary.Operator.Type, left.Type, right.Type);
        if (op is null)
        {
            // Diagnostics.Add($"ERROR: Binary expression does not exist for {binary.Operator.Type} with operands {left.Type} and {right.Type}.");
            Diagnostics.ReportInvalidBinaryExpression(binary, left, right, binary.Operator.Span);
            return null;
        }
        return new BoundBinaryExpression(left, op, right);
    }

    private BoundExpression? BindUnaryExpression(UnaryExpressionSyntax unary)
    {
        var boundOperand = BindExpression(unary.Operand);
        if (boundOperand is null)
        {
            Diagnostics.ReportBinderError(unary.Operator.Span);
            return null;
        }
        var op = BoundUnaryOperator.Bind(unary.Operator.Type, boundOperand.Type);
        if (op is null)
        {
            Diagnostics.ReportInvalidUnaryExpression(unary, boundOperand, unary.Operator.Span);
            return null;
        }
        return new BoundUnaryExpression(boundOperand, op);
    }


    private BoundExpression? BindNamedExpression(NamedExpressionSyntax namedExpression)
    {
        var name = namedExpression.IdentifierToken.Text;
        if (!Vars.TryGetValue(name, out var value))
        {
            Diagnostics.ReportUndefinedName(name, namedExpression.IdentifierToken.Span);
            return new BoundLiteralExpression(null);
        }
        var type = value?.GetType() ?? typeof(void);
        return new BoundVariableExpression(name, type);
    }

    private BoundExpression? BindAssignmentExpression(AssignmentExpressionSyntax assignmentExpression)
    {
        var name = assignmentExpression.Identifier.Text;
        var expression = BindExpression(assignmentExpression.Expression);
        return new BoundAssignmentExpression(name, expression);
    }

}
