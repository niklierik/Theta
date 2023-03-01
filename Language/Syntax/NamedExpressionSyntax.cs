namespace Theta.Language.Syntax;

public sealed class NamedExpressionSyntax : ExpressionSyntax
{
    public override SyntaxType Type => SyntaxType.NameExpression;

    public SyntaxToken[] NameParts { get; }

    public string FullName => string.Join("", NameParts.Select(part => part.Text));

    public string NameSpace
    {
        get
        {
            if (NameParts.Length <= 2)
            {
                return "";
            }
            return string.Join("", NameParts[0..^2].Select(part => part.Text));
        }
    }

    public string LastPart => NameParts.LastOrDefault()?.Text ?? "";

    public NamedExpressionSyntax(params SyntaxToken[] nameparts)
    {
        NameParts = nameparts;
    }

    public override List<SyntaxNode> Children => new(NameParts.ToList());
}
