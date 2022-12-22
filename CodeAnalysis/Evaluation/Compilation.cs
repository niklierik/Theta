namespace Theta.CodeAnalysis.Evaluation;

using Theta.CodeAnalysis.Binding;
using Theta.CodeAnalysis.Messages;
using Theta.CodeAnalysis.Syntax;
using Theta.CodeAnalysis;
using Theta.CodeAnalysis.Text;
using System;
using Theta.Transpilers;

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
                var global = Binder.BindGlobalScope(Prev?.GlobalScope ?? null, Syntax.Root);
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

        if (globalScope?.Statement is null || Diagnostics.HasError)
        {
            return; //new CompileResult { Value = null, Compilation = this };
        }
        var eval = new StatementProcessor(globalScope.Statement, transpiler);
        eval.Transpile();
        /*if (Diagnostics.HasError)
        {
          //  return new CompileResult { Value = null, Compilation = this };
        }
        // return new CompileResult { Value = res, Compilation = this };
        */
    }

    /*
    [Obsolete("Use normal evaluator", true)]
    public CompileResult Evaluate(Dictionary<VariableSymbol, object?> vars)
    {
        var globalScope = GlobalScope;

        if (globalScope?.Statement is null || Diagnostics.HasError)
        {
            return new CompileResult { Value = null, Compilation = this };
        }
        var eval = new StatementProcessor(globalScope.Statement, vars);
        var res = eval.Evaluate();
        if (Diagnostics.HasError)
        {
            return new CompileResult { Value = null, Compilation = this };
        }
        return new CompileResult { Value = res, Compilation = this };
    }*/

    public static void CompileText(string line, Transpiler transpiler)
    {
        CompileText(SourceText.FromText(line), transpiler);
    }


    public static void CompileText(SourceText line, Transpiler transpiler)
    {
        var syntaxTree = SyntaxTree.Parse(line);
        var compilation = new Compilation(syntaxTree);
        // var result =
        compilation.Compile(transpiler);
        /*if (printTree)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            PrintTree(syntaxTree.Root);
            Console.ResetColor();
        }*/
        // return result;
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
}
