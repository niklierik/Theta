using Theta.CodeAnalysis.Messages;

namespace Theta.CodeAnalysis.Evaluation;

public sealed class CompileResult
{

    public CompileResult()
    {
    }

    public bool HasResult => Value is not null;

    public required object? Value { get; init; }
    public required Compilation Compilation { get; init; }
}