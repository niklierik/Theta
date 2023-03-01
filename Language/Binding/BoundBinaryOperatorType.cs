using System.Drawing;
using System.Reflection;
using System;

namespace Theta.Language.Binding;

public enum BoundBinaryOperatorType
{
    Add,
    Subtract,
    Multiply,
    Divide,
    Modulo,
    Pow,
    BoolAnd,
    BoolOr,
    Equality,
    RefEquality,
    RefInequality,
    Inequality,
    Comparsion,
    LessOrEquals,
    Less,
    GreaterOrEquals,
    Greater,
}

public static class BoundBinaryOperatorHelper
{
    public static string GetFunctionName(this BoundBinaryOperatorType op)
    {
        return "__" + op.ToString().ToLower() + "__";
    }
}