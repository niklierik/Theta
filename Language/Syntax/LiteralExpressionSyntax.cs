using Theta.Language.Text;

namespace Theta.Language.Syntax;

public sealed class LiteralExpressionSyntax : ExpressionSyntax
{

    public LiteralExpressionSyntax(TextSpan span)
    {
        Span = span;
    }

    public override SyntaxType Type => SyntaxType.LiteralExpression;

    // public required SyntaxToken LiteralToken { get; init; }
    public required object? Value { get; set; }

    public override TextSpan Span { get; }



    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }

}
