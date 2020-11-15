using NUnit.Framework;
using Unity.PerformanceTesting;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
internal struct AppendJob : IJob
{
    [ReadOnly]
    public NativeArray<int> Source;

    public int Item;

    public void Execute()
    {
        var result = Blinq.Append(Source, Item).ToNativeList(Allocator.Temp);
        result.Dispose();
    }
}

internal class AppendPerformanceTest : BaseBlinqPerformanceTest
{
    [Test, Performance, Category("Performance")]
    public void BlinqNativeSequence()
    {
        var item = 32;
        MeasureBlinq(() => new AppendJob { Source = source, Item = item }.Run()).Run();
    }

    [Test, Performance, Category("Performance")]
    public void LinqNativeSequence()
    {
        var item = 32;
        MeasureLinq(() => Linq.ToList(Linq.Append(source, item))).Run();
    }
}
