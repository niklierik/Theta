namespace Theta.CodeAnalysis.Binding;

using System;
using Theta.CodeAnalysis.Syntax;

public sealed class BoundLiteralExpression : BoundExpression
{

    public BoundLiteralExpression(object? value)
    {
        Value = value;
    }

    public override Type Type => Value?.GetType() ?? typeof(void);

    public override BoundNodeType NodeType => BoundNodeType.LiteralExpression;

    public object? Value { get; }
}