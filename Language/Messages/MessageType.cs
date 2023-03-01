namespace Theta.Language.Messages;

public enum MessageType
{
    Info,
    Warning,
    Error
}

public static class MessageTypeHelper
{

    public static ConsoleColor GetColor(this MessageType messageType)
    {
        switch (messageType)
        {
            case MessageType.Info:
                return ConsoleColor.Cyan;
            case MessageType.Warning:
                return ConsoleColor.Yellow;
            case MessageType.Error:
                return ConsoleColor.Red;
        }
        return ConsoleColor.Gray;
    }

    public static string GetPrefix(this MessageType messageType)
    {
        switch (messageType)
        {
            case MessageType.Info:
                return "INFO: ";
            case MessageType.Warning:
                return "WARNING: ";
            case MessageType.Error:
                return "ERROR: ";
        }
        return "";
    }
}