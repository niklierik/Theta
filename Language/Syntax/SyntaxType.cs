﻿namespace Theta.Language.Syntax;

public enum SyntaxType
{
    // Specials
    InvalidToken,
    EndOfFile,
    Whitespace,
    // Literal
    NumberToken,
    IdentifierToken,
    QuotationMarkToken,
    ApostropheToken,
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
    OpenGroupToken,

    /// <summary>
    /// )
    /// </summary>
    CloseGroupToken,
    /// <summary>
    /// {
    /// </summary>
    OpenBlockToken,
    /// <summary>
    /// }
    /// </summary>
    CloseBlockToken,

    /// <summary>
    /// [
    /// </summary>
    OpenArrayToken,

    /// <summary>
    /// ]
    /// </summary>
    CloseArrayToken,

    /// <summary>
    /// ;
    /// </summary>
    SemicolonToken,

    /// <summary>
    /// .
    /// </summary>
    DotToken,

    /// <summary>
    /// ,
    /// </summary>
    CommaToken,

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

    /// <summary>
    /// let
    /// </summary>
    LetKeyword,

    /// <summary>
    /// const
    /// </summary>
    ConstKeyword,

    /// <summary>
    /// internal
    /// </summary>
    InternalKeyword,

    /// <summary>
    /// public
    /// </summary>
    PublicKeyword,

    /// <summary>
    /// protected
    /// </summary>
    ProtectedKeyword,

    /// <summary>
    /// private
    /// </summary>
    PrivateKeyword,
    /// <summary>
    /// alias
    /// </summary>
    AliasKeyword,

    // Expressions
    LiteralExpression,
    BinaryExpression,
    GroupExpression,
    UnaryExpression,
    NameExpression,
    AssignmentExpression,
    CompilationUnitNode,
    VariableDeclarationExpression,
    AliasStatement,
    // Statements
    ExpressionStatement,
    BlockStatement,
    CharToken,
    StringToken,
    NamespaceKeyword,
    NamespaceStatement,
}
