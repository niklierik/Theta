using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.Language.Evaluation;
using Theta.Language.Objects;
using Theta.Language.Text;
using Theta.Transpilers;

namespace Theta;

public static class Program
{

    public static List<string> Files { get; } = new()
    {
        "test"
    };

    public static void Main(string[] args)
    {
        var registry = new Registry();
        var sources = new Dictionary<string, SourceText>();
        foreach (var entry in Files)
        {
            Console.WriteLine(entry);
            using var reader = new StreamReader(entry + ".th");
            sources[entry] = SourceText.FromText(reader.ReadToEnd());
        }
        foreach (var (_, src) in sources)
        {
            Compilation.Prepare(src, registry);
        }
        foreach (var (entry, src) in sources)
        {
            using var transpiler = new CPPTranspiler(entry + ".hpp", entry + ".cpp");
            Compilation.CompilerFor(src).Compile(transpiler);
        }
    }

}