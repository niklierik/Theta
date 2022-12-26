﻿namespace Theta.Transpilers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.CodeAnalysis.Binding;

public abstract partial class Transpiler : IDisposable
{
    public abstract void Dispose();
    public abstract void TranspileBlockStatement(BoundBlockStatement blockStatement, int indentation = 0);
    public abstract void TranspileExpressionStatement(BoundExpressionStatement expressionStatement, int indentation = 0);
    public abstract string TranspileVariableDeclaration(BoundVariableDeclarationExpression boundVariableDeclarationExpression, int indentation = 0);

    public virtual string GetStringOfExpression(BoundExpression? expression)
    {
        return StringifyExpression(expression);
    }

    public void Transpile(BoundStatement statement)
    {
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

    public string StringifyExpression(BoundExpression? expression)
    {
        if (expression is null)
        {
            return "";
        }
        return expression.Stringify(this);
    }

}