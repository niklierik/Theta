namespace Theta.Transpilers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.CodeAnalysis.Binding;

public class CPPTranspiler : Transpiler
{

    private readonly StreamWriter header;
    private readonly StreamWriter src;

    public CPPTranspiler(string header, string src)
    {
        this.header = new(header);
        this.src = new(src);
    }

    public override void Dispose()
    {
        header?.Dispose();
        src?.Dispose();
    }

    // private int indentationLvl = 0;

    public const string IndentationStr = "    ";

    public static string Indentation(int lvl)
    {
        string s = "";
        for (int i = 0; i < lvl; i++)
        {
            s += IndentationStr;
        }
        return s;
    }

    public override void TranspileBlockStatement(BoundBlockStatement boundBlockStatement, int indentation = 0)
    {
        src.WriteLine("{");
        foreach (var statement in boundBlockStatement.Statements)
        {
            statement.Transpile(this, indentation + 1);
        }
        src.WriteLine("}");
    }

    public override void TranspileExpressionStatement(BoundExpressionStatement expressionStatement, int indentation = 0)
    {

    }
}