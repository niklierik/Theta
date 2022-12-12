namespace Theta.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.CodeAnalysis.Syntax;

public static class SyntaxUtils
{

    public static int GetUnaryOperatorPrecedence(this SyntaxType type)
    {
        switch (type)
        {
            case SyntaxType.Plus:
            case SyntaxType.Minus:
                return 3;
            default:
                return 0;
        }
    }

    public static int GetBinaryOperatorPrecedence(this SyntaxType type)
    {
        switch (type)
        {
            case SyntaxType.Plus:
            case SyntaxType.Minus:
                return 1;
            case SyntaxType.Star:
            case SyntaxType.Slash:
            case SyntaxType.Percent:
                return 2;
            case SyntaxType.Hat:
                return 3;
            default:
                return 0;
        }
    }
}