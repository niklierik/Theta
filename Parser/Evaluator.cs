namespace Theta.Parser;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Theta.Parser.Parser;

public sealed class Evaluator
{

    private readonly SyntaxTree _tree;

    public List<string> Diagnostics { get; }

    public Evaluator(SyntaxTree tree, List<string> diagnostics)
    {
        this._tree = tree;
        this.Diagnostics = diagnostics;
    }

    public object? Evaluate()
    {
        return EvaluateExpression(_tree.Root);
    }

    private object? EvaluateExpression(SyntaxNode root)
    {
        if (root is SyntaxToken token)
        {
            return token.Value;
        }
        if (root is LiteralExpressionSyntax literal)
        {
            return EvaluateExpression(literal.LiteralToken);
        }
        if (root is BracketExpression expression)
        {
            return EvaluateExpression(expression.Expression);
        }
        if (root is UnaryExpression unary)
        {
            try
            {
                var operand = EvaluateExpression(unary.Operand);
                if (operand is null)
                {
                    Diagnostics.Add("ERROR: Unexpected null literal as an operand for unary expression.");
                    return null;
                }
                switch(unary.Operator.Type)
                {
                    case SyntaxType.Plus:
                        return +(dynamic) operand;
                    case SyntaxType.Minus:
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
        if (root is BinaryExpressionSyntax binaryOperator)
        {
            try
            {
                var left = EvaluateExpression(binaryOperator.Left);
                var right = EvaluateExpression(binaryOperator.Right);
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
                switch (binaryOperator.Operator.Type)
                {
                    case SyntaxType.Plus:
                        return (dynamic) left + (dynamic) right;
                    case SyntaxType.Minus:
                        return (dynamic) left - (dynamic) right;
                    case SyntaxType.Star:
                        return (dynamic) left * (dynamic) right;
                    case SyntaxType.Slash:
                        return (dynamic) left / (dynamic) right;
                    case SyntaxType.Percent:
                        return (dynamic) left % (dynamic) right;
                }
                Diagnostics.Add($"ERROR: Unexpected binary operator: {binaryOperator.Type}.");
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