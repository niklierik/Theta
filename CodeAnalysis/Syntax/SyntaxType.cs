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
    /// <summary>
    /// =
    /// </summary>
    EqualsToken,
    /// <summary>
    /// +
    /// </summary>
    PlusToken,
    /// <summary>
    /// -
    /// </summary>
    MinusToken,

    /// <summary>
    /// *
    /// </summary>
    StarToken,
    /// <summary>
    /// /
    /// </summary>
    SlashToken,
    /// <summary>
    /// %
    /// </summary>
    PercentToken,
    /// <summary>
    /// ^
    /// </summary>
    HatToken,
    /// <summary>
    /// !
    /// </summary>
    BangToken,
    /// <summary>
    /// &&
    /// </summary>
    AmpersandAmpersandToken,
    /// <summary>
    /// ||
    /// </summary>
    PipePipeToken,
    /// <summary>
    /// ==
    /// </summary>
    DoubleEqualsToken,
    /// <summary>
    /// !=
    /// </summary>
    BangEqualsToken,
    /// <summary>
    /// ===
    /// </summary>
    TripleEqualsToken,
    /// <summary>
    /// !==
    /// </summary>
    BangDoubleEqualsToken,
    /// <summary>
    /// <=>
    /// </summary>
    LessEqualsGreaterToken,
    /// <summary>
    /// >=
    /// </summary>
    GreaterOrEqualsToken,
    /// <summary>
    /// <=
    /// </summary>
    LessOrEqualsToken,
    /// <summary>
    /// <
    /// </summary>
    LessToken,
    /// <summary>
    /// >
    /// </summary>
    GreaterToken,
    /// <summary>
    /// ->
    /// </summary>
    ThinArrowToken,
    /// <summary>
    /// =>
    /// </summary>
    ThickArrowToken,
    // Brackets
    /// <summary>
    /// (
    /// </summary>
    OpenGroup,

    /// <summary>
    /// )
    /// </summary>
    CloseGroup,
    /// <summary>
    /// {
    /// </summary>
    OpenBlock,
    /// <summary>
    /// }
    /// </summary>
    CloseBlock,

    /// <summary>
    /// [
    /// </summary>
    OpenArray,
    /// <summary>
    /// ]
    /// </summary>
    CloseArray,
    // Expressions
    LiteralExpression,
    BinaryExpression,
    GroupExpression,
    UnaryExpression,
    NameExpression,
    AssignmentExpression,
    // Keywords

    /// <summary>
    /// true
    /// </summary>
    TrueKeyword,
    /// <summary>
    /// false
    /// </summary>
    FalseKeyword,
    /// <summary>
    /// null
    /// </summary>
    NullKeyword,
}
