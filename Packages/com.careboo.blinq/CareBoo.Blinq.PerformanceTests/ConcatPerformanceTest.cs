using NUnit.Framework;
using Unity.PerformanceTesting;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
internal struct ConcatJob : IJob
{
    [ReadOnly]
    public NativeArray<int> Source;

    [ReadOnly]
    public NativeArray<int> Second;

    public void Execute()
    {
        var result = Blinq.Concat(Source, Second).ToNativeList(Allocator.Temp);
        result.Dispose();
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
