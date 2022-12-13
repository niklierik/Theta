﻿namespace Theta.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.CodeAnalysis.Binding;
using Theta.CodeAnalysis.Syntax;
using static Theta.CodeAnalysis.Syntax.Parser;

public sealed class Evaluator
{

    private readonly BoundExpression _tree;

    public List<string> Diagnostics { get; } = new();

    public Evaluator(BoundExpression tree)
    {
        this._tree = tree;
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
    private object? EvaluateExpression(BoundExpression root)
    {
        if (root is BoundLiteralExpression literal)
        {
            return literal.Value;
        }
        /*
        if (root is BracketExpression expression)
        {
            return EvaluateExpression(expression.Expression);
        }
        */
        if (root is BoundUnaryExpression unary)
        {
            try
            {
                var operand = EvaluateExpression(unary.Operand);
                if (operand is null)
                {
                    Diagnostics.Add("ERROR: Unexpected null literal as an operand for unary expression.");
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
                        Diagnostics.Add($"ERROR: Unary operator {unary.Operator.Type} has undefined behaviour.");
                        return null;
                }
            }
            catch (Exception ex)
            {
                Diagnostics.Add(ex.ToString());
            }
        }
        if (root is BoundBinaryExpression binary)
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
                        return object.Equals(left, right);
                    case BoundBinaryOperatorType.Inequality:
                        return !object.Equals(left, right);
                    case BoundBinaryOperatorType.RefEquality:
                        return object.ReferenceEquals(left, right);
                    case BoundBinaryOperatorType.RefInequality:
                        return !object.ReferenceEquals(left, right);
                    case BoundBinaryOperatorType.Less:
                        return (dynamic) left < (dynamic) right;
                    case BoundBinaryOperatorType.Greater:
                        return (dynamic) left > (dynamic) right;
                    case BoundBinaryOperatorType.LessOrEquals:
                        return (dynamic) left <= (dynamic) right;
                    case BoundBinaryOperatorType.GreaterOrEquals:
                        return (dynamic) left >= (dynamic) right;
                    case BoundBinaryOperatorType.Comparsion:
                        if (object.Equals(left, right))
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
                Diagnostics.Add($"ERROR: Binary operator {binary.Operator.Type} has undefined behaviour.");
                return null;
            }
            catch (Exception ex)
            {
                Diagnostics.Add("ERROR: " + ex.ToString());
            }
        }
        Diagnostics.Add("ERROR: Cannot parse expression.");
        return null;
    }
}