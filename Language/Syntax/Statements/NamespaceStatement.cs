using Theta.Language.Messages;

namespace Theta.Language.Syntax.Statements;

public class NamespaceStatement : StatementSyntax
{
    public override SyntaxType Type => SyntaxType.NamespaceStatement;

    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            yield return NamespaceToken;
            yield return Name;
            yield return SemicolonToken;
        }
    }

    public required SyntaxToken NamespaceToken { get; init; }
    public required NamedExpressionSyntax Name { get; init; }
    public required SyntaxToken SemicolonToken { get; init; }
}