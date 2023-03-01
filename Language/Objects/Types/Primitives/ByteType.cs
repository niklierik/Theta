namespace Theta.Language.Objects.Types.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ByteType : NumberType
{

    private static ByteType? _instance;
    private static object _lock = new();

    private ByteType() : base("Lib.Byte")
    {
    }

    public static ByteType Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (_lock)
                {
                    if (_instance is null)
                    {
                        _instance = new ByteType();
                    }
                }
            }
            return _instance;
        }
    }
}