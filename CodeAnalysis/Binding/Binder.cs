using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using Theta.CodeAnalysis.Messages;
using Theta.CodeAnalysis.Syntax;
using Theta.CodeAnalysis;

namespace Theta.CodeAnalysis.Binding;

public sealed class Binder
{

    public Binder(BoundScope? parent)
    {
        Scope = new BoundScope(parent);
    }

    public static BoundGlobalScope BindGlobalScope(BoundGlobalScope? prev, CompilationUnitSyntax compilation)
    {
        var parent = CreateParentScopes(prev);
        var binder = new Binder(parent);
        var expr = binder.BindExpression(compilation.Root);
        var variables = binder.Scope.GetVariables();
        return new BoundGlobalScope(prev, variables, expr);
    }

    private static BoundScope? CreateParentScopes(BoundGlobalScope? prev)
    {
        var stack = new Stack<BoundGlobalScope>();
        while (prev is not null)
        {
            stack.Push(prev);
            prev = prev.Prev;
        }
        BoundScope? current = null;
        while (stack.Count > 0)
        {
            var global = stack.Pop();
            var scope = new BoundScope(current);
            foreach (var var in global.DeclaredVars)
            {
                scope.TryDeclare(var);
            }
            current = scope;
        }
        return current;
    }

    public BoundScope Scope { get; private set; }

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
        if (!Scope.TryLookup(name, out var variable) || variable is null)
        {
            Diagnostics.ReportUndefinedName(name, namedExpression.Span);
            return new BoundLiteralExpression(null, namedExpression.Span);
        }
        return new BoundVariableExpression(variable, namedExpression.Span);
    }

    private BoundExpression? BindAssignmentExpression(AssignmentExpressionSyntax assignmentExpression)
    {
        var name = assignmentExpression.Identifier.Text;

        var expression = BindExpression(assignmentExpression.Expression);
        var variable = new VariableSymbol(name, expression?.Type ?? typeof(void));

        if (!Scope.TryDeclare(variable))
        {
            Diagnostics.ReportVarAlreadyDeclared(name, assignmentExpression.Span);
            return new BoundLiteralExpression(null, assignmentExpression.Span);
        }
        return new BoundAssignmentExpression(variable, expression, assignmentExpression.Span);
    }

}
