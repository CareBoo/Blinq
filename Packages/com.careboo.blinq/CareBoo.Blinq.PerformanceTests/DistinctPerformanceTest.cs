using NUnit.Framework;
using Unity.PerformanceTesting;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
internal struct DistinctPerformanceJob : IJob
{
    [ReadOnly]
    public NativeArray<int> Source;

    public void Execute()
    {
        var result = Blinq.Distinct(Source).ToNativeList(Allocator.Temp);
        result.Dispose();
    }
}

internal class DistinctPerformanceTest : BaseBlinqPerformanceTest
{
    [Test, Performance, Category("Performance")]
    public void BlinqSequence()
    {
        MakeMeasurement(() => new DistinctPerformanceJob { Source = source }.Run(), "Blinq").Run();
    }

    [Test, Performance, Category("Performance")]
    public void LinqSequence()
    {
        MakeMeasurement(() => Linq.ToList(Linq.Distinct(source)), "Linq").Run();
    }
}
