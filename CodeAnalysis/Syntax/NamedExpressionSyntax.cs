namespace Theta.CodeAnalysis.Syntax;

public sealed class NamedExpressionSyntax : ExpressionSyntax
{
    public override SyntaxType Type => SyntaxType.NameExpression;

    public SyntaxToken IdentifierToken { get; }

    public NamedExpressionSyntax(SyntaxToken identifierToken)
    {
        IdentifierToken = identifierToken;
    }

    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            yield return IdentifierToken;
        }
    }
}
