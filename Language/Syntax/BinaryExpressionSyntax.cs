
namespace Theta.Language.Syntax;
using Theta.Language.Text;

public sealed class BinaryExpressionSyntax : ExpressionSyntax
{

    public required ExpressionSyntax Left { get; init; }
    public required SyntaxToken Operator { get; init; }
    public required ExpressionSyntax Right { get; init; }

    public override SyntaxType Type => SyntaxType.BinaryExpression;

    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            yield return Left;
            yield return Operator;
            yield return Right;
        }
    }

}