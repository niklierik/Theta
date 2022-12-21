namespace Theta.CodeAnalysis.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.CodeAnalysis.Binding;

public sealed class CompilationUnitSyntax : SyntaxNode
{


    public CompilationUnitSyntax(ExpressionSyntax expression, SyntaxToken eof)
    {
        Expression = expression;
        EOF = eof;
    }



    public override SyntaxType Type => SyntaxType.CompilationUnitNode;

    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            yield return Expression;
            yield return EOF;
        }
    }

    public ExpressionSyntax Expression { get; }
    public SyntaxToken EOF { get; }
}