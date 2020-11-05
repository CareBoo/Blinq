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
internal struct AppendJob : IJob
{
    [ReadOnly]
    public NativeArray<int> Source;

    public int Item;

    public void Execute()
    {
        Blinq.Append(Source, Item).Execute();
    }
}

internal class AppendPerformanceTest : BaseBlinqPerformanceTest
{
    [Test, Performance, Category("Performance")]
    public void AppendNativeSequencePerformance()
    {
        var item = 32;
        MeasureBlinq(() => new AppendJob { Source = source, Item = item }.Run()).Run();
        MeasureLinq(() => Linq.ToList(Linq.Append(source, item))).Run();
    }
}
