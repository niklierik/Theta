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
using System.Text.RegularExpressions;
using Theta.Transpilers;

internal static class Program
{


    // private static bool _printTree = false;
    private static bool _multiline = false;
    // private static Dictionary<VariableSymbol, object?> _vars = new();
    // private static Compilation? _prev = null;

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
            /*
             if (arg.ToLower() == "-printtree")
             {
                 _printTree = true;
             }
            */
        }
    }

    public static void Main(string[] args)
    {
        ConsoleSetup();
        ConsumeArgs(args);
        using (var transpiler = new CPPTranspiler("test.h", "test.cpp"))
        {
            " > ".Log(ConsoleColor.DarkGray, false);

            SourceText? input = SourceText.FromConsole(_multiline);
            try
            {
                if (input.IsEmpty)
                {
                    return;
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
                    return;
                }
                /*
                if (inputtxt == "#printtree()")
                {
                    _printTree = !_printTree;
                    Console.Write("Printing tree: ");
                    _printTree.OnOff().Log(_printTree.GoodBadColor());
                    continue;
                }
                */
                if (inputtxt == "#clear()")
                {
                    Console.Clear();
                    return;
                }
                /*
                if (inputtxt == "#clearvars()")
                {
                    _vars.Clear();
                    continue;
                }
                */
                (input, var success) = TryOpenFile(input);
                #endregion
                if (success)
                {
                    "Opening file...".Log(ConsoleColor.DarkGray);
                }
                if (input is null)
                {
                    return;
                }
                // var result = Compilation.EvalLine(input, _vars, _printTree, _prev);
                Compilation.CompileText(input, transpiler);
                Diagnostics.ShowErrors();

                if (Diagnostics.HasError)
                {
                    Diagnostics.Clear();
                    return;
                }

                // $"{eval.AsStringVersion()}".Log(ConsoleColor.DarkGray);
            }
            catch (HasErrorException)
            {
                Diagnostics.ShowErrors();
            }
            Diagnostics.Clear();
            transpiler.Dispose();
            $"Transpiling was successful.".Log(ConsoleColor.Green);
        }
    }

    private static (SourceText? input, bool success) TryOpenFile(SourceText? input)
    {
        if (input is null)
        {
            return (null, false);
        }
        var regex = new Regex(@"^#file\((?<file>.*)\)$");
        var match = regex.Match(input.ToString());
        if (match.Success)
        {
            var file = match.Groups["file"].Value;
            if (!file.ToLower().EndsWith(".th"))
            {
                $"WARNING: You should only open '.th' files.{Environment.NewLine}{Environment.NewLine}".Log(ConsoleColor.Yellow);
            }
            return (SourceText.FromFile(file), true);
        }
        return (input, false);
    }
}