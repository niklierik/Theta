namespace Theta.Parser;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class SyntaxNode
{

    public abstract SyntaxType Type { get; }

    public abstract IEnumerable<SyntaxNode> Children { get; }

}
