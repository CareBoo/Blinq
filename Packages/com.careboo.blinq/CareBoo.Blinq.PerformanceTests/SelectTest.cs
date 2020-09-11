using NUnit.Framework;
using Unity.PerformanceTesting;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.NativeArrayExtensions;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using static ValueFuncs;
using CareBoo.Blinq;

[BurstCompile]
internal struct SelectJob : IJob
{
    [ReadOnly]
    public NativeArray<int> Source;
    [ReadOnly]
    public ValueFunc<int, long> Selector;

    public void Execute()
    {
        Blinq.Select(ref Source, Selector).Execute();
    }
}

internal class SelectTest : BaseBlinqPerformanceTest
{

    [Test, Performance]
    [Category("Performance")]
    public void BlinqSelectNativeSequencePerformance()
    {
        MeasureBlinq(() => new SelectJob { Source = source, Selector = IntToLong }.Run()).Run();
    }

    [Test, Performance]
    [Category("Performance")]
    public void LinqSelectNativeSequencePerformance()
    {
        MeasureLinq(() => Linq.ToList(Linq.Select(source, Functions.IntToLong))).Run();
    }
}
