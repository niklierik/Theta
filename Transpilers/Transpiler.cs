namespace Theta.Transpilers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.CodeAnalysis.Binding;

public abstract partial class Transpiler : IDisposable
{
    public abstract void Dispose();
    public abstract void TranspileBlockStatement(BoundBlockStatement blockStatement, int indentation = 0);
    public abstract void TranspileExpressionStatement(BoundExpressionStatement expressionStatement, int indentation = 0);
}