using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.Transpilers;

namespace Theta.Language.Binding;

public class BoundNamespaceStatement : BoundStatement
{

    public string Namespace { get; }

    public BoundNamespaceStatement(string ns)
    {
        Namespace = ns;
    }

    public override void Transpile(Transpiler transpiler, int indentation = 0)
    {
        transpiler.TranspileNamespaceStatement(this, indentation);
    }
}