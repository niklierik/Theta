namespace Theta.CodeAnalysis;

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
                switch (unary.OperatorType)
                {
                    case BoundUnaryOperatorType.Plus:
                        return +(dynamic) operand;
                    case BoundUnaryOperatorType.Not:
                        return !(dynamic) operand;
                    case BoundUnaryOperatorType.Minus:
                        return -(dynamic) operand;
                    default:
                        Diagnostics.Add($"ERROR: Unexpected binary operator: {unary.Type}.");
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
                if (left is null)
                {
                    Diagnostics.Add("ERROR: Unexpected null literal at the left of binary operation.");
                    return null;
                }
                if (right is null)
                {
                    Diagnostics.Add("ERROR: Unexpected null literal at the right of binary operation.");
                    return null;
                }
                switch (binary.OperatorType)
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
                }
                Diagnostics.Add($"ERROR: Unexpected binary operator: {binary.Type}.");
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