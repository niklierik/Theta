namespace Theta.CodeAnalysis.Syntax;

public enum SyntaxType
{
    Invalid,
    EndOfFile,
    Whitespace,
    Literal,
    // Operators
    Plus,
    Minus,
    Star,
    Slash,
    Percent,
    Hat,
    // Brackets
    OpenGroup,
    CloseGroup,
    // Expressions
    LiteralExpression,
    BinaryExpression,
    GroupExpression,
    UnaryExpression,
    // Keywords
    TrueKeyword,
    FalseKeyword,
    NullKeyword,
    IdentifierToken,
}
