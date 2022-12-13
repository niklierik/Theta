namespace Theta.CodeAnalysis.Binding;
using Theta.CodeAnalysis.Syntax;

public sealed class BoundBinaryOperator
{

    public static List<BoundBinaryOperator> BinaryOperators { get; private set; } = new()
    {
        new(SyntaxType.PlusToken,BoundBinaryOperatorType.Add,typeof(double)),
        new(SyntaxType.PlusToken,BoundBinaryOperatorType.Add,typeof(long)),
        new(SyntaxType.MinusToken,BoundBinaryOperatorType.Subtract,typeof(double)),
        new(SyntaxType.MinusToken,BoundBinaryOperatorType.Subtract,typeof(long)),
        new(SyntaxType.StarToken,BoundBinaryOperatorType.Multiply,typeof(double)),
        new(SyntaxType.StarToken,BoundBinaryOperatorType.Multiply,typeof(long)),
        new(SyntaxType.SlashToken,BoundBinaryOperatorType.Divide,typeof(double)),
        new(SyntaxType.SlashToken,BoundBinaryOperatorType.Divide,typeof(long)),
        new(SyntaxType.PercentToken,BoundBinaryOperatorType.Modulo,typeof(double)),
        new(SyntaxType.PercentToken,BoundBinaryOperatorType.Modulo,typeof(long)),
        new(SyntaxType.HatToken,BoundBinaryOperatorType.Pow,typeof(double)),
        new(SyntaxType.HatToken,BoundBinaryOperatorType.Pow,typeof(long)),
        new(SyntaxType.AmpersandAmpersandToken,BoundBinaryOperatorType.BoolAnd,typeof(bool)),
        new(SyntaxType.PipePipeToken,BoundBinaryOperatorType.BoolOr,typeof(bool)),
        new(SyntaxType.DoubleEqualsToken,BoundBinaryOperatorType.Equality,typeof(object),typeof(bool)),
        new(SyntaxType.TripleEqualsToken,BoundBinaryOperatorType.RefEquality,typeof(object),typeof(bool)),
        new(SyntaxType.BangEqualsToken,BoundBinaryOperatorType.Inequality,typeof(object),typeof(bool)),
        new(SyntaxType.BangDoubleEqualsToken,BoundBinaryOperatorType.RefInequality,typeof(object),typeof(bool)),
        new(SyntaxType.GreaterToken,BoundBinaryOperatorType.Greater,typeof(IComparable),typeof(bool)),
        new(SyntaxType.GreaterOrEqualsToken,BoundBinaryOperatorType.GreaterOrEquals,typeof(IComparable),typeof(bool)),
        new(SyntaxType.LessToken,BoundBinaryOperatorType.Less,typeof(IComparable),typeof(bool)),
        new(SyntaxType.LessOrEqualsToken,BoundBinaryOperatorType.LessOrEquals,typeof(IComparable),typeof(bool)),
        new(SyntaxType.LessEqualsGreaterToken,BoundBinaryOperatorType.Comparsion,typeof(IComparable),typeof(long)),

    };

    public BoundBinaryOperatorType Type { get; }

    public SyntaxType SyntaxType { get; }
    public Type LeftOperandType { get; }
    public Type RightOperandType { get; }
    public Type ResultType { get; }
    // public Func<dynamic, dynamic, dynamic> Function { get; }

    public BoundBinaryOperator(SyntaxType syntaxType, BoundBinaryOperatorType boundType, Type leftOperandType, Type rightOperandType, Type resultType)
    {
        SyntaxType = syntaxType;
        Type = boundType;
        LeftOperandType = leftOperandType;
        RightOperandType = rightOperandType;
        ResultType = resultType;
    }

    public BoundBinaryOperator(SyntaxType syntaxType, BoundBinaryOperatorType boundType, Type type)
        : this(syntaxType, boundType, type, type, type) { }

    public BoundBinaryOperator(SyntaxType syntaxType, BoundBinaryOperatorType boundType, Type inType, Type outType)
        : this(syntaxType, boundType, inType, inType, outType) { }

    public static BoundBinaryOperator? Bind(SyntaxType syntaxType, Type left, Type right)
    {
        foreach (var op in BinaryOperators)
        {
            if (op.SyntaxType == syntaxType && op.RightOperandType.IsAssignableFrom(right) && op.LeftOperandType.IsAssignableFrom(left))
            {
                return op;
            }
        }
        return null;
    }
}
