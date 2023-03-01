namespace Theta.Language.Objects.Types;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.Language.Objects.Types.Primitives;

public abstract class TypeIdentifier
{

    public virtual string GetTranspiledName() => Name.Replace(".", "::");

    public TypeIdentifier(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public abstract bool CanBeCastInto(TypeIdentifier other);

    public abstract bool IsNumber();

    public abstract bool IsPrimitive();

    public override string ToString()
    {
        return Name;
    }

}

public static class Types
{
    public static TypeIdentifier Object => ObjectClass.Instance;

    public static TypeIdentifier Long => LongType.Instance;
    public static TypeIdentifier Int => IntType.Instance;
    public static TypeIdentifier Short => ShortType.Instance;
    public static TypeIdentifier Byte => ByteType.Instance;
    public static TypeIdentifier Float => FloatType.Instance;
    public static TypeIdentifier Double => DoubleType.Instance;

    public static TypeIdentifier Bool => BoolType.Instance;

    public static TypeIdentifier Number => NumberType.Type;
    public static TypeIdentifier Void => VoidType.Instance;

    public static TypeIdentifier Type => TypeType.Instance;

    public static TypeIdentifier String => StringType.Instance;
    public static TypeIdentifier Char => CharType.Instance;

}