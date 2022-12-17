using Theta.CodeAnalysis.Diagnostics;

namespace Theta.CodeAnalysis.Evaluation;

public sealed class EvaluationResult
{

    public EvaluationResult(DiagnosticBag diagnostics, object? value)
    {
        Diagnostics = diagnostics;
        Value = value;
    }

    public bool HasResult => Value is not null;

    public DiagnosticBag Diagnostics { get; }
    public object? Value { get; }
}