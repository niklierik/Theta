namespace Theta.CodeAnalysis.Binding;

public sealed class BoundGlobalScope
{
    public BoundGlobalScope(BoundGlobalScope? prev, List<VariableSymbol> declaredVars, BoundExpression? expression)
    {
        Prev = prev;
        DeclaredVars = declaredVars;
        Expression = expression;
    }

    public BoundGlobalScope? Prev { get; }
    public List<VariableSymbol> DeclaredVars { get; }
    public BoundExpression? Expression { get; }
}
