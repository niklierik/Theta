namespace Theta.CodeAnalysis.Syntax;

using System.Collections;
using System.Collections.Generic;
using Theta.CodeAnalysis.Messages;
using Theta.CodeAnalysis.Text;

public sealed class SyntaxTree
{
    public SyntaxNode Root { get; }

    public SyntaxToken? EOF { get; init; } = null;

    public SourceText Src { get; }


    public static SyntaxTree Parse(string text)
    {
        return Parse(SourceText.From(text));
    }

    public static SyntaxTree Parse(SourceText text)
    {
        return new(text);
    }

    private SyntaxTree(SourceText text)
    {
        var parser = new Parser(text);
        var root = parser.ParseCompilationUnit();
        Src = text;
        Root = root.Root;
        EOF = root.EOF;
    }


}
