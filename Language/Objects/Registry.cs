using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.Language.Binding;

namespace Theta.Language.Objects;

public class Registry
{
    public Registry()
    {

    }

    public Dictionary<string, object> GlobalObjects { get; set; } = new();

    public void Register(List<BoundStatement> statements)
    {
        foreach (var statement in statements)
        {
            statement.Register(this);
        }
    }
}