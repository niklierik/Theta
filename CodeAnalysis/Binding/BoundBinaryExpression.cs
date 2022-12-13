using Theta.CodeAnalysis.Syntax;

namespace Theta.CodeAnalysis.Binding;

public sealed class BoundBinaryExpression : BoundExpression
{
    
    public BoundBinaryExpression(BoundExpression left, BoundBinaryOperator op, BoundExpression right)
    {
        Left = left;
        Operator = op;
        Right = right;
    }

    public BoundExpression Left { get; }
    public BoundExpression Right { get; }
    public BoundBinaryOperator Operator { get; }

    public override Type Type => Operator.ResultType;

    public override BoundNodeType NodeType => BoundNodeType.BinaryExpression;

}
