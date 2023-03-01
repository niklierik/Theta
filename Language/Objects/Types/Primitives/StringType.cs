using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theta.Language.Objects.Types.Primitives;

public class StringType : Class
{
    private static StringType? _type;
    private static object _lock = new();

    private StringType() : base("Lib.String")
    {

    }

    public static StringType Instance
    {
        get
        {
            if (_type is null)
            {
                lock (_lock)
                {
                    if (_type is null)
                    {
                        _type = new();
                    }
                }
            }
            return _type!;
        }
    }

}