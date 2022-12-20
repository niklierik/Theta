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
        var vars = new Dictionary<VariableSymbol, object?>();
        while (true)
        {
            var diagnostics = new DiagnosticBag();
            " > ".Log(ConsoleColor.DarkGray, false);

            var input = SourceText.From(Console.ReadLine() ?? "");
            if (input.ToLower() == "#exit()")
            {
                return;
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