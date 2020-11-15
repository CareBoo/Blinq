using NUnit.Framework;
using Unity.PerformanceTesting;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using static ValueFuncs;
using CareBoo.Burst.Delegates;

[BurstCompile]
internal struct SelectJob<TSelector> : IJob
    where TSelector : struct, IFunc<int, long>
{
    [ReadOnly]
    public NativeArray<int> Source;
    [ReadOnly]
    public ValueFunc<int, long>.Struct<TSelector> Selector;

    public void Execute()
    {
        Blinq.Select(Source, Selector).Execute();
    }
}

internal class SelectPerformanceTest : BaseBlinqPerformanceTest
{
    [Test, Performance, Category("Performance")]
    public void BlinqSequence()
    {
        MeasureBlinq(() => new SelectJob<Functions.IntToLong> { Source = source, Selector = IntToLong }.Run()).Run();
    }

    [Test, Performance, Category("Performance")]
    public void LinqSequence()
    {
        MeasureLinq(() => Linq.ToList(Linq.Select(source, IntToLong.Invoke))).Run();
    }
}
