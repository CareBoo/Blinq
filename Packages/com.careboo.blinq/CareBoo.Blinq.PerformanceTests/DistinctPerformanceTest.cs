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
internal struct DistinctPerformanceJob : IJob
{
    [ReadOnly]
    public NativeArray<int> Source;

    public void Execute()
    {
        Blinq.Distinct(Source).Execute();
    }
}

internal class DistinctPerformanceTest : BaseBlinqPerformanceTest
{
    [Test, Performance, Category("Performance")]
    public void DistinctPerformance()
    {
        MakeMeasurement(() => new DistinctPerformanceJob { Source = source }.Run(), "Blinq").Run();
        MakeMeasurement(() => Linq.ToList(Linq.Distinct(source)), "Linq").Run();
    }
}
