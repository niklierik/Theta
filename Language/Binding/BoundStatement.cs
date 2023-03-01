namespace Theta.Language.Binding;

using System;
using Theta.Language.Evaluation;
using Theta.Language.Objects;
using Theta.Transpilers;

public abstract class BoundStatement : BoundNode
{

    public abstract void Transpile(Transpiler transpiler, int indentation = 0);

    public virtual void Register(Registry registry) { }
}
