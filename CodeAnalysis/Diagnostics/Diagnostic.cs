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

    public override string ToString()
    {
        return MessageType.Message(this);
    }
}
