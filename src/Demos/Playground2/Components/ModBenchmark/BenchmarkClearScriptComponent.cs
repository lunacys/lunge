using System;
using lunge.Library.Scripting;
using Nez;

namespace Playground2.Components.ModBenchmark;

public class BenchmarkClearScriptComponent : Component, IModBenchmark
{
    private ModHandler _modHandler;

    public Language Lang => Language.JavaScript;

    public BenchmarkClearScriptComponent()
    {
    }

    public TimeSpan BenchmarkScript(string code)
    {
        throw new NotImplementedException();
    }
}