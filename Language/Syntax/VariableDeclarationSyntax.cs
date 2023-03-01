namespace Theta.Language.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.Language.Messages;

public sealed class VariableDeclarationSyntax : ExpressionSyntax
{

    public VariableDeclarationSyntax(bool isConst, SyntaxToken name, SyntaxToken? equals, ExpressionSyntax? equalsTo)
    {
        IsConst = isConst;
        Name = name;
        EqualsToken = equals;
        EqualsTo = equalsTo;
        if (equals is not null != equalsTo is not null)
        {
            throw new NullReferenceException(nameof(equals) + " should be null if " + nameof(equalsTo) + " is null too and vice versa.");
        }
    }

    public override SyntaxType Type => SyntaxType.VariableDeclarationExpression;

    public override IEnumerable<SyntaxNode> Children
    {
        get
        {
            yield return Name;
            if (EqualsToken is not null)
            {
                yield return EqualsToken;
            }
            if (EqualsTo is not null)
            {
                yield return EqualsTo;
            }
        }
    }

    public bool IsConst { get; }
    public SyntaxToken Name { get; }
    public SyntaxToken? EqualsToken { get; }
    public ExpressionSyntax? EqualsTo { get; }
}