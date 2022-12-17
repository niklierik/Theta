namespace Theta.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ConUtils
{
    public static void Log(this object? o, ConsoleColor color = ConsoleColor.Gray, bool lineBreak = true)
    {
        SwitchColor(color, () =>
        {
            Console.Write(o ?? "null");
            if (lineBreak)
            {
                Console.WriteLine();
            }
        });
    }

    public static void SwitchColor(ConsoleColor color, Action? toDo)
    {
        if (toDo is null)
        {
            return;
        }
        Console.ForegroundColor = color;
        toDo();
        Console.ResetColor();
    }

    public static void PrintMany(this IEnumerable<object> objects, string separator, ConsoleColor color = ConsoleColor.Gray, bool lineBreak = true)
    {
        var strings = objects.Select(o => o?.ToString() ?? "null");
        var line = string.Join(separator, strings);
        SwitchColor(color, () =>
        {
            Console.Write(line);
            Console.WriteLine();
        });
    }

    public static ConsoleColor GoodBadColor(this bool b)
    {
        return b ? ConsoleColor.Green : ConsoleColor.Red;
    }

    public static string OnOff(this bool b)
    {
        return b ? "On" : "Off";
    }

    public static ConsoleColor GetColor(this Type type)
    {
        if (type == typeof(double))
        {
            return ConsoleColor.Green;
        }
        if (type == typeof(long))
        {
            return ConsoleColor.Yellow;
        }
        if (type == typeof(bool))
        {
            return ConsoleColor.Cyan;
        }
        if (type == typeof(void))
        {
            return ConsoleColor.Red;
        }
        return ConsoleColor.Gray;
    }
}