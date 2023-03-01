namespace Theta.Language.Objects.Types.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FloatType : NumberType
{

    private static FloatType? _instance;
    private static object _lock = new();

    private FloatType() : base("Lib.Float")
    {
    }

    public static FloatType Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (_lock)
                {
                    if (_instance is null)
                    {
                        _instance = new FloatType();
                    }
                }
            }
            return _instance!;
        }
    }


}