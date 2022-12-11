namespace Theta.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class SyntaxToken : SyntaxNode
{

    public SyntaxToken(SyntaxType _type) : base()
    {
        Type = _type;
    }
    public override SyntaxType Type { get; }
    public required int Position { get; init; }
    public required string Text { get; init; }

    public object? Value { get; init; } = null;

    public override string ToString()
    {
        return $"Pos = {Position}, Type = {Type}, Text = {Text}, Value = {Value ?? "#none"}";
    }

    public override IEnumerable<SyntaxNode> Children => Enumerable.Empty<SyntaxNode>();

}