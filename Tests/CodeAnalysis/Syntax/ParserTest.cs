namespace Theta.Tests.CodeAnalysis.Syntax;

using System.Globalization;
using Theta.CodeAnalysis;
using Theta.CodeAnalysis.Syntax;

public class ParserTest
{


    [Theory]
    [MemberData(nameof(GetBinaryOperatorPairsData))]
    public void Parser_BinaryExpression_HonorsPrecedences(SyntaxType op1, SyntaxType op2)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var text1 = op1.GetSyntaxText();
        var text2 = op2.GetSyntaxText();
        Assert.NotNull(text1);
        Assert.NotNull(text2);
        var pr1 = op1.GetBinaryOperatorPrecedence();
        var pr2 = op2.GetBinaryOperatorPrecedence();
        var text = $"a {text1} b {text2} c";
        var expression = SyntaxTree.Parse(text);
        using var e = new AssertingEnumerator(expression.Root);
        if (pr1 >= pr2)
        {
            e.AssertNode(SyntaxType.BinaryExpression);
            e.AssertNode(SyntaxType.BinaryExpression);
            e.AssertNode(SyntaxType.NameExpression);
            e.AssertToken(SyntaxType.IdentifierToken, "a");
            e.AssertToken(op1, text1);
            e.AssertNode(SyntaxType.NameExpression);
            e.AssertToken(SyntaxType.IdentifierToken, "b");
            e.AssertToken(op2, text2);
            e.AssertNode(SyntaxType.NameExpression);
            e.AssertToken(SyntaxType.IdentifierToken, "c");
        }
        else
        {
            e.AssertNode(SyntaxType.BinaryExpression);
            e.AssertNode(SyntaxType.NameExpression);
            e.AssertToken(SyntaxType.IdentifierToken, "a");
            e.AssertToken(op1, text1);
            e.AssertNode(SyntaxType.BinaryExpression);
            e.AssertNode(SyntaxType.NameExpression);
            e.AssertToken(SyntaxType.IdentifierToken, "b");
            e.AssertToken(op2, text2);
            e.AssertNode(SyntaxType.NameExpression);
            e.AssertToken(SyntaxType.IdentifierToken, "c");
        }
    }


    [Theory]
    [MemberData(nameof(GetUnaryBinaryOperatorPairsData))]
    public void Parser_UnaryAndBinaryExpression_HonorsPrecedences(SyntaxType unary, SyntaxType binary)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var uText = unary.GetSyntaxText();
        var bText = binary.GetSyntaxText();
        Assert.NotNull(uText);
        Assert.NotNull(bText);
        var uPr = unary.GetUnaryOperatorPrecedence();
        var bPr = binary.GetBinaryOperatorPrecedence();
        var text = $"{uText} a {bText} b";
        var expression = SyntaxTree.Parse(text);
        using var e = new AssertingEnumerator(expression.Root);
        if (uPr >= bPr)
        {
            e.AssertNode(SyntaxType.BinaryExpression);
            e.AssertNode(SyntaxType.UnaryExpression);
            e.AssertToken(unary, uText);
            e.AssertNode(SyntaxType.NameExpression);
            e.AssertToken(SyntaxType.IdentifierToken, "a");
            e.AssertToken(binary, bText);
            e.AssertNode(SyntaxType.NameExpression);
            e.AssertToken(SyntaxType.IdentifierToken, "b");
        }
        else
        {
            e.AssertNode(SyntaxType.UnaryExpression);
            e.AssertToken(unary, uText);
            e.AssertNode(SyntaxType.BinaryExpression);
            e.AssertNode(SyntaxType.NameExpression);
            e.AssertToken(SyntaxType.IdentifierToken, "a");
            e.AssertToken(binary, bText);
            e.AssertNode(SyntaxType.NameExpression);
            e.AssertToken(SyntaxType.IdentifierToken, "b");
        }
    }

    public static IEnumerable<object[]> GetBinaryOperatorPairsData()
    {
        foreach (var op1 in SyntaxUtils.BinaryOperatorTokens)
        {
            foreach (var op2 in SyntaxUtils.BinaryOperatorTokens)
            {
                yield return new object[] { op1, op2 };
            }
        }
    }

    public static IEnumerable<object[]> GetUnaryBinaryOperatorPairsData()
    {
        foreach (var op1 in SyntaxUtils.UnaryOperatorTokens)
        {
            foreach (var op2 in SyntaxUtils.BinaryOperatorTokens)
            {
                yield return new object[] { op1, op2 };
            }
        }
    }
}