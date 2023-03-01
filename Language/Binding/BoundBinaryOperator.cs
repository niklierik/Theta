namespace Theta.Language.Binding;

using System.Numerics;
using Theta.Language.Objects.Types;
using Theta.Language.Syntax;

public sealed class BoundBinaryOperator
{

    public static List<BoundBinaryOperator> BinaryOperators { get; private set; } = GenerateDefaults().ToList();

    public static IEnumerable<BoundBinaryOperator> GenerateDefaults()
    {
        return new List<BoundBinaryOperator>()
        {
            new (SyntaxType.AmpersandAmpersandToken, BoundBinaryOperatorType.BoolAnd,Types.Bool),
            new (SyntaxType.PipePipeToken, BoundBinaryOperatorType.BoolOr,Types.Bool),
            new (SyntaxType.DoubleEqualsToken, BoundBinaryOperatorType.Equality,Types.Object,Types.Bool),
            new (SyntaxType.TripleEqualsToken, BoundBinaryOperatorType.RefEquality,Types.Object, Types.Bool),
            new (SyntaxType.BangEqualsToken, BoundBinaryOperatorType.Inequality,Types.Object,Types.Bool),
            new (SyntaxType.BangDoubleEqualsToken, BoundBinaryOperatorType.RefInequality,Types.Object,Types.Bool),
            //new (SyntaxType.GreaterToken, BoundBinaryOperatorType.Greater,typeof(IComparable),typeof(bool)),
            //new (SyntaxType.GreaterOrEqualsToken, BoundBinaryOperatorType.GreaterOrEquals,typeof(IComparable),typeof(bool)),
            //new (SyntaxType.LessToken, BoundBinaryOperatorType.Less,typeof(IComparable),typeof(bool)),
            //new (SyntaxType.LessOrEqualsToken, BoundBinaryOperatorType.LessOrEquals,typeof(IComparable),typeof(bool)),
            //new (SyntaxType.LessEqualsGreaterToken, BoundBinaryOperatorType.Comparsion,typeof(IComparable),typeof(long)),
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
        yield return new(syntax, bound, Types.Number);
    }

    public BoundBinaryOperatorType Type { get; }

    public SyntaxType SyntaxType { get; }
    public TypeIdentifier LeftOperandType { get; }
    public TypeIdentifier RightOperandType { get; }
    public TypeIdentifier ResultType { get; }
    // public Func<dynamic, dynamic, dynamic> Function { get; }

    public BoundBinaryOperator(SyntaxType syntaxType, BoundBinaryOperatorType boundType, TypeIdentifier leftOperandType, TypeIdentifier rightOperandType, TypeIdentifier resultType)
    {
        SyntaxType = syntaxType;
        Type = boundType;
        LeftOperandType = leftOperandType;
        RightOperandType = rightOperandType;
        ResultType = resultType;
    }

    public BoundBinaryOperator(SyntaxType syntaxType, BoundBinaryOperatorType boundType, TypeIdentifier type)
        : this(syntaxType, boundType, type, type, type) { }

    public BoundBinaryOperator(SyntaxType syntaxType, BoundBinaryOperatorType boundType, TypeIdentifier inType, TypeIdentifier outType)
        : this(syntaxType, boundType, inType, inType, outType) { }

    public static BoundBinaryOperator? Bind(SyntaxType syntaxType, TypeIdentifier left, TypeIdentifier right)
    {
        foreach (var op in BinaryOperators)
        {
            if (op.SyntaxType == syntaxType && op.RightOperandType.CanBeCastInto(right) && op.LeftOperandType.CanBeCastInto(left))
            {
                return op;
            }
        }
        return null;
    }
}
