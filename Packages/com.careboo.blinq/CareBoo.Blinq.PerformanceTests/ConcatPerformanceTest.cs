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
internal struct ConcatJob : IJob
{
    [ReadOnly]
    public NativeArray<int> Source;

    [ReadOnly]
    public NativeArray<int> Second;

    public void Execute()
    {
        Blinq.Concat(Source, Second).Execute();
    }
}

internal class ConcatPerformanceTest : BaseBlinqPerformanceTest
{
    [Test, Performance, Category("Performance")]
    public void BlinqNativeSequence()
    {
        MeasureBlinq(() => new ConcatJob { Source = source, Second = source }.Run()).Run();
    }

    [Test, Performance, Category("Performance")]
    public void LinqNativeSequence()
    {
        MeasureLinq(() => Linq.ToList(Linq.Concat(source, source))).Run();
    }
}
