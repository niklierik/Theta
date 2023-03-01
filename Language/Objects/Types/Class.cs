namespace Theta.Language.Objects.Types;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.Language.Objects.Types;
using Theta.Language.Objects.Types.Primitives;

public class Class : TypeIdentifier
{
    public Class? Super { get; set; }

    public Class(string name) : this(name, ObjectClass.Instance) { }

    public Class(string name, Class? super) : base(name)
    {
        Super = super;
    }

    public override bool CanBeCastInto(TypeIdentifier other)
    {
        if (other == this)
        {
            return true;
        }
        if (Super is null)
        {
            return false;
        }
        return Super.CanBeCastInto(other);
    }

    public override bool IsPrimitive() => false;
    public override bool IsNumber() => false;
}
