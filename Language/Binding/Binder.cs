using Theta.Language.Messages;
using Theta.Language.Objects.Types;
using Theta.Language.Objects.Types.Primitives;
using Theta.Language.Syntax;
using Theta.Language.Syntax.Statements;

namespace Theta.Language.Binding;

public sealed class Binder
{

    public Binder(BoundScope? parent)
    {
        Scope = new BoundScope(parent);
    }

    public static (BoundGlobalScope, BoundScope) BindGlobalScope(BoundGlobalScope? prev, CompilationUnitSyntax compilation)
    {
        var parent = CreateParentScopes(prev);
        var binder = new Binder(parent);
        var statements = binder.BindStatements(compilation.Statements);
        List<BoundStatement> statements2 = (statements?.Where(statement => statement is not null)?.ToList() ?? new());
        var variables = binder.Scope.GetVariables();
        return (new BoundGlobalScope(prev, variables, statements2.ToList()), binder.Scope);
    }

    private static BoundScope? CreateParentScopes(BoundGlobalScope? prev)
    {
        var stack = new Stack<BoundGlobalScope>();
        while (prev is not null)
        {
            stack.Push(prev);
            prev = prev.Prev;
        }
        BoundScope? current = null;
        while (stack.Count > 0)
        {
            var global = stack.Pop();
            var scope = new BoundScope(current);
            foreach (var var in global.DeclaredVars)
            {
                scope.TryDeclare(var);
            }
            current = scope;
        }
        return current;
    }

    public BoundScope Scope { get; private set; }

    public BoundStatement? BindStatement(StatementSyntax? syntax)
    {
        if (syntax is null)
        {
            return null;
        }
        switch (syntax.Type)
        {
            case SyntaxType.BlockStatement:
                return BindBlockStatement(syntax as BlockStatementSyntax<StatementSyntax>);
            case SyntaxType.ExpressionStatement:
                return BindExpressionStatement(syntax as ExpressionStatement);
            case SyntaxType.AliasStatement:
                return BindAliasStatement(syntax as AliasStatement);
            case SyntaxType.NamespaceStatement:
                return BindNamespaceStatement(syntax as NamespaceStatement);
            default:
                throw new Exception("Invalid statement was give. This should not happen.");
        }

    }

    private BoundStatement? BindNamespaceStatement(NamespaceStatement? @namespace)
    {
        if (@namespace is null)
        {
            return null;
        }
        return new BoundNamespaceStatement(@namespace.Name.FullName);
    }

    private BoundStatement? BindAliasStatement(AliasStatement? alias)
    {
        if (alias is null)
        {
            return null;
        }
        return new BoundAliasStatement(alias.NewName ?? new NamedExpressionSyntax(new SyntaxToken(SyntaxType.IdentifierToken) { Position = 0, Text = alias.OldName.LastPart }), alias.OldName);
    }

    private BoundStatement? BindExpressionStatement(ExpressionStatement? expressionStatement)
    {
        return new BoundExpressionStatement(BindExpression(expressionStatement?.Expression)!);
    }

    private IEnumerable<BoundStatement> BindStatements(IEnumerable<StatementSyntax?>? statements)
    {
        if (statements is null)
        {
            yield break;
        }
        foreach (var statement in statements)
        {
            var bound = BindStatement(statement);
            if (bound is not null)
            {
                yield return bound;
            }
        }
    }

    private BoundStatement? BindBlockStatement(BlockStatementSyntax<StatementSyntax>? blockStatementSyntax)
    {
        IEnumerable<BoundStatement> statements = BindStatements(blockStatementSyntax?.Statements);
        return new BoundBlockStatement(statements.ToList());
    }

    public BoundExpression? BindExpression(ExpressionSyntax? syntax)
    {
        if (syntax is null)
        {
            return null;
        }
        switch (syntax.Type)
        {
            case SyntaxType.LiteralExpression:
                return BindLiteralExpression((LiteralExpressionSyntax) syntax);
            case SyntaxType.BinaryExpression:
                BinaryExpressionSyntax binary = (BinaryExpressionSyntax) syntax;
                return BindBinaryExpression(binary);
            case SyntaxType.UnaryExpression:
                UnaryExpressionSyntax unary = (UnaryExpressionSyntax) syntax;
                return BindUnaryExpression(unary);
            case SyntaxType.GroupExpression:
                BracketExpression bracketExpression = (BracketExpression) syntax;
                return BindExpression(bracketExpression.Expression);
            case SyntaxType.NameExpression:
                NamedExpressionSyntax namedExpression = (NamedExpressionSyntax) syntax;
                return BindNamedExpression(namedExpression);
            case SyntaxType.AssignmentExpression:
                AssignmentExpressionSyntax assignmentExpression = (AssignmentExpressionSyntax) syntax;
                return BindAssignmentExpression(assignmentExpression);
            case SyntaxType.VariableDeclarationExpression:
                VariableDeclarationSyntax? varDecl = syntax as VariableDeclarationSyntax;
                return BindVariableDeclaration(varDecl);
            default:
                throw new Exception($"Cannot continue binding. Unexpected syntax {syntax.Type}.");
        }
    }

