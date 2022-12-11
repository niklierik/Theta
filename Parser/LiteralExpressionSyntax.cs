namespace Theta.Parser;

public sealed class LiteralExpressionSyntax : ExpressionSyntax
{

    public LiteralExpressionSyntax()
    {
    }

    public override SyntaxType Type => SyntaxType.LiteralExpression;
    
    public required SyntaxToken LiteralToken { get; init; }

    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            yield return LiteralToken;
        }
    }

}
