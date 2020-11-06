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
internal struct FirstPerformanceJob : IJob
{
    [ReadOnly]
    public NativeArray<int> Source;

    public void Execute()
    {
        Blinq.First(Source);
    }
}

internal class FirstPerformanceTest : BaseBlinqPerformanceTest
{
    [Test, Performance, Category("Performance")]
    public void FirstPerformance()
    {
        MakeMeasurement(() => new FirstPerformanceJob { Source = source }.Run(), "Blinq").Run();
        MakeMeasurement(() => Linq.First(source), "Linq").Run();
    }
}
