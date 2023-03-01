using Theta.Language.Evaluation;
using Theta.Transpilers;

namespace Theta.Language.Binding;

public sealed class BoundBlockStatement : BoundStatement
{
    public BoundBlockStatement(IEnumerable<BoundStatement> statements)
    {
        Statements = statements;
    }

    public IEnumerable<BoundStatement> Statements { get; }

    public override void Transpile(Transpiler transpiler, int indentation = 0)
    {
        transpiler.TranspileBlockStatement(this, indentation);
    }
}
