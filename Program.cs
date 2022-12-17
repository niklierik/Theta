namespace Theta;

using System;
using System.Globalization;
using System.Text;
using Theta.CodeAnalysis.Binding;
using Theta.CodeAnalysis.Diagnostics;
using Theta.CodeAnalysis.Evaluation;
using Theta.CodeAnalysis.Syntax;
using Theta.Utils;

internal static class Program
{


    public static void Main(string[] args)
    {
        Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        bool printTree = false;
        while (true)
        {
            var diagnostics = new DiagnosticBag();
            " > ".Log(ConsoleColor.DarkGray, false);

            var line = Console.ReadLine() ?? "";
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
            var result = Compilation.EvalLine(line, printTree);
            diagnostics.InsertAll(result.Diagnostics);

            if (diagnostics.HasError)
            {
                ShowErrors(diagnostics);
                continue;
            }

            $"   {result.Value ?? "null"}".Log((result?.Value?.GetType() ?? typeof(void)).GetColor());

            // $"{eval.AsStringVersion()}".Log(ConsoleColor.DarkGray);
        }
    }

    private static void ShowErrors(DiagnosticBag diagnostics)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        diagnostics.ForEach(Console.WriteLine);
        Console.ResetColor();
    }
}