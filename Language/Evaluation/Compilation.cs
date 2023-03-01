namespace Theta.Language.Evaluation;

using Theta.Language.Binding;
using Theta.Language.Messages;
using Theta.Language.Syntax;
using Theta.Language;
using Theta.Language.Text;
using System;
using Theta.Transpilers;
using Theta.Language.Objects;

public sealed class Compilation
{
    private BoundGlobalScope? _global;
    public Compilation(SyntaxTree syntax) : this(null, syntax)
    {
    }

    private Compilation(Compilation? prev, SyntaxTree tree)
    {
        Prev = prev;
        Syntax = tree;
    }

    public SyntaxTree Syntax { get; }

    public BoundGlobalScope GlobalScope
    {
        get
        {
            if (_global is null)
            {
                var (global, scope) = Binder.BindGlobalScope(Prev?.GlobalScope ?? null, Syntax.Root);
                Interlocked.CompareExchange(ref _global, global, null);
            }
            return _global!;
        }
    }

    public Compilation? Prev { get; }

    public Compilation ContinueWith(SyntaxTree next)
    {
        return new(this, next);
    }

    public void Compile(Transpiler transpiler)
    {
        var globalScope = GlobalScope;
        transpiler.Global = globalScope;
        transpiler.GlobalScope = null;
        if (globalScope?.Statements is null || Diagnostics.HasError)
        {
            return;
        }
        transpiler.Transpile(globalScope.Statements);
    }

    public void Prepare(Registry registry)
    {
        var globalScope = GlobalScope;
        if (globalScope?.Statements is null || Diagnostics.HasError)
        {
            return;
        }
        registry.Register(GlobalScope.Statements);
    }

    public static void Prepare(SourceText input, Registry registry)
    {
        CompilerFor(input).Prepare(registry);
    }

    public static void CompileText(string line, Transpiler transpiler)
    {
        CompileText(SourceText.FromText(line), transpiler);
    }


    public static void CompileText(SourceText input, Transpiler transpiler)
    {
        CompilerFor(input).Compile(transpiler);
    }

    public static void PrintTree(SyntaxNode node, string indent = "", bool isLast = true)
    {
        var marker = isLast ? "└──" : "├──";

        Console.Write(indent);
        Console.Write(marker);
        Console.Write(node.Type);

        if (node is SyntaxToken t && t.Value != null)
        {
            Console.Write(" ");
            Console.Write(t.Value);
        }

        Console.WriteLine();

        indent += isLast ? "   " : "│  ";

        var lastChild = node.Children.LastOrDefault();

        foreach (var child in node.Children)
        {
            PrintTree(child, indent, child == lastChild);
        }
    }

    public static Compilation CompilerFor(SourceText input)
    {
        Diagnostics.Instance.Input = input;
        var syntaxTree = SyntaxTree.Parse(input);
        var compilation = new Compilation(syntaxTree);
        return compilation;
    }
}
