namespace Theta.CodeAnalysis.Binding;

public sealed class BoundGlobalScope
{
    public BoundGlobalScope(BoundGlobalScope? prev, List<VariableSymbol> declaredVars, BoundStatement? statement)
    {
        Prev = prev;
        DeclaredVars = declaredVars;
        Statement = statement;
    }

    public BoundGlobalScope? Prev { get; }
    public List<VariableSymbol> DeclaredVars { get; }
    public BoundStatement? Statement { get; }
}
