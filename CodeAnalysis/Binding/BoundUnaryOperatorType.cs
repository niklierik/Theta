namespace Theta.CodeAnalysis.Binding;

public enum BoundUnaryOperatorType
{
    Plus,
    Minus,
    Not
}

public static class BoundUnaryOperatorHelper
{
    public static string GetFunctionName(this BoundUnaryOperatorType op)
    {
        return "__" + op.ToString().ToLower() + "__";
    }
}