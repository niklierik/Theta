namespace Theta.CodeAnalysis.Syntax;

public enum SyntaxType
{
    // Specials
    Invalid,
    EndOfFile,
    Whitespace,
    // Literal
    Literal,
    IdentifierToken,
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
    DoubleEqualsToken,
    BangEqualsToken,
    TripleEqualsToken,
    BangDoubleEqualsToken,
    LessEqualsGreaterToken,
    GreaterOrEqualsToken,
    LessOrEqualsToken,
    LessToken,
    GreaterToken,
    // Brackets
    OpenGroup,
    CloseGroup,
    // Expressions
    LiteralExpression,
    BinaryExpression,
    GroupExpression,
    UnaryExpression,
    NameExpression,
    // Keywords
    TrueKeyword,
    FalseKeyword,
    NullKeyword,
    AssignmentExpression,
    EqualsToken,
}
