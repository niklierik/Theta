using System.Collections;
using Theta.CodeAnalysis.Binding;
using Theta.CodeAnalysis.Syntax;
using Theta.CodeAnalysis.Text;

namespace Theta.CodeAnalysis.Messages;

public sealed class Diagnostics : IEnumerable<Diagnostic>
{

    private static volatile Diagnostics? _instance = null;
    private static object syncRoot = new object();

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

    public static void ShowErrors(SourceText input)
    {
        ReportAll(input);
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

    public static void ReportAll(SourceText input)
    {
        ReportAll(Console.Write, input);
    }

    public static void ReportAll(Action<string>? write, SourceText input)
    {
        if (write is null)
        {
            return;
        }
        foreach (var d in Instance)
        {
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
            WriteWrongLine(write, input, d, d.Span);
            write(Environment.NewLine);
            write(Environment.NewLine);
        }
        Console.ResetColor();
    }

    public static void Clear()
    {
        Instance.Messages.Clear();
    }

    private static void WriteWrongLine(Action<string> write, SourceText input, Diagnostic diagnostic, TextSpan span)
    {
        for (int i = span.Start; i < span.Start + span.Length; i++)
        {
            Console.ForegroundColor = (diagnostic.Span.In(i)) ? diagnostic.MessageType.GetColor() : ConsoleColor.Gray;
            write(input[i].ToString());
        }
        Console.ResetColor();
    }

    public static bool HasError => Instance.Messages.Any(d => d.MessageType == MessageType.Error);

    public IEnumerator<Diagnostic> GetEnumerator() => Messages.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private static void Report(TextSpan span, string message, MessageType type = MessageType.Error)
    {
        Instance.Messages.Add(new(span, message, type));
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

    public static void ReportUnexpectedToken(TextSpan span, SyntaxType currentType, SyntaxType expected)
    {
        Report(span, $"Unexpected token <{currentType}> expected <{expected}>.");
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

    public static void ReportUndefinedName(string name, TextSpan span)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Report(span, $"Variable name expected.", MessageType.Error);
            return;
        }
        Report(span, $"Undefined object with name '{name}'.", MessageType.Warning);
    }

    public static void ReportInvalidCast(CodeAnalysis.VariableSymbol key, BoundExpression? expression, TextSpan span)
    {
        Report(span, $"Cannot cast {expression?.Type ?? typeof(void)} to {key.Type} for variable {key.Name}.", MessageType.Warning);
    }
}
