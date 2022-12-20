using System.Globalization;
using Theta.CodeAnalysis;
using Theta.CodeAnalysis.Syntax;
using System.Linq;
using Theta.CodeAnalysis.Messages;

namespace Theta.Tests.CodeAnalysis.Syntax;

public class LexerTest
{


    [Theory]
    [MemberData(nameof(GetTokensData))]
    [MemberData(nameof(GetConcreteTokensData))]
    public void Lexers_Lexes_Token(SyntaxType type, string text)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var tokens = new Lexer(text).Where(token => token.Type != SyntaxType.EndOfFile);
        var token = Assert.Single(tokens);
        Assert.Equal(type, token.Type);
        Assert.Equal(text, (token as SyntaxToken)?.Text ?? null);
        Diagnostics.Clear();
    }


    [Theory]
    [MemberData(nameof(GetTokenPairsData))]
    public void Lexers_Lexes_TokenPairs(SyntaxType t1Type, string t1Text, SyntaxType t2Type, string t2Text)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var tokens = new Lexer(t1Text + " " + t2Text).Where(token => token.Type != SyntaxType.EndOfFile).ToList();
        Assert.Equal(3, tokens.Count);
        Assert.Equal(tokens[0].Type, t1Type);
        Assert.Equal(tokens[0].Text, t1Text);
        Assert.Equal(SyntaxType.Whitespace, tokens[1].Type);
        Assert.Equal(tokens[2].Type, t2Type);
        Assert.Equal(tokens[2].Text, t2Text);
        Diagnostics.Clear();
    }

    [Fact]
    public void Lexers_LexDoubleWithOperators()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var lexer = new Lexer("5.0 + 2.0").Where(token => token.Type != SyntaxType.Whitespace).Where(token => token.Type != SyntaxType.EndOfFile).ToArray();
        Assert.Equal("5.0", lexer[0].Text);
        Assert.Equal("+", lexer[1].Text);
        Assert.Equal("2.0", lexer[2].Text);
    }

    public static IEnumerable<object[]> GetTokensData()
    {
        return GetTokens().Select(input => new object[] { input.type, input.text });
    }

    public static IEnumerable<object?[]> GetConcreteTokensData()
    {
        return Enum.GetValues(typeof(SyntaxType)).Cast<SyntaxType>().Select(t => new object?[] { t, t.GetSyntaxText() }).Where(array => array[1] is not null);
    }


    public static IEnumerable<object[]> GetTokenPairsData()
    {
        return GetTokenPairs().Select(input => new object[] { input.t1Type, input.t1Text, input.t2Type, input.t2Text });
    }

    private static IEnumerable<(SyntaxType t1Type, string t1Text, SyntaxType t2Type, string t2Text)> GetTokenPairs()
    {
        foreach (var t1 in GetTokens())
        {
            if (t1.type == SyntaxType.Whitespace)
            {
                continue;
            }
            foreach (var t2 in GetTokens())
            {
                if (t2.type == SyntaxType.Whitespace)
                {
                    continue;
                }
                yield return (t1.type, t1.text, t2.type, t2.text);
            }
        }
    }

    public static IEnumerable<(SyntaxType type, string text)> GetTokens()
    {
        //yield return new(SyntaxType.TrueKeyword, "true");
        //yield return new(SyntaxType.FalseKeyword, "false");
        //yield return new(SyntaxType.NullKeyword, "null");
        //yield return new(SyntaxType.OpenGroup, "(");
        //yield return new(SyntaxType.CloseGroup, ")");
        //yield return new(SyntaxType.OpenArray, "[");
        //yield return new(SyntaxType.CloseArray, "]");
        //yield return new(SyntaxType.OpenBlock, "{");
        //yield return new(SyntaxType.CloseBlock, "}");
        //yield return new(SyntaxType.ThinArrowToken, "->");
        //yield return new(SyntaxType.ThickArrowToken, "=>");
        //yield return new(SyntaxType.EqualsToken, "=");
        //yield return new(SyntaxType.DoubleEqualsToken, "==");
        //yield return new(SyntaxType.TripleEqualsToken, "===");
        //yield return new(SyntaxType.BangToken, "!");
        //yield return new(SyntaxType.BangEqualsToken, "!=");
        //yield return new(SyntaxType.BangDoubleEqualsToken, "!==");
        //yield return new(SyntaxType.LessToken, "<");
        //yield return new(SyntaxType.GreaterToken, ">");
        //yield return new(SyntaxType.LessOrEqualsToken, "<=");
        //yield return new(SyntaxType.GreaterOrEqualsToken, ">=");
        //yield return new(SyntaxType.LessEqualsGreaterToken, "<=>");
        //yield return new(SyntaxType.PlusToken, "+");
        //yield return new(SyntaxType.MinusToken, "-");
        //yield return new(SyntaxType.StarToken, "*");
        //yield return new(SyntaxType.SlashToken, "/");
        //yield return new(SyntaxType.PercentToken, "%");
        //yield return new(SyntaxType.HatToken, "^");
        //yield return new(SyntaxType.AmpersandAmpersandToken, "&&");
        //yield return new(SyntaxType.PipePipeToken, "||");


        yield return new(SyntaxType.IdentifierToken, "a");
        yield return new(SyntaxType.IdentifierToken, "abc");
        yield return new(SyntaxType.Whitespace, " ");
        yield return new(SyntaxType.Whitespace, "  ");
        yield return new(SyntaxType.Whitespace, "\n");
        yield return new(SyntaxType.Whitespace, "\t");
        yield return new(SyntaxType.Whitespace, "\n\r");
        yield return new(SyntaxType.Whitespace, "\r\n");
        yield return new(SyntaxType.NumberToken, "2");
        yield return new(SyntaxType.NumberToken, "3.0");
        yield return new(SyntaxType.NumberToken, "5_000_000");
        yield return new(SyntaxType.NumberToken, "5_000_000.23");
    }


}