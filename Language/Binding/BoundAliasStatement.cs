using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.Language.Objects;
using Theta.Language.Syntax;
using Theta.Language.Text;
using Theta.Transpilers;

namespace Theta.Language.Binding;

public class BoundAliasStatement : BoundStatement
{

    public BoundAliasStatement(NamedExpressionSyntax _new, NamedExpressionSyntax old)
    {
        New = _new;
        Old = old;
    }

    public NamedExpressionSyntax New { get; }
    public NamedExpressionSyntax Old { get; }


    public override void Transpile(Transpiler transpiler, int indentation = 0)
    {
        transpiler.TranspileAliasStatement(this, indentation);
    }
}