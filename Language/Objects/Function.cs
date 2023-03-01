
namespace Theta.Language.Objects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.Language.Objects.Types;

public class Function
{

    public required FunctionType Type { get; init; }

    public required string Name { get; init; }

    public required string[] InputNames { get; init; }

    public required string[] OutputNames { get; init; }

}