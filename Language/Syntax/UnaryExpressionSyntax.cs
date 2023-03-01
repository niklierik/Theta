namespace Theta.Language.Syntax;

public sealed class UnaryExpressionSyntax : ExpressionSyntax
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