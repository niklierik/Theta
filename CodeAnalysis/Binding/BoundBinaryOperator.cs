namespace Theta.CodeAnalysis.Binding;

using System.Numerics;
using Theta.CodeAnalysis.Syntax;

public sealed class BoundBinaryOperator
{

    public static List<BoundBinaryOperator> BinaryOperators { get; private set; } = GenerateDefaults().ToList();

    public static IEnumerable<BoundBinaryOperator> GenerateDefaults()
    {
        return new List<BoundBinaryOperator>()
        {
            new (SyntaxType.AmpersandAmpersandToken, BoundBinaryOperatorType.BoolAnd,typeof(bool)),
            new (SyntaxType.PipePipeToken, BoundBinaryOperatorType.BoolOr,typeof(bool)),
            new (SyntaxType.DoubleEqualsToken, BoundBinaryOperatorType.Equality,typeof(object),typeof(bool)),
            new (SyntaxType.TripleEqualsToken, BoundBinaryOperatorType.RefEquality,typeof(object),typeof(bool)),
            new (SyntaxType.BangEqualsToken, BoundBinaryOperatorType.Inequality,typeof(object),typeof(bool)),
            new (SyntaxType.BangDoubleEqualsToken, BoundBinaryOperatorType.RefInequality,typeof(object),typeof(bool)),
            new (SyntaxType.GreaterToken, BoundBinaryOperatorType.Greater,typeof(IComparable),typeof(bool)),
            new (SyntaxType.GreaterOrEqualsToken, BoundBinaryOperatorType.GreaterOrEquals,typeof(IComparable),typeof(bool)),
            new (SyntaxType.LessToken, BoundBinaryOperatorType.Less,typeof(IComparable),typeof(bool)),
            new (SyntaxType.LessOrEqualsToken, BoundBinaryOperatorType.LessOrEquals,typeof(IComparable),typeof(bool)),
            new (SyntaxType.LessEqualsGreaterToken, BoundBinaryOperatorType.Comparsion,typeof(IComparable),typeof(long)),
        }
        .Concat(NumberOperator(SyntaxType.PlusToken, BoundBinaryOperatorType.Add))
        .Concat(NumberOperator(SyntaxType.MinusToken, BoundBinaryOperatorType.Subtract))
        .Concat(NumberOperator(SyntaxType.StarToken, BoundBinaryOperatorType.Multiply))
        .Concat(NumberOperator(SyntaxType.SlashToken, BoundBinaryOperatorType.Divide))
        .Concat(NumberOperator(SyntaxType.PercentToken, BoundBinaryOperatorType.Modulo))
        .Concat(NumberOperator(SyntaxType.HatToken, BoundBinaryOperatorType.Pow));
    }

    public static IEnumerable<BoundBinaryOperator> NumberOperator(SyntaxType syntax, BoundBinaryOperatorType bound)
    {
        yield return new(syntax, bound, typeof(double));
        yield return new(syntax, bound, typeof(long));
        yield return new(syntax, bound, typeof(long), typeof(double), typeof(double));
        yield return new(syntax, bound, typeof(double), typeof(long), typeof(double));
    }

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
