namespace Theta.Language.Text;

public sealed class TextLine
{
    public SourceText Text { get; }
    public int Start { get; }
    public int Length { get; }
    public int LengthIncludingLineBreak { get; }
    public TextSpan Span => new(Start, Length);
    public TextSpan SpanIncludingLineBreak => new(Start, LengthIncludingLineBreak);

    public int End => Start + Length;

    public TextLine(SourceText text, int start, int length, int lengthIncludingLineBreak)
    {
        Text = text;
        Start = start;
        Length = length;
        LengthIncludingLineBreak = lengthIncludingLineBreak;
    }


    public override string ToString()
    {
        return Text.ToString(Span);
    }

    public string ToString(int start, int length) => ToString().Substring(start, length);
    public string ToString(TextSpan span) => ToString(span.Start, span.Length);

}