namespace Theta.Language.Objects.Types.Primitives;

public class NumberType : TypeIdentifier
{

    private static NumberType? _type = null;
    private static object _lock = new();

    public static NumberType Type
    {
        get
        {
            if (_type is null)
            {
                lock (_lock)
                {
                    if (_type is null)
                    {
                        _type = new("Lib.Number");
                    }
                }
            }
            return _type!;
        }
    }

    protected NumberType(string name) : base(name)
    {
    }

    public override bool IsPrimitive() => false;
    public override bool IsNumber() => true;

    public override bool CanBeCastInto(TypeIdentifier other) => other.IsNumber();



}