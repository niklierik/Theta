namespace Theta.CodeAnalysis.Diagnostics;
public struct TextSpan
{
    public TextSpan(int start, int length)
    {
        Start = start;
        Length = length;
    }

    public int Start { get; }
    public int Length { get; }

    public int End => Start + Length;

    public static TextSpan FromBounds(int start, int end)
    {
        var length = end - start;
        return new(start, length);
    }

    public override string ToString()
    {
        if (Length == 0)
        {
            return "";
        }
        if (Length < 2)
        {
            return $" at character {Start}";
        }
        return $" from {Start} to {End}";
    }

    public bool In(int pos)
    {
        return Start <= pos && pos < End;
    }
}