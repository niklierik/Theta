using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theta.Language.Objects.Types;

public class FunctionType : TypeIdentifier
{

    public FunctionType() : base("Lib.Function") { }
    public required List<TypeIdentifier> InputTypes { get; init; }

    public required List<TypeIdentifier> OutputTypes { get; init; }

    public override bool CanBeCastInto(TypeIdentifier other)
    {
        if (other is FunctionType ft)
        {
            if (InputTypes.Count != ft.InputTypes.Count)
            {
                return false;
            }
            if (OutputTypes.Count != ft.OutputTypes.Count)
            {
                return false;
            }
            for (int i = 0; i < ft.InputTypes.Count; i++)
            {
                var from = InputTypes[i];
                var to = ft.InputTypes[i];
                if (!from.CanBeCastInto(to))
                {
                    return false;
                }
            }
            for (int i = 0; i < ft.OutputTypes.Count; i++)
            {
                var from = OutputTypes[i];
                var to = ft.OutputTypes[i];
                if (!from.CanBeCastInto(to))
                {
                    return false;
                }
            }
            return true;
        }
        return false;
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