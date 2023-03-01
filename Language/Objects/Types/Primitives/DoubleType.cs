namespace Theta.Language.Objects.Types.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DoubleType : NumberType
{

    private static DoubleType? _instance;
    private static object _lock = new();

    private DoubleType() : base("Lib.Double")
    {
    }

    public static DoubleType Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (_lock)
                {
                    if (_instance is null)
                    {
                        _instance = new DoubleType();
                    }
                }
            }
            return _instance;
        }
    }
}