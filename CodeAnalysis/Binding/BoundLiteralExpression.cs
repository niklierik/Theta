namespace Theta.CodeAnalysis.Binding;

using System;
using Theta.CodeAnalysis.Evaluation;
using Theta.CodeAnalysis.Syntax;
using Theta.CodeAnalysis.Text;
using Theta.Transpilers;

public sealed class BoundLiteralExpression : BoundExpression
{

    public BoundLiteralExpression(object? value, TextSpan span)
    {
        Value = value;
        Span = span;
    }

    public override Type Type => Value?.GetType() ?? typeof(void);

    // public override BoundNodeType NodeType => BoundNodeType.LiteralExpression;

    public object? Value { get; }
    public override TextSpan Span { get; }

    /*
    public override object? Evaluate(Evaluator eval)
    {
        return Value;
    }
    */

    public override string Stringify(Transpiler transpiler, int indentation = 0)
    {
        return Value?.ToString() ?? "null";
    }
}