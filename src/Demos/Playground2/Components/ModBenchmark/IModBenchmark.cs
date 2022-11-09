using System;

namespace Playground2.Components.ModBenchmark;

public enum Language
{
    JavaScript,
    Lua
}

public interface IModBenchmark
{
    Language Lang { get; }

    TimeSpan BenchmarkScript(string code);
}