using Theta.CodeAnalysis.Text;

namespace Theta.CodeAnalysis.Messages;

public sealed class Diagnostic
{
    public Diagnostic(TextSpan span, string message, SourceText source, MessageType type = MessageType.Error)
    {
        Span = span;
        Message = message;
        Source = source;
        MessageType = type;
    }

    public TextSpan Span { get; }
    public string Message { get; }
    public SourceText Source { get; }
    public MessageType MessageType { get; }

    public string Input { get; set; } = string.Empty;

    public string ToString(int start, int end, int offset)
    {
        return $"""
            {MessageType.GetPrefix()}
            {Message}
            {Span.ToString(start, end, offset)}
            """;
    }
}
