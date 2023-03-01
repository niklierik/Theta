/*

namespace Theta.Tests.CodeAnalysis.Evaluation;

using System;
using System.Collections.Generic;
using System.Globalization;
using Theta.CodeAnalysis;
using Theta.CodeAnalysis.Evaluation;
using Theta.CodeAnalysis.Messages;
using Theta.CodeAnalysis.Syntax;

public class EvaluatorTest
{

    private Dictionary<VariableSymbol, object?> Vars { get; set; } = new();

    
    [Theory]
    [InlineData("1", (long) 1)]
    [InlineData("1+1", (long) 2)]
    [InlineData("+1", (long) 1)]
    [InlineData("-1", (long) -1)]
    [InlineData("true", true)]
    [InlineData("false", false)]
    [InlineData("3*2", (long) 6)]
    [InlineData("3*(2+2)", (long) 12)]
    [InlineData("(2+2)*3", (long) 12)]
    [InlineData("(2+2)^2", (double) 16)]
    [InlineData("2^2+2", (double) 6)]
    [InlineData("(13)", (long) 13)]
    [InlineData("null", null)]
    [InlineData("2.0", 2.0)]
    [InlineData("2.0 - 1", 1.0)]
    [InlineData("2.0 - 1.0", 1.0)]
    [InlineData("2.5 * 3.5", 2.5 * 3.5)]
    [InlineData("5 / 3", (long) 1)]
    [InlineData("5.0 / 2", 2.5)]
    [InlineData("5.0 == 5", true)]
    [InlineData("6.0 == 5", false)]
    [InlineData("4 != 4", false)]
    [InlineData("7.0 != 7", false)]
    [InlineData("1 <=> 2", -1)]
    [InlineData("1 <=> 1", 0)]
    [InlineData("2 <=> 0", 1)]
    [InlineData("2 < 3", true)]
    [InlineData("2 < 2", false)]
    [InlineData("2 < 0", false)]
    [InlineData("5 > 4", true)]
    [InlineData("5 > 5", false)]
    [InlineData("2 >= 2", true)]
    [InlineData("13 <= 13", true)]
    [InlineData("3 >= 2", true)]
    [InlineData("3 >= 534", false)]
    [MemberData(nameof(GetBooleanOperators))]
    public void Evaluator_Eval(string expression, object? value)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var result = Compilation.CompileText(expression, Vars);
        Assert.Equal(value, result.Value);
        Diagnostics.Clear();
    }

    private static IEnumerable<(SyntaxType syntax, Func<bool, bool, bool> func)> BooleanOperators
    {
        get
        {
            yield return (SyntaxType.AmpersandAmpersandToken, (a, b) => a && b);
            yield return (SyntaxType.PipePipeToken, (a, b) => a || b);
            yield return (SyntaxType.BangToken, (a, b) => !a);
            yield return (SyntaxType.DoubleEqualsToken, (a, b) => a == b);
            yield return (SyntaxType.BangEqualsToken, (a, b) => a != b);
        }
    }

    public static IEnumerable<object[]> GetBooleanOperators()
    {
        foreach (var op in BooleanOperators)
        {
            var optext = SyntaxUtils.GetSyntaxText(op.syntax);
            var unary = SyntaxUtils.GetUnaryOperatorPrecedence(op.syntax) > 0;
            foreach (var aV in new bool[] { true, false })
            {
                var a = aV ? "true" : "false";
                if (unary)
                {
                    yield return new object[] { $"{optext}{a}", op.func(aV, false) };
                    continue;
                }
                foreach (var bV in new bool[] { true, false })
                {
                    var b = bV ? "true" : "false";
                    
                    yield return new object[] { $"{a} {optext} {b}", op.func(aV, bV) };
                }
            }
        }
    }
}*/