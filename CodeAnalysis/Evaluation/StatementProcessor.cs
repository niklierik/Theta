namespace Theta.CodeAnalysis.Evaluation;

using System;
using Theta.CodeAnalysis.Binding;
using Theta.CodeAnalysis.Messages;
using Theta.Transpilers;

public sealed partial class StatementProcessor
{

    // private readonly BoundExpression _tree;
    private readonly BoundStatement _statement;
    private readonly Transpiler _transpiler;

    public StatementProcessor(BoundStatement statement, Transpiler transpiler)
    {
        _statement = statement;
        _transpiler = transpiler;
    }

    public void Transpile()
    {
        TranspileStatement(_statement);
    }

    public void TranspileStatement(BoundStatement? statement, int indentation = 0)
    {
        if (statement is null)
        {
            return;
        }
        statement.Transpile(_transpiler, indentation);
    }

    public string StringifyExpression(BoundExpression? expression)
    {
        if (expression is null)
        {
            return "";
        }
        return expression.Stringify(this);
    }

    /*
    public object? EvaluateExpression(BoundExpression? expression)
    {
        try
        {
            if (expression is null)
            {
                return null;
            }
            return expression.Evaluate(this);
        }
        catch (Exception ex)
        {
            Diagnostics.ReportException(ex, expression?.Span ?? new());
            return null;
        }
    }*/

}