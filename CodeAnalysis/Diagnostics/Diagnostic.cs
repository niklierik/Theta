using Theta.CodeAnalysis.Text;

namespace Theta.CodeAnalysis.Diagnostics;

public sealed class Diagnostic
{
    public Diagnostic(TextSpan span, string message, MessageType type = MessageType.Error)
    {
        Span = span;
        Message = message;
        MessageType = type;
    }

    public TextSpan Span { get; }
    public string Message { get; }

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
