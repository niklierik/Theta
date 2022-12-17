namespace Theta.CodeAnalysis.Evaluation;

using Theta.CodeAnalysis.Binding;
using Theta.CodeAnalysis.Diagnostics;
using Theta.CodeAnalysis.Syntax;

public sealed class Compilation
{
    public Compilation(SyntaxTree syntax)
    {
        Syntax = syntax;
    }

    public SyntaxTree Syntax { get; }

    public DiagnosticBag Diagnostics { get; } = new();

    public EvaluationResult Evaluate(Dictionary<string, object> vars)
    {
        var binder = new Binder(vars);
        var boundExpression = binder.BindExpression(Syntax.Root as ExpressionSyntax);
        Diagnostics.InsertAll(binder.Diagnostics);
        if (boundExpression is null || Diagnostics.HasError)
        {
            return new EvaluationResult(Diagnostics, null);
        }
        var eval = new Evaluator(boundExpression, vars);
        var res = eval.Evaluate();
        Diagnostics.InsertAll(eval.Diagnostics);
        if (Diagnostics.HasError)
        {
            return new EvaluationResult(Diagnostics, null);
        }
        return new EvaluationResult(Diagnostics, res);
    }

    public static EvaluationResult EvalLine(string line, Dictionary<string, object> vars, bool printTree = false)
    {
        var expression = SyntaxTree.Parse(line);
        var compilation = new Compilation(expression);
        var result = compilation.Evaluate(vars);
        result.Diagnostics.InsertAll(expression.Diagnostics);
        if (printTree)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            PrintTree(expression.Root);
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
