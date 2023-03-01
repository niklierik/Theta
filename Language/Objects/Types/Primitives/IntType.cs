namespace Theta.Language.Objects.Types.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class IntType : NumberType
{

    private static IntType? _instance;
    private static object _lock = new();

    private IntType() : base("Lib.Int")
    {
    }

    public static IntType Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (_lock)
                {
                    if (_instance is null)
                    {
                        _instance = new IntType();
                    }
                }
            }
            return _instance;
        }
    }

}