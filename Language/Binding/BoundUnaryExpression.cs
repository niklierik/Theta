namespace Theta.Language.Binding;

using System;
using Theta.Language.Evaluation;
using Theta.Language.Messages;
using Theta.Language.Objects.Types;
using Theta.Language.Syntax;
using Theta.Language.Text;
using Theta.Transpilers;

public sealed class BoundUnaryExpression : BoundExpression
{
    public BoundUnaryExpression(BoundExpression operand, BoundUnaryOperator op, TextSpan span)
    {
        Operand = operand;
        Operator = op;
        Span = span;
    }

    public override TypeIdentifier Type => Operator.ResultType;

    // public override BoundNodeType NodeType => BoundNodeType.UnaryExpression;

    public BoundExpression Operand { get; }
    public BoundUnaryOperator Operator { get; }
    public override TextSpan Span { get; }

    /*
    public override object? Evaluate(Evaluator eval)
    {
        var operand = eval.EvaluateExpression(Operand);
        if (operand is null)
        {
            Diagnostics.ReportUnexpectedNull(Span);
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
                Diagnostics.ReportUndefinedUnaryBehaviour(this, Span);
                return null;
        }
    }
    */

    public override string Stringify(Transpiler transpiler, int indentation = 0)
    {
        var operand = transpiler.StringifyExpression(Operand);
        return $"{Operator.Type.GetFunctionName()}({operand})";
    }
}