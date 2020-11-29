using NUnit.Framework;
using Unity.PerformanceTesting;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;
using Unity.Collections;

internal class ConcatPerformanceTest : BaseBlinqPerformanceTest
{
    [Test, Performance, Category("Performance")]
    public void BlinqNativeSequence()
    {
        var second = new NativeArray<int>(source, Allocator.Persistent);
        MeasureBlinq(() =>
        {
            var concat = Blinq.RunToNativeList(Blinq.Concat(in source, in second), Allocator.Persistent);
            concat.Dispose();
        }).Run();
        second.Dispose();
    }

    [Test, Performance, Category("Performance")]
    public void LinqNativeSequence()
    {
        var second = new NativeArray<int>(source, Allocator.Persistent);
        MeasureLinq(() => Linq.ToList(Linq.Concat(source, second))).Run();
        second.Dispose();
    }
}
