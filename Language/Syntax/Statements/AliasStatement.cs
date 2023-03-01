using Theta.Language.Messages;

namespace Theta.Language.Syntax.Statements;

public class AliasStatement : StatementSyntax
{
    public override SyntaxType Type => SyntaxType.AliasStatement;

    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            yield return AliasToken;
            if (NewName is not null || EqualsToken is not null)
            {
                if (NewName is null || EqualsToken is null)
                {
                    Diagnostics.ReportInvalidAliasStatement(Span);
                }
                else
                {
                    yield return NewName;
                    yield return EqualsToken;
                }
            }
            yield return OldName;
        }
    }

    public required SyntaxToken AliasToken { get; init; }
    public required NamedExpressionSyntax? NewName { get; init; }

    public required SyntaxToken? EqualsToken { get; init; }

    public required NamedExpressionSyntax OldName { get; init; }
    public required SyntaxToken SemicolonToken { get; init; }
}