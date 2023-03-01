namespace Theta.Transpilers;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.Language.Binding;
using Theta.Language.Messages;

public class CPPTranspiler : Transpiler
{

    private StreamWriter? header;
    private StreamWriter? src;

    public StringBuilder StartHeader { get; } = new();
    public StringBuilder Header { get; } = new();
    public StringBuilder EndHeader { get; } = new();
    public StringBuilder StartSource { get; } = new();
    public StringBuilder Source { get; } = new();
    public StringBuilder EndSource { get; } = new();

    public string HeaderFile { get; }
    public string SourceFile { get; }

    public CPPTranspiler(string header, string src)
    {
        HeaderFile = header;
        SourceFile = src;
        this.header = new(header);
        this.src = new(src);
    }

    public override void Init()
    {
        StartSource.AppendLine($"#include <{HeaderFile}>");
    }

    private void FinishHeader()
    {
        if (header is not null)
        {
            try
            {
                if (!Diagnostics.HasError)
                {
                    header?.WriteLine(StartHeader.ToString());
                    header?.WriteLine(Header.ToString());
                    header?.WriteLine(EndHeader.ToString());
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unable to write into header file.");
                Console.WriteLine("Unable to write into header file.");
                Console.WriteLine(e);
                Console.ResetColor();
            }
            try
            {
                header?.Close();
            }
            catch { }
            try
            {
                header?.Dispose();
            }
            catch { }
        }
        header = null;
    }

    private void FinishSource()
    {
        try
        {
            src?.WriteLine(StartSource.ToString());
            src?.WriteLine(Source.ToString());
            src?.WriteLine(EndSource.ToString());
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Unable to write into source file.");
            Console.WriteLine("Unable to write into source file.");
            Console.WriteLine(e);
            Console.ResetColor();
        }
        try
        {
            src?.Close();
        }
        catch { }
        try
        {
            src?.Dispose();
        }
        catch { }
        src = null;
    }

    public override void Dispose()
    {
        FinishHeader();
        FinishSource();
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
        StartSource.AppendLine("{");
        foreach (var statement in boundBlockStatement.Statements)
        {
            statement.Transpile(this, indentation + 1);
            StartSource.AppendLine();
        }
        StartSource.AppendLine("}");
    }

    public override void TranspileExpressionStatement(BoundExpressionStatement expressionStatement, int indentation = 0)
    {
        StartSource.AppendLine(Indentation(indentation) + GetStringOfExpression(expressionStatement.Expression) + ";");
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

    public override void TranspileAliasStatement(BoundAliasStatement import, int indentation = 0)
    {
        // StartHeader.AppendLine($"using {import.New.FullName.Replace(".", "::")} = {import.Old.FullName.Replace(".", "::")};");

    }

    public override void TranspileNamespaceStatement(BoundNamespaceStatement ns, int indentation = 0)
    {
        StartSource.AppendLine("namespace " + ns.Namespace.Replace(".", "::"));
        StartSource.AppendLine("{");
        EndSource.AppendLine("}");
    }

    public override string TranspileLiteral(BoundLiteralExpression boundLiteralExpression)
    {
        var val = boundLiteralExpression.Value;
        if (val is char)
        {
            return $"'{val}'";
        }
        return JsonConvert.SerializeObject(val);
    }
}