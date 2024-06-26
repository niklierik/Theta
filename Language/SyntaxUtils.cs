﻿namespace Theta.Language;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.Language.Syntax;
using Theta.Language.Objects.Types;

public static class SyntaxUtils
{

    // Some random number, just make sure it doesn't go to negative
    public const int Strongest = 100;

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
            case SyntaxType.AmpersandAmpersandToken:
            case SyntaxType.PipePipeToken:
                return Strongest - 8;
            case SyntaxType.DoubleEqualsToken:
            case SyntaxType.BangEqualsToken:
            case SyntaxType.TripleEqualsToken:
            case SyntaxType.BangDoubleEqualsToken:
                return Strongest - 7;
            case SyntaxType.GreaterOrEqualsToken:
            case SyntaxType.LessOrEqualsToken:
            case SyntaxType.GreaterToken:
            case SyntaxType.LessToken:
            case SyntaxType.LessEqualsGreaterToken:
                return Strongest - 6;
            case SyntaxType.PlusToken:
            case SyntaxType.MinusToken:
                return Strongest - 5;
            case SyntaxType.StarToken:
            case SyntaxType.SlashToken:
            case SyntaxType.PercentToken:
                return Strongest - 4;
            case SyntaxType.HatToken:
                return Strongest - 3;
            default:
                return 0;
        }
    }

    public static IEnumerable<SyntaxType> OperatorTokens
    {
        get
        {
            yield return SyntaxType.AmpersandAmpersandToken;
            yield return SyntaxType.PipePipeToken;
            yield return SyntaxType.DoubleEqualsToken;
            yield return SyntaxType.BangEqualsToken;
            yield return SyntaxType.TripleEqualsToken;
            yield return SyntaxType.BangDoubleEqualsToken;
            yield return SyntaxType.GreaterOrEqualsToken;
            yield return SyntaxType.LessOrEqualsToken;
            yield return SyntaxType.GreaterToken;
            yield return SyntaxType.LessToken;
            yield return SyntaxType.LessEqualsGreaterToken;
            yield return SyntaxType.PlusToken;
            yield return SyntaxType.MinusToken;
            yield return SyntaxType.StarToken;
            yield return SyntaxType.SlashToken;
            yield return SyntaxType.PercentToken;
            yield return SyntaxType.HatToken;
            yield return SyntaxType.PlusToken;
            yield return SyntaxType.MinusToken;
            yield return SyntaxType.BangToken;
        }
    }

    public static IEnumerable<SyntaxType> BinaryOperatorTokens
    {
        get
        {
            var types = (SyntaxType[]) Enum.GetValues(typeof(SyntaxType));
            return types.Where(type => GetBinaryOperatorPrecedence(type) > 0);
        }
    }

    public static IEnumerable<SyntaxType> UnaryOperatorTokens
    {
        get
        {
            var types = (SyntaxType[]) Enum.GetValues(typeof(SyntaxType));
            return types.Where(type => GetUnaryOperatorPrecedence(type) > 0);
        }
    }

    public static SyntaxType GetKeywordType(string text)
    {
        return text switch
        {
            "true" => SyntaxType.TrueKeyword,
            "false" => SyntaxType.FalseKeyword,
            "null" => SyntaxType.NullKeyword,
            "let" => SyntaxType.LetKeyword,
            "const" => SyntaxType.ConstKeyword,
            "public" => SyntaxType.PublicKeyword,
            "protected" => SyntaxType.ProtectedKeyword,
            "private" => SyntaxType.PrivateKeyword,
            "internal" => SyntaxType.InternalKeyword,
            "alias" => SyntaxType.AliasKeyword,
            "namespace" => SyntaxType.NamespaceKeyword,
            _ => SyntaxType.IdentifierToken,
        };
    }

    public static bool IsBoolean(this SyntaxType type)
    {
        return type == SyntaxType.TrueKeyword || type == SyntaxType.FalseKeyword;
    }

    public static TypeIdentifier? GetTypeID(this Type type)
    {
        if (typeof(long).Equals(type))
        {
            return Types.Long;
        }
        if (typeof(int).Equals(type))
        {
            return Types.Int;
        }
        if (typeof(short).Equals(type))
        {
            return Types.Short;
        }
        if (typeof(byte).Equals(type))
        {
            return Types.Byte;
        }
        if (typeof(float).Equals(type))
        {
            return Types.Float;
        }
        if (typeof(double).Equals(type))
        {
            return Types.Double;
        }
        if (typeof(bool).Equals(type))
        {
            return Types.Bool;
        }
        if (typeof(string).Equals(type))
        {
            return Types.String;
        }
        if (typeof(char).Equals(type))
        {
            return Types.Char;
        }
        if (typeof(object).Equals(type))
        {
            return Types.Object;
        }
        return Types.Void;
    }

    public static string? GetSyntaxText(this SyntaxType type)
    {
        return type switch
        {
            SyntaxType.EqualsToken => "=",
            SyntaxType.PlusToken => "+",
            SyntaxType.MinusToken => "-",
            SyntaxType.StarToken => "*",
            SyntaxType.SlashToken => "/",
            SyntaxType.PercentToken => "%",
            SyntaxType.HatToken => "^",
            SyntaxType.BangToken => "!",
            SyntaxType.AmpersandAmpersandToken => "&&",
            SyntaxType.PipePipeToken => "||",
            SyntaxType.DoubleEqualsToken => "==",
            SyntaxType.BangEqualsToken => "!=",
            SyntaxType.TripleEqualsToken => "===",
            SyntaxType.BangDoubleEqualsToken => "!==",
            SyntaxType.LessEqualsGreaterToken => "<=>",
            SyntaxType.GreaterOrEqualsToken => ">=",
            SyntaxType.LessOrEqualsToken => "<=",
            SyntaxType.LessToken => "<",
            SyntaxType.GreaterToken => ">",
            SyntaxType.OpenGroupToken => "(",
            SyntaxType.CloseGroupToken => ")",
            SyntaxType.OpenArrayToken => "[",
            SyntaxType.CloseArrayToken => "]",
            SyntaxType.OpenBlockToken => "{",
            SyntaxType.CloseBlockToken => "}",
            SyntaxType.ThinArrowToken => "->",
            SyntaxType.ThickArrowToken => "=>",
            SyntaxType.SemicolonToken => ";",
            SyntaxType.DotToken => ".",
            SyntaxType.CommaToken => ",",
            SyntaxType.TrueKeyword => "true",
            SyntaxType.FalseKeyword => "false",
            SyntaxType.NullKeyword => "null",
            SyntaxType.LetKeyword => "let",
            SyntaxType.ConstKeyword => "const",
            SyntaxType.PublicKeyword => "public",
            SyntaxType.ProtectedKeyword => "protected",
            SyntaxType.PrivateKeyword => "private",
            SyntaxType.InternalKeyword => "internal",
            SyntaxType.AliasKeyword => "alias",
            SyntaxType.NamespaceKeyword => "namespace",
            _ => null
        };
    }
}