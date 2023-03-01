using Theta.Language.Objects.Types;
using Theta.Language.Syntax;

namespace Theta.Language.Binding;

public sealed class BoundUnaryOperator
{

    public static List<BoundUnaryOperator> UnaryOperators { get; private set; } = new()
    {
        new(SyntaxType.PlusToken,BoundUnaryOperatorType.Plus,Types.Number),
        new(SyntaxType.MinusToken,BoundUnaryOperatorType.Minus,Types.Number),
        new(SyntaxType.BangToken,BoundUnaryOperatorType.Not,Types.Bool)
    };


    public SyntaxType SyntaxType { get; }
    public BoundUnaryOperatorType Type { get; }
    public TypeIdentifier OperandType { get; }
    public TypeIdentifier ResultType { get; }

    public BoundUnaryOperator(SyntaxType syntaxType, BoundUnaryOperatorType boundType, TypeIdentifier operandType) : this(syntaxType, boundType, operandType, operandType) { }


    public BoundUnaryOperator(SyntaxType syntaxType, BoundUnaryOperatorType boundType, TypeIdentifier operandType, TypeIdentifier resultType)
    {
        SyntaxType = syntaxType;
        Type = boundType;
        OperandType = operandType;
        ResultType = resultType;
    }

    public static BoundUnaryOperator? Bind(SyntaxType syntaxType, TypeIdentifier operand)
    {
        foreach (var op in UnaryOperators)
        {
            if (op.SyntaxType == syntaxType && op.OperandType.CanBeCastInto(operand))
            {
                return op;
            }
        }
        return null;
    }
}