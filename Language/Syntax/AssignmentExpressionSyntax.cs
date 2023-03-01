namespace Theta.Language.Syntax;

public sealed class AssignmentExpressionSyntax : ExpressionSyntax
{

    public AssignmentExpressionSyntax(SyntaxToken identifier, SyntaxToken equalsTo, ExpressionSyntax expressionSyntax)
    {
        Identifier = identifier;
        EqualsTo = equalsTo;
        Expression = expressionSyntax;
    }

    public override SyntaxType Type => SyntaxType.AssignmentExpression;

    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            yield return Identifier;
            yield return EqualsTo;
            yield return Expression;
        }
    }

    public SyntaxToken Identifier { get; }
    public SyntaxToken EqualsTo { get; }
    public ExpressionSyntax Expression { get; }
}
