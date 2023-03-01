namespace Theta.Language.Text;
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

    public string ToString(int start = -1, int end = -1, int offset = -1)
    {
        if (Length == 0)
        {
            return "";
        }
        if (start == -1 || end == -1)
        {

            if (Length < 2)
            {
                return $" at character {Start + 1}";
            }
            return $" from character {Start + 1} to character {End + 1}";
        }
        if (start == end)
        {
            if (Length < 2)
            {
                return $" at line {start + 1} at character {Start + offset + 1}";
            }
            return $" at line {start + 1} from character {Start + offset + 1} to character {End + offset + 1}";
        }
        return $" from line {start + 1} to line {end + 1}";
    }

    public bool In(int pos)
    {
        return Start <= pos && pos < End;
    }
}