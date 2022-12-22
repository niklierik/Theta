namespace Theta.CodeAnalysis.Syntax.Statements;

public sealed class ExpressionStatement : StatementSyntax
{
    public ExpressionStatement(ExpressionSyntax expression, SyntaxToken semicolon)
    {
        Expression = expression;
        Semicolon = semicolon;
    }

    public override SyntaxType Type => SyntaxType.ExpressionStatement;

    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            yield return Expression;
            yield return Semicolon;
        }
    }

    public ExpressionSyntax Expression { get; }
    public SyntaxToken Semicolon { get; }
}
