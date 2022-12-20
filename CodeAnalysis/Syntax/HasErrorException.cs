
namespace Theta.CodeAnalysis.Syntax;
using System.Runtime.Serialization;



/// <summary>
/// It's used for quitting execution line if an error occurred that may invalidate the continuation of the execution.
/// </summary>
[Serializable]
internal class HasErrorException : Exception
{
    public HasErrorException()
    {
    }

    public HasErrorException(string? message) : base(message)
    {
    }

    public HasErrorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected HasErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}