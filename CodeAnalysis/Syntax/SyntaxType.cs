﻿namespace Theta.CodeAnalysis.Syntax;

public enum SyntaxType
{
    Invalid,
    EndOfFile,
    Whitespace,
    Literal,
    Plus,
    Minus,
    Star,
    Slash,
    Percent,
    OpenBracket,
    CloseBracket,
    LiteralExpression,
    BinaryExpression,
    GroupExpression,
    UnaryExpression,
}