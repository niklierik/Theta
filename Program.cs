namespace Theta;

using System;
using System.Globalization;
using System.Text;
using Theta.CodeAnalysis.Binding;
using Theta.CodeAnalysis.Diagnostics;
using Theta.CodeAnalysis.Evaluation;
using Theta.CodeAnalysis.Syntax;
using Theta.CodeAnalysis;
using Theta.CodeAnalysis.Text;

internal static class Program
{


    public static void Main(string[] args)
    {
        Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        bool printTree = false;
        bool multiline = false;
        foreach (var arg in args)
        {
            if (arg.ToLower() == "-multiline")
            {
                multiline = true;
            }
            if (arg.ToLower() == "-printtree")
            {
                printTree = true;
            }
        }
        var vars = new Dictionary<VariableSymbol, object?>();
        while (true)
        {
            var diagnostics = new DiagnosticBag();
            " > ".Log(ConsoleColor.DarkGray, false);

            var input = SourceText.FromConsole(multiline);
            if (input.IsEmpty)
            {
                continue;
            }
            if (input.ToLower() == "#exit()")
            {
                return;
            }
            if (input.ToLower() == "#multiline()")
            {
                multiline = !multiline;
                Console.Write("Multiline input: ");
                multiline.OnOff().Log(multiline.GoodBadColor());
                continue;
            }
            if (input.ToLower() == "#printtree()")
            {
                printTree = !printTree;
                Console.Write("Printing tree: ");
                printTree.OnOff().Log(printTree.GoodBadColor());
                continue;
            }
            if (input.ToLower() == "#clear()")
            {
                Console.Clear();
                continue;
            }
            var result = Compilation.EvalLine(input, vars, printTree);
            diagnostics.InsertAll(result.Diagnostics);
            ShowErrors(diagnostics, input);
            if (diagnostics.HasError)
            {
                continue;
            }

            $"   {result.Value ?? "null"}".Log((result?.Value?.GetType() ?? typeof(void)).GetColor());

            // $"{eval.AsStringVersion()}".Log(ConsoleColor.DarkGray);
        }
    }

    private static void ShowErrors(DiagnosticBag diagnostics, SourceText input)
    {
        diagnostics.ReportAll(input);
    }
}