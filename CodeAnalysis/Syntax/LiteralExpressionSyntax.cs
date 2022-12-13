namespace Theta.CodeAnalysis.Syntax;

public sealed class LiteralExpressionSyntax : ExpressionSyntax
{

    public LiteralExpressionSyntax()
    {
    }

    public override SyntaxType Type => SyntaxType.LiteralExpression;

    // public required SyntaxToken LiteralToken { get; init; }
    public required object? Value { get; set; }

    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }

}
