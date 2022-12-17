using System.Collections;
using Theta.CodeAnalysis.Binding;
using Theta.CodeAnalysis.Syntax;
using static System.Net.Mime.MediaTypeNames;

namespace Theta.CodeAnalysis.Diagnostics;

public sealed class DiagnosticBag : IEnumerable<Diagnostic>
{
    public List<Diagnostic> Diagnostics { get; private set; } = new();

    public void InsertAll(DiagnosticBag other)
    {
        Diagnostics.AddRange(other.Diagnostics);
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

    public void ReportUnexpectedNull()
    {
        Report(new(), "Unexpected null literal as an operand for unary expression.");
    }

    public void ReportUndefinedUnaryBehaviour(BoundUnaryExpression unary)
    {
        Report(new(), $"Unary operator {unary.Operator.Type} has undefined behaviour.");
    }

    public void ReportBinderError(TextSpan span = new())
    {
        Report(span, $"Cannot bind binary expression.");
    }

    public void ReportInvalidBinaryExpression(BinaryExpressionSyntax binary, BoundExpression left, BoundExpression right, TextSpan span = new())
    {
        Report(span, $"Binary expression does not exist for {binary.Operator.Type} with operands {left.Type} and {right.Type}.");
    }

    public void ReportInvalidUnaryExpression(UnaryExpressionSyntax unary, BoundExpression boundOperand, TextSpan span = new())
    {
        Report(span, $"Unary operator does not exist for {unary.Operator.Type} with operand type {boundOperand.Type}.");
    }

    public void ReportUndefinedBinaryBehaviour(BoundBinaryExpression binary, TextSpan span = new())
    {
        Report(span, $"Binary operator {binary.Operator.Type} has undefined behaviour.");
    }

    public void ReportInvalidExpression(TextSpan span = new())
    {
        Report(span, "Cannot parse expression.");
    }

    public void ReportException(Exception ex, TextSpan span = new(), MessageType type = MessageType.Error)
    {
        Report(span, ex.ToString(), type);
    }

    public void ForEach(Action<Diagnostic> writeLine)
    {
        foreach (var d in this)
        {
            writeLine?.Invoke(d);
        }
    }

    public void ReportAll()
    {
        ReportAll(Console.WriteLine);
    }

    public void ReportAll(Action<string>? write)
    {
        if (write is null)
        {
            return;
        }
        foreach (var d in this)
        {
            Console.ForegroundColor = d.MessageType.GetColor();
            write(d.ToString());
        }
        Console.ResetColor();
    }

    public void ReportUndefinedName(string name, TextSpan span)
    {
        Report(span, $"Undefined object with name '{name}'.", MessageType.Warning);
    }
}
