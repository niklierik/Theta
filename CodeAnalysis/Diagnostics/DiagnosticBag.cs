using System.Collections;
using Theta.CodeAnalysis.Binding;
using Theta.CodeAnalysis.Syntax;
using Theta.CodeAnalysis.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Theta.CodeAnalysis.Diagnostics;

public sealed class DiagnosticBag : IEnumerable<Diagnostic>
{
    public List<Diagnostic> Diagnostics { get; private set; } = new();

    public void InsertAll(DiagnosticBag other)
    {
        Diagnostics.AddRange(other.Diagnostics);
    }

    public void ForEach(Action<Diagnostic> writeLine)
    {
        foreach (var d in this)
        {
            writeLine?.Invoke(d);
        }
    }

    public void ReportAll(SourceText input)
    {
        ReportAll(Console.Write, input);
    }

    public void ReportAll(Action<string>? write, SourceText input)
    {
        if (write is null)
        {
            return;
        }
        foreach (var d in this)
        {
            Console.ForegroundColor = d.MessageType.GetColor();
            int startLineIndex = input.GetLineIndex(d.Span.Start);
            int endLineIndex = input.GetLineIndex(d.Span.Start + d.Span.Length);
            var start = input.Lines[startLineIndex];
            var end = input.Lines[endLineIndex];
            write(d.ToString(startLineIndex, endLineIndex, -start.Start));
            write(Environment.NewLine);
            WriteWrongLine(write, input, d, d.Span);
            write(Environment.NewLine);
            write(Environment.NewLine);
        }
        Console.ResetColor();
    }

    public void WriteWrongLine(Action<string> write, SourceText input, Diagnostic diagnostic, TextSpan span)
    {
        for (int i = span.Start; i < span.Start + span.Length; i++)
        {
            Console.ForegroundColor = (diagnostic.Span.In(i)) ? diagnostic.MessageType.GetColor() : ConsoleColor.Gray;
            write(input[i].ToString());
        }
        Console.ResetColor();
    }

    public bool HasError => Diagnostics.Where(d => d.MessageType == MessageType.Error).Any();

    public IEnumerator<Diagnostic> GetEnumerator() => Diagnostics.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private void Report(TextSpan span, string message, MessageType type = MessageType.Error)
    {
        Diagnostics.Add(new(span, message, type));
    }

    public void ReportInvalidInt64(string text, TextSpan span)
    {
        Report(span, $"{text} cannot be interpreted as an integer.");
    }

    public void ReportInvalidDouble(string text, TextSpan span)
    {
        Report(span, $"{text} cannot be interpreted as a floating number.");
    }

    public void ReportInvalidSyntax(ExpressionSyntax syntax)
    {
        Report(new(), $"Unexpected syntax {syntax.Type}.");
    }

    public void ReportInvalidCharacter(char current, int pos)
    {
        Report(new(pos, 1), $"Invalid character input: {current}.");
    }

    public void ReportUnexpectedToken(TextSpan span, SyntaxType currentType, SyntaxType expected)
    {
        Report(span, $"Unexpected token <{currentType}> expected <{expected}>.");
    }

    public void ReportUnexpectedNull(TextSpan span)
    {
        Report(span, "Unexpected null literal as an operand for unary expression.");
    }

    public void ReportUndefinedUnaryBehaviour(BoundUnaryExpression unary, TextSpan span)
    {
        Report(span, $"Unary operator {unary.Operator.Type} with operand {unary.Operand.Type} has undefined behaviour.");
    }

    public void ReportBinderError(TextSpan span)
    {
        Report(span, $"Cannot bind binary expression.");
    }

    public void ReportInvalidBinaryExpression(BinaryExpressionSyntax binary, BoundExpression left, BoundExpression right, TextSpan span)
    {
        Report(span, $"Binary expression does not exist for {binary.Operator.Type} with operands {left.Type} and {right.Type}.");
    }

    public void ReportInvalidUnaryExpression(UnaryExpressionSyntax unary, BoundExpression boundOperand, TextSpan span)
    {
        Report(span, $"Unary operator does not exist for {unary.Operator.Type} with operand type {boundOperand.Type}.");
    }

    public void ReportUndefinedBinaryBehaviour(BoundBinaryExpression binary, TextSpan span)
    {
        Report(span, $"Binary operator {binary.Operator.Type} with operands {binary.Left.Type} and {binary.Right.Type} has undefined behaviour.");
    }

    public void ReportInvalidExpression(TextSpan span)
    {
        Report(span, "Cannot parse expression.");
    }

    public void ReportException(Exception ex, TextSpan span, MessageType type = MessageType.Error)
    {
        Report(span, ex.ToString(), type);
    }

    public void ReportUndefinedName(string name, TextSpan span)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Report(span, $"Variable name expected.", MessageType.Error);
            return;
        }
        Report(span, $"Undefined object with name '{name}'.", MessageType.Warning);
    }

    public void ReportInvalidCast(CodeAnalysis.VariableSymbol key, BoundExpression? expression, TextSpan span)
    {
        Report(span, $"Cannot cast {expression?.Type ?? typeof(void)} to {key.Type} for variable {key.Name}.", MessageType.Warning);
    }
}
