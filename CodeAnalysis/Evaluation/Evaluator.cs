namespace Theta.CodeAnalysis.Evaluation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.CodeAnalysis.Binding;
using Theta.CodeAnalysis.Diagnostics;
using Theta.CodeAnalysis;
using static Theta.CodeAnalysis.Syntax.Parser;

public sealed class Evaluator
{

    private readonly BoundExpression _tree;

    public DiagnosticBag Diagnostics { get; } = new();
    public Dictionary<VariableSymbol, object?> Vars { get; }

    public Evaluator(BoundExpression tree, Dictionary<VariableSymbol, object?> vars)
    {
        _tree = tree;
        Vars = vars;
    }

    public object? Evaluate()
    {
        return EvaluateExpression(_tree);
    }
    /*
    public string AsStringVersion()
    {
        return AsStringVersion(_tree.Root);
    }

    public string AsStringVersion(SyntaxNode node)
    {
        if (node is SyntaxToken token)
        {
            return token.Value?.ToString() ?? "null";
        }
        return $"__{node.Type}__( {string.Join(", ", node.Children.Select(child => AsStringVersion(child)))} )";
    }
    */
    private object? EvaluateExpression(BoundExpression? root)
    {
        if (root is null)
        {
            return null;
        }
        else if (root is BoundLiteralExpression literal)
        {
            return literal.Value;
        }
        /*
        if (root is BracketExpression expression)
        {
            return EvaluateExpression(expression.Expression);
        }
        */
        else if (root is BoundUnaryExpression unary)
        {
            try
            {
                var operand = EvaluateExpression(unary.Operand);
                if (operand is null)
                {
                    Diagnostics.ReportUnexpectedNull();
                    return null;
                }
                switch (unary.Operator.Type)
                {
                    case BoundUnaryOperatorType.Plus:
                        return +(dynamic) operand;
                    case BoundUnaryOperatorType.Not:
                        return !(dynamic) operand;
                    case BoundUnaryOperatorType.Minus:
                        return -(dynamic) operand;
                    default:
                        Diagnostics.ReportUndefinedUnaryBehaviour(unary);
                        return null;
                }
            }
            catch (Exception ex)
            {
                Diagnostics.ReportException(ex);
            }
        }
        else if (root is BoundVariableExpression variable)
        {
            return Vars[variable.Variable];
        }
        else if (root is BoundAssignmentExpression assignment)
        {
            var value = EvaluateExpression(assignment.Expression);
            Vars[new VariableSymbol(assignment.Name, assignment.Type)] = value;
            return value;
        }
        else if (root is BoundBinaryExpression binary)
        {
            try
            {
                var left = EvaluateExpression(binary.Left);
                var right = EvaluateExpression(binary.Right);

                // Binder makes sure if it is a null literal, then we will not get to this point if we aren't supposed to.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                switch (binary.Operator.Type)
                {
                    case BoundBinaryOperatorType.Add:
                        return (dynamic) left + (dynamic) right;
                    case BoundBinaryOperatorType.Subtract:
                        return (dynamic) left - (dynamic) right;
                    case BoundBinaryOperatorType.Multiply:
                        return (dynamic) left * (dynamic) right;
                    case BoundBinaryOperatorType.Divide:
                        return (dynamic) left / (dynamic) right;
                    case BoundBinaryOperatorType.Modulo:
                        return (dynamic) left % (dynamic) right;
                    case BoundBinaryOperatorType.Pow:
                        return Math.Pow((dynamic) left, (dynamic) right);
                    case BoundBinaryOperatorType.BoolAnd:
                        return (dynamic) left && (dynamic) right;
                    case BoundBinaryOperatorType.BoolOr:
                        return (dynamic) left || (dynamic) right;
                    case BoundBinaryOperatorType.Equality:
                        return Equals(left, right);
                    case BoundBinaryOperatorType.Inequality:
                        return !Equals(left, right);
                    case BoundBinaryOperatorType.RefEquality:
                        return ReferenceEquals(left, right);
                    case BoundBinaryOperatorType.RefInequality:
                        return !ReferenceEquals(left, right);
                    case BoundBinaryOperatorType.Less:
                        return (dynamic) left < (dynamic) right;
                    case BoundBinaryOperatorType.Greater:
                        return (dynamic) left > (dynamic) right;
                    case BoundBinaryOperatorType.LessOrEquals:
                        return (dynamic) left <= (dynamic) right;
                    case BoundBinaryOperatorType.GreaterOrEquals:
                        return (dynamic) left >= (dynamic) right;
                    case BoundBinaryOperatorType.Comparsion:
                        if (Equals(left, right))
                        {
                            return 0;
                        }
                        try
                        {
                            if ((dynamic) left < (dynamic) right)
                            {
                                return -1;
                            }
                            return 1;
                        }
                        catch
                        {
                            return 1;
                        }
                }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                Diagnostics.ReportUndefinedBinaryBehaviour(binary);
                return null;
            }
            catch (Exception ex)
            {
                Diagnostics.ReportException(ex);
            }
        }
        Diagnostics.ReportInvalidExpression();
        return null;
    }
}