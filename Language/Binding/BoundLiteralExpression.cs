namespace Theta.Language.Binding;

using System;
using Theta.Language.Evaluation;
using Theta.Language.Syntax;
using Theta.Language.Text;
using Theta.Transpilers;
using Newtonsoft.Json;
using Theta.Language.Objects.Types;

public sealed class BoundLiteralExpression : BoundExpression
{

    public BoundLiteralExpression(object? value, TextSpan span)
    {
        Value = value;
        Span = span;
    }

    public override TypeIdentifier Type => Value?.GetType().GetTypeID() ?? Types.Object;

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
        return transpiler.TranspileLiteral(this);
    }
}