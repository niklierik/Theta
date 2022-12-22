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


    private static bool _printTree = false;
    private static bool _multiline = false;
    private static Dictionary<VariableSymbol, object?> _vars = new();
    private static Compilation _prev = null;

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
                _multiline = true;
            }
            if (arg.ToLower() == "-printtree")
            {
                _printTree = true;
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

            var input = SourceText.FromConsole(_multiline);
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
                    _multiline = !_multiline;
                    Console.Write("Multiline input: ");
                    _multiline.OnOff().Log(_multiline.GoodBadColor());
                    continue;
                }
                if (inputtxt == "#printtree()")
                {
                    _printTree = !_printTree;
                    Console.Write("Printing tree: ");
                    _printTree.OnOff().Log(_printTree.GoodBadColor());
                    continue;
                }
                if (inputtxt == "#clear()")
                {
                    Console.Clear();
                    continue;
                }
                if (inputtxt == "#clearvars()")
                {
                    _vars.Clear();
                    continue;
                }
                #endregion

                var result = Compilation.EvalLine(input, _vars, _printTree, _prev);
                Diagnostics.ShowErrors(input);

                if (Diagnostics.HasError)
                {
                    Diagnostics.Clear();
                    continue;
                }
                _prev = result.Compilation;

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