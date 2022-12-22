namespace Theta.CodeAnalysis.Evaluation;

using Theta.CodeAnalysis.Binding;
using Theta.CodeAnalysis.Messages;
using Theta.CodeAnalysis.Syntax;
using Theta.CodeAnalysis;
using Theta.CodeAnalysis.Text;

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

    public EvaluationResult Evaluate(Dictionary<VariableSymbol, object?> vars)
    {
        var globalScope = GlobalScope;

        if (globalScope?.Expression is null || Diagnostics.HasError)
        {
            return new EvaluationResult { Value = null , Compilation = this };
        }
        var eval = new Evaluator(globalScope.Expression, vars);
        var res = eval.Evaluate();
        if (Diagnostics.HasError)
        {
            return new EvaluationResult { Value = null, Compilation = this };
        }
        return new EvaluationResult { Value = res, Compilation = this };
    }

    public static EvaluationResult EvalLine(string line, Dictionary<VariableSymbol, object?> vars, bool printTree = false, Compilation? _prev = null)
    {
        return EvalLine(SourceText.From(line), vars, printTree, _prev);
    }

    public static EvaluationResult EvalLine(SourceText line, Dictionary<VariableSymbol, object?> vars, bool printTree = false, Compilation? _prev = null)
    {
        var syntaxTree = SyntaxTree.Parse(line);
        var compilation = _prev is null ? new Compilation(syntaxTree) : _prev.ContinueWith(syntaxTree);
        var result = compilation.Evaluate(vars);
        if (printTree)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            PrintTree(syntaxTree.Root);
            Console.ResetColor();
        }
        return result;
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
