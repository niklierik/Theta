using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.Language.Objects.Types.Primitives;

namespace Theta.Language.Objects.Types;

public class VoidType : TypeIdentifier
{
    private static VoidType? _instance;
    private static object _lock = new();

    private VoidType() : base("Lib.Void")
    {
    }

    public static VoidType Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (_lock)
                {
                    if (_instance is null)
                    {
                        _instance = new VoidType();
                    }
                }
            }
            return _instance;
        }
    }

    public override bool CanBeCastInto(TypeIdentifier other)
    {
        return false;
    }

    public override bool IsNumber()
    {
        return false;
    }

    public override bool IsPrimitive()
    {
        return true;
    }
}