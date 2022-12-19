namespace Theta.Tests.CodeAnalysis.Syntax;
using System;
using Theta.CodeAnalysis.Syntax;

public sealed class AssertingEnumerator : IDisposable
{
    public AssertingEnumerator(SyntaxNode node)
    {
        _enumerator = Flatten(node).GetEnumerator();
    }

    private IEnumerator<SyntaxNode> _enumerator;
    private bool _hasErrors = false;

    private static IEnumerable<SyntaxNode> Flatten(SyntaxNode node)
    {
        var stack = new Stack<SyntaxNode>();
        stack.Push(node);
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            yield return n;
            foreach (var child in n.Children.Reverse())
            {
                stack.Push(child);
            }
        }
    }

    public void AssertToken(SyntaxType type, string text)
    {
        _hasErrors = true;
        Assert.True(_enumerator.MoveNext());
        var token = Assert.IsType<SyntaxToken>(_enumerator.Current);
        Assert.Equal(type, _enumerator.Current.Type);
        Assert.Equal(text, token.Text);
        _hasErrors = false;
    }
    public void AssertNode(SyntaxType type)
    {
        _hasErrors = true;
        Assert.True(_enumerator.MoveNext());
        Assert.Equal(type, _enumerator.Current.Type);
        Assert.IsNotType<SyntaxToken>(_enumerator.Current);
        _hasErrors = false;
    }

    public void Dispose()
    {
        if (!_hasErrors)
        {
            Assert.False(_enumerator.MoveNext());
        }
        _enumerator.Dispose();
    }
}
