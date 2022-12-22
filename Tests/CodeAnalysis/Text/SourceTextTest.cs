namespace Theta.Tests.CodeAnalysis.Text;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.CodeAnalysis.Text;

public class SourceTextTest
{
    [Theory]
    [InlineData(".", 1)]
    [InlineData(".\r\n", 2)]
    [InlineData(".\n", 2)]
    [InlineData(".\r\n\r\n", 3)]
    public void SourceText_IncludeLastLine(string text, int expectedLineCount)
    {
        var sourceText = SourceText.FromText(text);
        Assert.Equal(expectedLineCount, sourceText.Lines.Count);
    }
}