﻿namespace Theta.Language.Syntax;

using System.Collections;
using System.Collections.Generic;
using Theta.Language.Messages;
using Theta.Language.Text;

public sealed class SyntaxTree
{
    public CompilationUnitSyntax Root { get; }

    public SyntaxToken? EOF { get; init; } = null;

    public SourceText Src { get; }


    public static SyntaxTree Parse(string text)
    {
        return Parse(SourceText.FromText(text));
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
        Root = root;
        EOF = root.EOF;
    }


}
