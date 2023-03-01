namespace Theta.Language.Objects.Types.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ShortType : NumberType
{

    private static ShortType? _instance;
    private static object _lock = new();

    private ShortType() : base("Lib.Short")
    {
    }

    public static ShortType Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (_lock)
                {
                    if (_instance is null)
                    {
                        _instance = new ShortType();
                    }
                }
            }
            return _instance;
        }
    }
}