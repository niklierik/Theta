using Theta.CodeAnalysis.Evaluation;
using Theta.CodeAnalysis.Messages;
using Theta.CodeAnalysis.Syntax;
using Theta.CodeAnalysis.Text;
using Theta.Transpilers;

namespace Theta.CodeAnalysis.Binding;

public sealed class BoundBinaryExpression : BoundExpression
{

    public BoundBinaryExpression(BoundExpression left, BoundBinaryOperator op, BoundExpression right, TextSpan span)
    {
        Left = left;
        Operator = op;
        Right = right;
        Span = span;
    }

    public BoundExpression Left { get; }
    public BoundExpression Right { get; }
    public override TextSpan Span { get; }
    public BoundBinaryOperator Operator { get; }

    public override Type Type => Operator.ResultType;

   // public override BoundNodeType NodeType => BoundNodeType.BinaryExpression;

    /*
    public override object? Evaluate(Evaluator eval)
    {
        var left = eval.EvaluateExpression(Left);
        var right = eval.EvaluateExpression(Right);

        // Binder makes sure if it is a null literal, then we will not get to this point if we aren't supposed to.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        switch (Operator.Type)
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
                try
                {
                    return (dynamic) left == (dynamic) right;
                }
                catch
                {
                    return Equals(left, right);
                }
            case BoundBinaryOperatorType.Inequality:
                try
                {
                    return (dynamic) left != (dynamic) right;
                }
                catch
                {
                    return !Equals(left, right);
                }
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
                if ((dynamic)left == (dynamic)right)
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
        Diagnostics.ReportUndefinedBinaryBehaviour(this, Span);
        return null;
    }
    */

    public override string Stringify(Transpiler transpiler)
    {
        throw new NotImplementedException();
    }
}