    private BoundExpression? BindVariableDeclaration(VariableDeclarationSyntax? varDecl)
    {
        if (varDecl is null)
        {
            return null;
        }
        var name = varDecl.Name.Text;
        var equalsTo = BindExpression(varDecl.EqualsTo);
        var var = new VariableSymbol(name, equalsTo?.Type ?? Types.Object, varDecl.IsConst);
        if (!Scope.TryDeclare(var))
        {
            Diagnostics.ReportVarAlreadyDeclared(name, varDecl.Span);
            return null;
        }
        return new BoundVariableDeclarationExpression(varDecl.Span, var, equalsTo);
    }

    private BoundExpression BindLiteralExpression(LiteralExpressionSyntax literal)
    {
        var value = literal.Value;
        return new BoundLiteralExpression(value, literal.Span);
    }

    private BoundExpression? BindBinaryExpression(BinaryExpressionSyntax binary)
    {
        var left = BindExpression(binary.Left);
        if (left is null)
        {
            Diagnostics.ReportBinderError(binary.Span);
            return null;
        }
        var right = BindExpression(binary.Right);
        if (right is null)
        {
            Diagnostics.ReportBinderError(binary.Span);
            return null;
        }
        var op = BoundBinaryOperator.Bind(binary.Operator.Type, left.Type, right.Type);
        if (op is null)
        {
            // Diagnostics.Add($"ERROR: Binary expression does not exist for {binary.Operator.Type} with operands {left.Type} and {right.Type}.");
            Diagnostics.ReportInvalidBinaryExpression(binary, left, right, binary.Span);
            return null;
        }
        return new BoundBinaryExpression(left, op, right, binary.Span);
    }

    private BoundExpression? BindUnaryExpression(UnaryExpressionSyntax unary)
    {
        var boundOperand = BindExpression(unary.Operand);
        if (boundOperand is null)
        {
            Diagnostics.ReportBinderError(unary.Span);
            return null;
        }
        var op = BoundUnaryOperator.Bind(unary.Operator.Type, boundOperand.Type);
        if (op is null)
        {
            Diagnostics.ReportInvalidUnaryExpression(unary, boundOperand, unary.Span);
            return null;
        }
        return new BoundUnaryExpression(boundOperand, op, unary.Span);
    }


    private BoundExpression? BindNamedExpression(NamedExpressionSyntax namedExpression)
    {
        var name = namedExpression.FullName;
        if (!Scope.TryLookup(name, out var variable) || variable is null)
        {
            Diagnostics.ReportUndefinedVariable(name, namedExpression.Span);
            return new BoundLiteralExpression(null, namedExpression.Span);
        }
        return new BoundVariableExpression(variable, namedExpression.Span);
    }

    private BoundExpression? BindAssignmentExpression(AssignmentExpressionSyntax assignmentExpression)
    {
        var name = assignmentExpression.Identifier.Text;

        var expression = BindExpression(assignmentExpression.Expression);

        if (!Scope.TryLookup(name, out var variable))
        {
            // variable = new VariableSymbol(name, expression?.Type ?? typeof(void));

            Diagnostics.ReportUndefinedVariable(name, assignmentExpression.Span);
            return new BoundLiteralExpression(null, assignmentExpression.Span);
        }
        // var variable = new VariableSymbol(name, expression?.Type ?? typeof(void));
        if (!(expression?.Type ?? ObjectClass.Instance).CanBeCastInto((variable?.Type ?? ObjectClass.Instance)))
        {
            Diagnostics.ReportInvalidCast(variable, expression, assignmentExpression.Span);
            return new BoundLiteralExpression(null, assignmentExpression.Span);
        }
        return new BoundAssignmentExpression(variable!, expression, assignmentExpression.Span);
    }

}
