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
internal struct ExceptPerformanceJob : IJob
{
    [ReadOnly]
    public NativeArray<int> Source;

    [ReadOnly]
    public NativeArray<int> Second;

    public void Execute()
    {
        var result = Blinq.Except(Source, Second).ToNativeList(Allocator.Temp);
        result.Dispose();
    }
}

internal class ExceptPerformanceTest : BaseBlinqPerformanceTest
{
    [Test, Performance, Category("Performance")]
    public void BlinqSequence()
    {
        MakeMeasurement(() => new ExceptPerformanceJob { Source = source, Second = source }.Run(), "Blinq").Run();
    }

    [Test, Performance, Category("Performance")]
    public void LinqSequence()
    {
        MakeMeasurement(() => Linq.ToList(Linq.Except(source, source)), "Linq").Run();
    }
}
