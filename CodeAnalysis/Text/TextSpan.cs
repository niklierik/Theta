namespace Theta.CodeAnalysis.Text;
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
        return ToString(-1);
    }

    public string ToString(int line = -1)
    {
        if (Length == 0)
        {
            return "";
        }
        if (Length < 2)
        {
            return $" at line {line + 1} at character {Start}";
        }
        return $" at line {line + 1} from {Start} to {End}";
    }

    public bool In(int pos)
    {
        return Start <= pos && pos < End;
    }
}