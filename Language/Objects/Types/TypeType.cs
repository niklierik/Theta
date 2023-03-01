using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theta.Language.Objects.Types;

public class TypeType : TypeIdentifier
{
    private static TypeType? _type;
    private static object _lock = new();

    private TypeType() : base("Lib.Type") { }

    public static TypeType Instance
    {
        get
        {
            if (_type is null)
            {
                lock (_lock)
                {
                    if (_type is null)
                    {
                        _type = new();
                    }
                }
            }
            return _type!;
        }
    }

    public override bool CanBeCastInto(TypeIdentifier other)
    {
        return other == this;
    }

    public override bool IsNumber()
    {
        return false;
    }

    public override bool IsPrimitive()
    {
        return false;
    }
}