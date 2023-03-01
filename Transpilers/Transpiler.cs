namespace Theta.Transpilers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.Language.Binding;

public abstract partial class Transpiler : IDisposable
{
    public abstract void Dispose();
    public abstract void TranspileBlockStatement(BoundBlockStatement blockStatement, int indentation = 0);
    public abstract void TranspileExpressionStatement(BoundExpressionStatement expressionStatement, int indentation = 0);
    public abstract string TranspileVariableDeclaration(BoundVariableDeclarationExpression boundVariableDeclarationExpression, int indentation = 0);

    public abstract void TranspileAliasStatement(BoundAliasStatement import, int indentation = 0);

    public abstract void Init();


    /*
     * Should not be null when we start using it
     * Otherwise throwing a null pointer exception when using it is the correcte behaviour
     */
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public BoundGlobalScope Global { get; set; }
    public BoundScope GlobalScope { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public virtual string GetStringOfExpression(BoundExpression? expression)
    {
        return StringifyExpression(expression);
    }

    public void Transpile(IEnumerable<BoundStatement?> statements)
    {
        foreach (var statement in statements)
        {
            Transpile(statement);
        }
    }

    public void Transpile(BoundStatement? statement)
    {
        if (statement is null)
        {
            return;
        }
        TranspileStatement(statement);
    }

    public void TranspileStatement(BoundStatement? statement, int indentation = 0)
    {
        if (statement is null)
        {
            return;
        }
        statement.Transpile(this, indentation);
    }

    public virtual string StringifyExpression(BoundExpression? expression)
    {
        if (expression is null)
        {
            return "";
        }
        return expression.Stringify(this);
    }

    public abstract void TranspileNamespaceStatement(BoundNamespaceStatement ns, int indentation = 0);

    public abstract string TranspileLiteral(BoundLiteralExpression boundLiteralExpression);
}