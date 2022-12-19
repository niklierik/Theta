namespace Theta.CodeAnalysis.Evaluation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.CodeAnalysis.Binding;
using Theta.CodeAnalysis.Diagnostics;
using Theta.CodeAnalysis;
using static Theta.CodeAnalysis.Syntax.Parser;

public sealed class Evaluator
{

    private readonly BoundExpression _tree;

    public DiagnosticBag Diagnostics { get; } = new();
    public Dictionary<VariableSymbol, object?> Vars { get; }

    public Evaluator(BoundExpression tree, Dictionary<VariableSymbol, object?> vars)
    {
        _tree = tree;
        Vars = vars;
    }

    public object? Evaluate()
    {
        return EvaluateExpression(_tree);
    }

    public object? EvaluateExpression(BoundExpression? root)
    {
        try
        {
            if (root is null)
            {
                return null;
            }
            return root.Evaluate(this);
        }
        catch (Exception ex)
        {
            Diagnostics.ReportException(ex, root?.Span ?? new());
            return null;
        }
    }

}