using System;
using System.Collections;
using Theta.Language.Binding;
using Theta.Language.Objects.Types;
using Theta.Language.Syntax;
using Theta.Language.Text;

namespace Theta.Language.Messages;

public sealed class Diagnostics : IEnumerable<Diagnostic>
{

    private static volatile Diagnostics? _instance = null;
    private static readonly object syncRoot = new();

    private Diagnostics() { }

    public static Diagnostics Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (syncRoot)
                {
                    if (_instance is null)
                    {
                        _instance = new();
                    }
                }
            }
            return _instance;
        }
    }

    public static void ShowErrors()
    {
        ReportAll();
    }

    public List<Diagnostic> Messages { get; private set; } = new();

    /*
    public void InsertAll(DiagnosticBag other)
    {
        Diagnostics.AddRange(other.Diagnostics);
    }
    */

    public static void ForEach(Action<Diagnostic> writeLine)
    {
        foreach (var d in Instance)
        {
            writeLine?.Invoke(d);
        }
    }

    public static void ReportAll()
    {
        ReportAll(Console.Write);
    }

    public static void ReportAll(Action<string>? write)
    {
        if (write is null)
        {
            return;
        }
        foreach (var d in Instance)
        {
            var input = d.Source;
            Console.ForegroundColor = d.MessageType.GetColor();
            int startLineIndex = input.GetLineIndex(d.Span.Start);
            int endLineIndex = input.GetLineIndex(d.Span.Start + d.Span.Length);

            TextLine? start = null;
            if (startLineIndex > -1)
            {
                start = input.Lines[startLineIndex];
            }
            TextLine? end = null;
            if (endLineIndex > -1)
            {
                end = input.Lines[endLineIndex];
            }
            write(d.ToString(startLineIndex, endLineIndex, -start?.Start ?? 0));
            write(Environment.NewLine);
            WriteWrongLine(write, input, d, GetLines(input, startLineIndex, endLineIndex).ToList());
            write(Environment.NewLine);
            write(Environment.NewLine);
        }
        Console.ResetColor();
    }

    private static IEnumerable<TextLine> GetLines(SourceText input, int startLineIndex, int endLineIndex)
    {
        var start = Math.Max(0, startLineIndex);
        var end = Math.Min(endLineIndex, input.Lines.Count - 1);
        for (int i = start; i < end; i++)
        {
            yield return input.Lines[i];
        }
    }

    public static void Clear()
    {
        Instance.Messages.Clear();
    }

    private static void WriteWrongLine(Action<string> write, SourceText input, Diagnostic diagnostic, IEnumerable<TextLine> including)
    {
        foreach (var line in including)
        {
            for (int i = line.Start; i < line.Start + line.LengthIncludingLineBreak; i++)
            {
                Console.ForegroundColor = (diagnostic.Span.In(i)) ? diagnostic.MessageType.GetColor() : ConsoleColor.Gray;
                write(input[i].ToString());
            }
        }
        Console.ResetColor();
    }

    public static bool HasError => Instance.Messages.Any(d => d.MessageType == MessageType.Error);

    public IEnumerator<Diagnostic> GetEnumerator() => Messages.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public SourceText? Input { get; set; } = null;

    private static void Report(TextSpan span, string message, MessageType type = MessageType.Error)
    {
        Instance.Messages.Add(new(span, message, Instance.Input!, type));
    }

    public static void ReportInvalidInt64(string text, TextSpan span)
    {
        Report(span, $"{text} cannot be interpreted as an integer.");
    }

    public static void ReportInvalidDouble(string text, TextSpan span)
    {
        Report(span, $"{text} cannot be interpreted as a floating number.");
    }

    public static void ReportInvalidSyntax(ExpressionSyntax syntax)
    {
        Report(new(), $"Unexpected syntax {syntax.Type}.");
    }

    public static void ReportInvalidCharacter(char current, int pos)
    {
        Report(new(pos, 1), $"Invalid character input: {current}.");
    }

    public static void ReportUnexpectedToken(TextSpan span, SyntaxType currentType, SyntaxType[] expected)
    {
        Report(span, $"Unexpected token <{currentType}> expected <{string.Join("or", expected)}>.");
    }

    public static void ReportUnexpectedNull(TextSpan span)
    {
        Report(span, "Unexpected null literal as an operand for unary expression.");
    }

    public static void ReportUndefinedUnaryBehaviour(BoundUnaryExpression unary, TextSpan span)
    {
        Report(span, $"Unary operator {unary.Operator.Type} with operand {unary.Operand.Type} has undefined behaviour.");
    }

    public static void ReportBinderError(TextSpan span)
    {
        Report(span, $"Cannot bind binary expression.");
    }

    public static void ReportInvalidBinaryExpression(BinaryExpressionSyntax binary, BoundExpression left, BoundExpression right, TextSpan span)
    {
        Report(span, $"Binary expression does not exist for {binary.Operator.Type} with operands {left.Type} and {right.Type}.");
    }

    public static void ReportInvalidUnaryExpression(UnaryExpressionSyntax unary, BoundExpression boundOperand, TextSpan span)
    {
        Report(span, $"Unary operator does not exist for {unary.Operator.Type} with operand type {boundOperand.Type}.");
    }

    public static void ReportUndefinedBinaryBehaviour(BoundBinaryExpression binary, TextSpan span)
    {
        Report(span, $"Binary operator {binary.Operator.Type} with operands {binary.Left.Type} and {binary.Right.Type} has undefined behaviour.");
    }

    public static void ReportInvalidExpression(TextSpan span)
    {
        Report(span, "Cannot parse expression.");
    }

    public static void ReportException(Exception ex, TextSpan span, MessageType type = MessageType.Error)
    {
        Report(span, ex.ToString(), type);
    }

    public static void ReportUndefinedVariable(string name, TextSpan span)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Report(span, $"Variable name expected.", MessageType.Error);
            return;
        }
        Report(span, $"Undefined variable with name '{name}'.");
    }

    public static void ReportInvalidCast(Language.VariableSymbol? key, BoundExpression? expression, TextSpan span)
    {
        Report(span, $"Cannot cast {expression?.Type ?? Types.Object} to {key?.Type ?? Types.Object} for variable {key?.Name ?? "unkown"}.", MessageType.Error);
    }

    public static void ReportVarAlreadyDeclared(string var, TextSpan span)
    {
        Report(span, $"Variable '{var} is already declared.'");
    }

    public static void ReportInvalidName(SyntaxToken? token)
    {
        if (token is null)
        {
            Report(new(0, 0), "Expected identifier.");
            return;
        }
        Report(token.Span, $"Identifier {token.Text} cannot be used as it is used for {token.Type}.");
    }

    public static void ReportInvalidStringLiteral(string chr, TextSpan span)
    {
        Report(span, "String literal is invalid: " + chr + ".");
    }

    public static void ReportCharLiteralsSizeMustBeOne(string chr, TextSpan span)
    {
        Report(span, "Char literal's length must be 1, but it is not: " + chr + ".");
    }

    public static void ReportInvalidAliasStatement(TextSpan span)
    {
        Report(span, "Wrong syntax for alias statement. Use 'alias <old-name>;' or 'alias <new-name> = <old-name>;'");
    }
}
