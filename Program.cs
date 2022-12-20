namespace Theta;

using System;
using System.Globalization;
using System.Text;
using Theta.CodeAnalysis.Binding;
using Theta.CodeAnalysis.Messages;
using Theta.CodeAnalysis.Evaluation;
using Theta.CodeAnalysis.Syntax;
using Theta.CodeAnalysis;
using Theta.CodeAnalysis.Text;

internal static class Program
{


    private static bool printTree = false;
    private static bool multiline = false;
    private static Dictionary<VariableSymbol, object?> vars = new();

    private static void ConsoleSetup()
    {
        Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
    }

    

    private static void ConsumeArgs(string[] args)
    {
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
    }

    public static void Main(string[] args)
    {
        ConsoleSetup();
        ConsumeArgs(args);
        while (true)
        {

            " > ".Log(ConsoleColor.DarkGray, false);

            var input = SourceText.FromConsole(multiline);
            try
            {
                if (input.IsEmpty)
                {
                    continue;
                }

                #region Macros
                var inputtxt = input.ToLower().Trim();
                if (inputtxt == "#exit()")
                {
                    return;
                }
                if (inputtxt == "#multiline()")
                {
                    multiline = !multiline;
                    Console.Write("Multiline input: ");
                    multiline.OnOff().Log(multiline.GoodBadColor());
                    continue;
                }
                if (inputtxt == "#printtree()")
                {
                    printTree = !printTree;
                    Console.Write("Printing tree: ");
                    printTree.OnOff().Log(printTree.GoodBadColor());
                    continue;
                }
                if (inputtxt == "#clear()")
                {
                    Console.Clear();
                    continue;
                }
                if (inputtxt == "#clearvars()")
                {
                    vars.Clear();
                    continue;
                }
                #endregion

                var result = Compilation.EvalLine(input, vars, printTree);
                Diagnostics.ShowErrors(input);

                if (Diagnostics.HasError)
                {
                    continue;
                }

                $"   {result.Value ?? "null"}".Log((result?.Value?.GetType() ?? typeof(void)).GetColor());

                // $"{eval.AsStringVersion()}".Log(ConsoleColor.DarkGray);
            }
            catch (HasErrorException)
            {
                Diagnostics.ShowErrors(input);
            }
            Diagnostics.Clear();
        }
    }




}