namespace Theta.CodeAnalysis.Syntax;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.CodeAnalysis.Binding;
using Theta.CodeAnalysis.Syntax.Statements;

public sealed class CompilationUnitSyntax : SyntaxNode
{


    public CompilationUnitSyntax(StatementSyntax expression, SyntaxToken eof)
    {
        Statement = expression;
        EOF = eof;
    }



    public override SyntaxType Type => SyntaxType.CompilationUnitNode;

    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            yield return Statement;
            yield return EOF;
        }
    }

    public StatementSyntax Statement { get; }
    public SyntaxToken EOF { get; }
}