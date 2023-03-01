namespace Theta.Language.Binding;

public sealed class BoundGlobalScope
{
    public BoundGlobalScope(BoundGlobalScope? prev, List<VariableSymbol> declaredVars, List<BoundStatement> statements)
    {
        Prev = prev;
        DeclaredVars = declaredVars;
        Statements = statements;
    }

    public BoundGlobalScope? Prev { get; }
    public List<VariableSymbol> DeclaredVars { get; }
    public List<BoundStatement> Statements { get; }
}
