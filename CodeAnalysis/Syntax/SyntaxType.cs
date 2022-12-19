namespace Theta.CodeAnalysis.Syntax;

public enum SyntaxType
{
    // Specials
    InvalidToken,
    EndOfFile,
    Whitespace,
    // Literal
    NumberToken,
    IdentifierToken,
    // Operators
    EqualsToken,
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
    ThinArrowToken,
    ThickArrowToken,
    // Brackets
    OpenGroup,
    CloseGroup,
    // Expressions
    LiteralExpression,
    BinaryExpression,
    GroupExpression,
    UnaryExpression,
    NameExpression,
    AssignmentExpression,
    // Keywords
    TrueKeyword,
    FalseKeyword,
    NullKeyword,
}
