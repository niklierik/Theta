using Theta.Language.Messages;

namespace Theta.Language.Evaluation;

public sealed class CompileResult
{

    public CompileResult()
    {
    }

    public bool HasResult => Value is not null;

    public required object? Value { get; init; }
    public required Compilation Compilation { get; init; }
}