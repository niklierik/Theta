namespace Theta.Transpilers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.CodeAnalysis.Binding;

public class CPPTranspiler : Transpiler
{

    private StreamWriter? header;
    private StreamWriter? src;

    public StreamWriter Header => header!;
    public StreamWriter Source => src!;

    public CPPTranspiler(string header, string src)
    {
        this.header = new(header);
        this.src = new(src);
    }

    public override void Dispose()
    {
        header?.Close();
        src?.Close();
        header?.Dispose();
        src?.Dispose();
        header = null;
        src = null;
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
        Source.WriteLine("{");
        foreach (var statement in boundBlockStatement.Statements)
        {
            statement.Transpile(this, indentation + 1);
        }
        Source.WriteLine("}");
    }

    public override void TranspileExpressionStatement(BoundExpressionStatement expressionStatement, int indentation = 0)
    {
        Source.Write(Indentation(indentation) + GetStringOfExpression(expressionStatement.Expression) + ";");
    }

    public override string TranspileVariableDeclaration(BoundVariableDeclarationExpression varDecl, int indentation = 0)
    {
        var var = varDecl.Variable;
        var res = varDecl.Variable.IsConst ? "const " : "";
        res += $"auto {var.Name}";
        if (varDecl.EqualsTo is not null)
        {
            res += $" = {varDecl.EqualsTo.Stringify(this)}";
        }
        return res;
    }
}