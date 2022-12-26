namespace Theta.CodeAnalysis;

public sealed class VariableSymbol
{
    public VariableSymbol(string name, Type type, bool isConst)
    {
        Name = name;
        Type = type;
        IsConst = isConst;
    }

    public string Name { get; }
    public Type Type { get; }

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