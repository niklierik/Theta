namespace Theta.CodeAnalysis.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public sealed class CompilationUnitSyntax : SyntaxNode
{
    public CompilationUnitSyntax(ExpressionSyntax expression, SyntaxToken eof)
    {
        Root = expression;
        EOF = eof;
    }

    public override SyntaxType Type => SyntaxType.CompilationUnitNode;

    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            yield return Root;
            yield return EOF;
        }
    }

    public ExpressionSyntax Root { get; }
    public SyntaxToken EOF { get; }
}