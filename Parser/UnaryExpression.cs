namespace Theta.Parser;

public sealed class UnaryExpression : ExpressionSyntax
{

    public required ExpressionSyntax Operand { get; init; }
    public required SyntaxToken Operator { get; init; }

    public override SyntaxType Type => SyntaxType.UnaryExpression;

    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            yield return Operator;
            yield return Operand;
        }
    }

}