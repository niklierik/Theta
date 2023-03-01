namespace Theta.Language.Syntax;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.Language.Binding;

public sealed class CompilationUnitSyntax : SyntaxNode
{


    public CompilationUnitSyntax(List<StatementSyntax> expressions, SyntaxToken eof)
    {
        Statements = expressions;
        EOF = eof;
    }



    public override SyntaxType Type => SyntaxType.CompilationUnitNode;

    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            foreach (var statement in Statements)
            {
                yield return statement;
            }
            yield return EOF;
        }
    }

    public List<StatementSyntax> Statements { get; }
    public SyntaxToken EOF { get; }
}