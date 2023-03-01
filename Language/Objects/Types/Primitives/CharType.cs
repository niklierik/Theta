namespace Theta.Language.Objects.Types.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CharType : NumberType
{

    private static CharType? _instance;
    private static object _lock = new();

    private CharType() : base("Lib.Char")
    {
    }

    public static CharType Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (_lock)
                {
                    if (_instance is null)
                    {
                        _instance = new();
                    }
                }
            }
            return _instance;
        }
    }
}