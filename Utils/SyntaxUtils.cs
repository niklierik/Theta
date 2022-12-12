namespace Theta.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.CodeAnalysis.Syntax;

public static class SyntaxUtils
{

    public const int Strongest = 10;

    public static int GetUnaryOperatorPrecedence(this SyntaxType type)
    {
        switch (type)
        {
            case SyntaxType.PlusToken:
            case SyntaxType.MinusToken:
                return Strongest - 1;
            case SyntaxType.BangToken:
                return Strongest;
            default:
                return 0;
        }
    }

    public static int GetBinaryOperatorPrecedence(this SyntaxType type)
    {
        switch (type)
        {
            case SyntaxType.PlusToken:
            case SyntaxType.MinusToken:
                return Strongest - 5;
            case SyntaxType.StarToken:
            case SyntaxType.SlashToken:
            case SyntaxType.PercentToken:
                return Strongest - 4;
            case SyntaxType.HatToken:
                return Strongest - 3;
            case SyntaxType.AmpersandAmpersandToken:
            case SyntaxType.PipePipeToken:
                return Strongest - 2;
            default:
                return 0;
        }
    }

    public static SyntaxType GetKeywordType(string text)
    {
        switch (text)
        {
            case "true":
                return SyntaxType.TrueKeyword;
            case "false":
                return SyntaxType.FalseKeyword;
            case "null":
                return SyntaxType.NullKeyword;
            default:
                return SyntaxType.IdentifierToken;
        }
    }

    public static bool IsBoolean(this SyntaxType type)
    {
        return type == SyntaxType.TrueKeyword || type == SyntaxType.FalseKeyword;
    }
}