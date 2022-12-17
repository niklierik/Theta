﻿namespace Theta.CodeAnalysis.Syntax;

using System.Collections;
using System.Collections.Generic;
using Theta.CodeAnalysis.Diagnostics;

public sealed class SyntaxTree : IEnumerable<SyntaxNode>
{
    public required SyntaxNode Root { get; init; }

    public SyntaxToken? EOF { get; init; } = null;

    public DiagnosticBag Diagnostics { get; private set; } = new();

    public SyntaxTree()
    {

    }

    public static SyntaxTree Parse(string text)
    {
        var parser = new Parser(text);
        return parser.Parse();
    }

    public SyntaxTree(DiagnosticBag diagnostics) : this()
    {
        Diagnostics.InsertAll(diagnostics);
    }

    public IEnumerator<SyntaxNode> GetEnumerator()
    {
        var tokens = new List<SyntaxNode>();
        foreach (var token in Root.Children)
        {
            tokens.AddRange(new SyntaxTree() { Root = token, EOF = null });
        }
        return tokens.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
