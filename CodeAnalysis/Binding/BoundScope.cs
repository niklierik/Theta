using System.Collections.ObjectModel;

namespace Theta.CodeAnalysis.Binding;
public sealed class BoundScope
{

    private readonly Dictionary<string, VariableSymbol> _variables = new();
    private readonly BoundScope? _parent;

    public BoundScope(BoundScope? parent = null)
    {
        _parent = parent;
    }

    public bool TryDeclare(VariableSymbol variable)
    {
        if (_variables.ContainsKey(variable.Name))
        {
            return false;
        }
        _variables.Add(variable.Name, variable);
        return true;
    }

    public bool TryLookup(string name, out VariableSymbol? variable)
    {
        if (_variables.TryGetValue(name, out variable))
        {
            return true;
        }
        if (_parent is null)
        {
            variable = null;
            return false;
        }
        return TryLookup(name, out variable);
    }

    public List<VariableSymbol> GetVariables() => _variables.Values.ToList();

}
