using BenchmarkDotNet.Attributes;
using ChainOfResponsibility;

namespace Benchmarks;

[MemoryDiagnoser]
public class ChainOfResponsibilityBenchmark
{
    private IChain<int[]> _chainTyped;

    private IAsyncChain<int[]> _asyncChain;

    private int[] _values;

    [GlobalSetup]
    public void Setup()
    {
        _values = [1, 2, 3, 4, 5];
        _chainTyped = ChainBuilder
            .Start<int[]>(x => x.Length > 3)
            .Then(x => x.Length > 4)
            .Then(x => x.Length == 5)
            .Build();

        _asyncChain = AsyncChainBuilder
            .Start<int[]>(x => Task.FromResult(x.Length > 3))
            .Then(x => Task.FromResult(x.Length > 4))
            .Then(x => Task.FromResult(x.Length == 5))
            .Build();
    }

    [Benchmark]
    public int[] RunArrayChainTyped()
    {
        _chainTyped.Execute(_values);
        return _values;
    }

    [Benchmark]
    public async Task<int[]> RunArrayChainTypedAsync()
    {
        await _asyncChain.Execute(_values);
        return _values;
    }
}
