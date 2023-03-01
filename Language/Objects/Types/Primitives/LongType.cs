namespace Theta.Language.Objects.Types.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LongType : NumberType
{

    private static LongType? _instance;
    private static object _lock = new();

    private LongType() : base("Lib.Long")
    {
    }

    public static LongType Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (_lock)
                {
                    if (_instance is null)
                    {
                        _instance = new LongType();
                    }
                }
            }
            return _instance;
        }
    }
}