namespace Theta;

using System.Globalization;
using System.Text;
using Theta.Parser;
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
            if (printTree)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                PrintTree(expression.Root);
                Console.ResetColor();
            }
            var eval = new Evaluator(expression, expression.Diagnostics);
            if (eval.Diagnostics.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                eval.Diagnostics.ForEach(Console.WriteLine);
                Console.ResetColor();
                continue;
            }
            var result = eval.Evaluate();
            result.Log(ConsoleColor.Green);
        }
    }
}