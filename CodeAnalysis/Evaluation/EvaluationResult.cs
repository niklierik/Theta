using Theta.CodeAnalysis.Messages;

namespace Theta.CodeAnalysis.Evaluation;

public sealed class EvaluationResult
{

    public EvaluationResult()
    {
    }

    public bool HasResult => Value is not null;

    public required object? Value { get; init; }
    public required Compilation Compilation { get; init; }
}