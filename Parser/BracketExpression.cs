namespace Theta.Parser;

public sealed class BracketExpression : ExpressionSyntax
{

    public required SyntaxToken Open { get; init; }
    public required ExpressionSyntax Expression { get; init; }
    public required SyntaxToken Close { get; init; }

    public override SyntaxType Type => SyntaxType.GroupExpression;

    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            yield return Open;
            yield return Expression;
            yield return Close;
        }
    }

}