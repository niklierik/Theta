namespace Theta.CodeAnalysis.Syntax;

public enum SyntaxType
{
    // Specials
    Invalid,
    EndOfFile,
    Whitespace,
    // Literal
    Literal,
    // Operators
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    PercentToken,
    HatToken,
    BangToken,
    AmpersandAmpersandToken,
    PipePipeToken,
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
