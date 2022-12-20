namespace Theta.CodeAnalysis.Syntax;

using System.Collections;
using System.Collections.Generic;
using Theta.CodeAnalysis.Messages;
using Theta.CodeAnalysis.Text;

public sealed class SyntaxTree
{
    public required SyntaxNode Root { get; init; }

    public SyntaxToken? EOF { get; init; } = null;

    public required SourceText Src { get; init; }


    public static SyntaxTree Parse(string text)
    {
        return Parse(SourceText.From(text));
    }

    public static SyntaxTree Parse(SourceText text)
    {
        var parser = new Parser(text);
        return parser.Parse();
    }

    public SyntaxTree()
    {
    }


}
