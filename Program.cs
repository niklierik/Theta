namespace Theta;

using System;
using System.Globalization;
using System.Text;
using Theta.CodeAnalysis;
using Theta.CodeAnalysis.Binding;
using Theta.CodeAnalysis.Syntax;
using Theta.Utils;

internal static class Program
{
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

        indent += isLast ? "   " : "│   ";

        var lastChild = node.Children.LastOrDefault();

        foreach (var child in node.Children)
        {
            PrintTree(child, indent, child == lastChild);
        }
    }

    public static void Main(string[] args)
    {
        var diagnostics = new List<string>();
        Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        string line = "";
        bool printTree = false;
        while (true)
        {
            " > ".Log(ConsoleColor.DarkGray, false);

            line = Console.ReadLine() ?? "";
            if (line.ToLower() == "#exit()")
            {
                return;
            }
            if (line.ToLower() == "#printtree()")
            {
                printTree = !printTree;
                Console.Write("Printing tree: ");
                printTree.OnOff().Log(printTree.GoodBadColor());
                continue;
            }
            if (line.ToLower() == "#clear()")
            {
                Console.Clear();
                continue;
            }
            var expression = SyntaxTree.Parse(line);
            var binder = new Binder();
            var boundExpression = binder.BindExpression(expression?.Root as ExpressionSyntax);
            if (expression is null)
            {
                ShowErrors(diagnostics);
                continue;
            }
            diagnostics.AddRange(expression.Diagnostics);
            diagnostics.AddRange(binder.Diagnostics);
            if (printTree)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                PrintTree(expression.Root);
                Console.ResetColor();
            }
            if (boundExpression is null)
            {
                ShowErrors(diagnostics);
                continue;
            }
            var eval = new Evaluator(boundExpression);
            diagnostics.AddRange(eval.Diagnostics);
            if (diagnostics.Count > 0)
            {
                ShowErrors(diagnostics);
                continue;
            }
            var result = eval.Evaluate();
            $"   {result}".Log(ConsoleColor.Green);
            // $"{eval.AsStringVersion()}".Log(ConsoleColor.DarkGray);
        }
    }

    private static void ShowErrors(List<string> diagnostics)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        diagnostics.ForEach(Console.WriteLine);
        Console.ResetColor();
    }
}