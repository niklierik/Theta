namespace Theta.CodeAnalysis.Binding;

internal sealed class BoundBinaryExpression : BoundExpression
{
    public BoundBinaryExpression(BoundExpression left, BoundBinaryOperatorType operatorType, BoundExpression right)
    {
        Left = left;
        OperatorType = operatorType;
        Right = right;
    }

    public BoundExpression Left { get; }
    public BoundExpression Right { get; }
    public BoundBinaryOperatorType OperatorType { get; }

    public override Type Type => Left.Type;

    public override BoundNodeType NodeType => BoundNodeType.BinaryExpression;
}
