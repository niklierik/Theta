namespace Theta.CodeAnalysis.Binding;

using System;
using Theta.CodeAnalysis.Evaluation;
using Theta.CodeAnalysis.Syntax;

public sealed class BoundUnaryExpression : BoundExpression
{
    public BoundUnaryExpression(BoundExpression operand, BoundUnaryOperator op)
    {
        Operand = operand;
        Operator = op;
    }

    public override Type Type => Operator.ResultType;

    public override BoundNodeType NodeType => BoundNodeType.UnaryExpression;

    public BoundExpression Operand { get; }
    public BoundUnaryOperator Operator { get; }

    public override object? Evaluate(Evaluator eval)
    {
        var operand = eval.EvaluateExpression(Operand);
        if (operand is null)
        {
            eval.Diagnostics.ReportUnexpectedNull();
            return null;
        }
        switch (Operator.Type)
        {
            case BoundUnaryOperatorType.Plus:
                return +(dynamic) operand;
            case BoundUnaryOperatorType.Not:
                return !(dynamic) operand;
            case BoundUnaryOperatorType.Minus:
                return -(dynamic) operand;
            default:
                eval.Diagnostics.ReportUndefinedUnaryBehaviour(this);
                return null;
        }
    }
}