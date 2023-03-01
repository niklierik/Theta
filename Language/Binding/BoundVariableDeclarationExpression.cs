using Theta.Language.Objects.Types;
using Theta.Language.Text;
using Theta.Transpilers;

namespace Theta.Language.Binding
{
    public class BoundVariableDeclarationExpression : BoundExpression
    {
        public BoundVariableDeclarationExpression(TextSpan span, VariableSymbol variable, BoundExpression? equalsTo)
        {
            Variable = variable;
            EqualsTo = equalsTo;
            Span = span;
        }

        public override TypeIdentifier Type => Variable.Type;

        public override TextSpan Span { get; }
        public VariableSymbol Variable { get; }
        public BoundExpression? EqualsTo { get; }

        public override string Stringify(Transpiler transpiler, int indentation = 0)
        {
            return transpiler.TranspileVariableDeclaration(this);
        }
    }
}