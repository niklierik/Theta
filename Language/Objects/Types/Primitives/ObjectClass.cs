namespace Theta.Language.Objects.Types.Primitives;

public sealed class ObjectClass : Class
{
    private static ObjectClass? _class;
    private static object threadsafety = new();

    public static ObjectClass Instance
    {
        get
        {
            if (_class is null)
            {
                lock (threadsafety)
                {
                    if (_class is null)
                    {
                        _class = new ObjectClass();
                    }
                }
            }
            return _class;
        }
    }

    private ObjectClass() : base("Lib.Object", null)
    {
    }

    public override bool CanBeCastInto(TypeIdentifier other) => true;
}