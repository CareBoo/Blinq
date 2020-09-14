using NUnit.Framework;
using Unity.PerformanceTesting;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using static ValueFuncs;
using CareBoo.Blinq;

[BurstCompile]
internal struct SelectJob<TSelector> : IJob
    where TSelector : struct, IFunc<int, long>
{
    [ReadOnly]
    public NativeArray<int> Source;
    [ReadOnly]
    public ValueFunc<int, long>.Impl<TSelector> Selector;

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
        MeasureBlinq(() => new SelectJob<Functions.IntToLong> { Source = source, Selector = IntToLong }.Run()).Run();
    }

    [Test, Performance]
    [Category("Performance")]
    public void LinqSelectNativeSequencePerformance()
    {
        MeasureLinq(() => Linq.ToList(Linq.Select(source, IntToLong.Invoke))).Run();
    }
}
