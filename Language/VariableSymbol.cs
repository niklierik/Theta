using Theta.Language.Objects.Types;

namespace Theta.Language;

public sealed class VariableSymbol
{
    public VariableSymbol(string name, TypeIdentifier type, bool isConst)
    {
        Name = name;
        Type = type;
        IsConst = isConst;
    }

    public string Name { get; }
    public TypeIdentifier Type { get; }

    public bool IsConst { get; }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name.GetHashCode(), Type.GetHashCode());
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }
        if (obj is VariableSymbol var)
        {
            return object.Equals(var.Name, Name) && object.Equals(var.Type, Type);
        }
        return false;
    }
}