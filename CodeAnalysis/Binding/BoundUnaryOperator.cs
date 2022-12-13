using Theta.CodeAnalysis.Syntax;

namespace Theta.CodeAnalysis.Binding;

public sealed class BoundUnaryOperator
{

    public static List<BoundUnaryOperator> UnaryOperators { get; private set; } = new()
    {
        new(SyntaxType.PlusToken,BoundUnaryOperatorType.Plus,typeof(long)),
        new(SyntaxType.MinusToken,BoundUnaryOperatorType.Minus,typeof(long)),
        new(SyntaxType.PlusToken,BoundUnaryOperatorType.Plus,typeof(double)),
        new(SyntaxType.MinusToken,BoundUnaryOperatorType.Minus,typeof(double)),
        new(SyntaxType.BangToken,BoundUnaryOperatorType.Not,typeof(bool))
    };


    public SyntaxType SyntaxType { get; }
    public BoundUnaryOperatorType Type { get; }
    public Type OperandType { get; }
    public Type ResultType { get; }

    public BoundUnaryOperator(SyntaxType syntaxType, BoundUnaryOperatorType boundType, Type operandType) : this(syntaxType, boundType, operandType, operandType) { }


    public BoundUnaryOperator(SyntaxType syntaxType, BoundUnaryOperatorType boundType, Type operandType, Type resultType)
    {
        SyntaxType = syntaxType;
        Type = boundType;
        OperandType = operandType;
        ResultType = resultType;
    }

    public static BoundUnaryOperator? Bind(SyntaxType syntaxType, Type operand)
    {
        foreach (var op in UnaryOperators)
        {
            if (op.SyntaxType == syntaxType && op.OperandType.IsAssignableFrom(operand))
            {
                return op;
            }
        }
        return null;
    }
}