namespace Theta.Language.Objects.Types.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BoolType : TypeIdentifier
{

    private static BoolType? _instance;
    private static object _lock = new();

    private BoolType() : base("Lib.Bool")
    {
    }

    public static BoolType Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (_lock)
                {
                    if (_instance is null)
                    {
                        _instance = new BoolType();
                    }
                }
            }
            return _instance;
        }
    }

    public override bool IsPrimitive() => true;

    public override bool IsNumber() => false;

    public override bool CanBeCastInto(TypeIdentifier other)
    {
        return other == this;
    }
}