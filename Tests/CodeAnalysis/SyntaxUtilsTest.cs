using System.Globalization;
using Theta.CodeAnalysis;
using Theta.CodeAnalysis.Messages;
using Theta.CodeAnalysis.Syntax;

namespace Theta.Tests.CodeAnalysis;

public class SyntaxUtilsTest
{
    [Theory]
    [MemberData(nameof(GetSyntaxTypes))]
    public void SyntaxUtils_GetText_RoundTrips(SyntaxType type)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var text = type.GetSyntaxText();
        if (text is null)
        {
            return;
        }
        var tokens = new Lexer(text).Where(token => token.Type != SyntaxType.EndOfFile);
        var token = Assert.Single(tokens) as SyntaxToken;
        Assert.NotNull(token);
        Assert.Equal(type, token.Type);
        Assert.Equal(text, token.Text);
        Diagnostics.Clear();
    }

    [Theory]
    [MemberData(nameof(GetOperators))]
    public void SyntaxUtils_GetOperatorPrecedence_IsPositive(SyntaxType operatorToken)
    {
        var precedence = operatorToken.GetUnaryOperatorPrecedence();
        if (precedence <= 0)
        {
            precedence = operatorToken.GetBinaryOperatorPrecedence();
        }
        Assert.True(precedence > 0, "Precedence must be positive");
    }

    public static IEnumerable<object[]> GetSyntaxTypes()
    {
        return ((SyntaxType[]) Enum.GetValues(typeof(SyntaxType))).Select(value => new object[] { value });
    }

    public static IEnumerable<object[]> GetOperators()
    {
        return SyntaxUtils.OperatorTokens.Select(token => new object[] { token });
    }
}
