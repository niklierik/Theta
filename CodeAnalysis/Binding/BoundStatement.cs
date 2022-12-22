namespace Theta.CodeAnalysis.Binding;

using System;
using Theta.CodeAnalysis.Evaluation;
using Theta.Transpilers;

public abstract class BoundStatement : BoundNode
{

    public abstract void Transpile(Transpiler transpiler, int indentation = 0);

}
