namespace Theta.CodeAnalysis.Binding;

using System;

public abstract class BoundExpression : BoundNode
{
    public abstract Type Type { get; }
}