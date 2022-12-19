namespace Theta.CodeAnalysis.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.CodeAnalysis.Diagnostics;

public abstract class SyntaxNode
{

    public abstract SyntaxType Type { get; }

    public abstract IEnumerable<SyntaxNode> Children { get; }

    public virtual TextSpan Span
    {
        get
        {
            var first = Children.FirstOrDefault()?.Span ?? new();
            var last = Children.LastOrDefault()?.Span ?? new();
            return TextSpan.FromBounds(first.Start, last.End);
        }
    }

}
