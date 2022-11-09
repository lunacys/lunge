using System;
using lunge.Library.Scripting;
using Nez;

namespace Playground2.Components.ModBenchmark;

public class BenchmarkMoonSharpComponent : Component, IModBenchmark
{
    private ModHandler _modHandler;

    public Language Lang => Language.Lua;

    public BenchmarkMoonSharpComponent()
    {
    }

    public TimeSpan BenchmarkScript(string code)
    {
        throw new NotImplementedException();
    }
}