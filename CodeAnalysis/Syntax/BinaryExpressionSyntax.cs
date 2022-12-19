using Theta.CodeAnalysis.Diagnostics;

namespace Theta.CodeAnalysis.Syntax;

public sealed class BinaryExpressionSyntax : ExpressionSyntax
{

    public required ExpressionSyntax Left { get; init; }
    public required SyntaxToken Operator { get; init; }
    public required ExpressionSyntax Right { get; init; }

    public override SyntaxType Type => SyntaxType.BinaryExpression;

    public override TextSpan Span => new(base.Span.Start, base.Span.Length - 1);

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