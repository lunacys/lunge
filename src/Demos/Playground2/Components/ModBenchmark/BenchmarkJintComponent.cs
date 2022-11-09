using System;
using lunge.Library.Scripting;
using Nez;

namespace Playground2.Components.ModBenchmark;

public class BenchmarkJintComponent : Component, IModBenchmark
{
    private ModHandler _modHandler;

    public Language Lang => Language.JavaScript;

    public BenchmarkJintComponent()
    {
    }

    public TimeSpan BenchmarkScript(string code)
    {
        throw new NotImplementedException();
    }
}