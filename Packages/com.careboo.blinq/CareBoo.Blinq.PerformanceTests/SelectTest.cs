using CareBoo.Blinq;
using NUnit.Framework;
using Unity.Collections;
using Unity.PerformanceTesting;
using LinqEnumerable = System.Linq.Enumerable;

internal class SelectTest : BaseBlinqPerformanceTest
{
    [Test, Performance]
    [Category("Performance")]
    public void BlinqSelectNativeSequencePerformance()
    {
        NativeSequence<int> source = default;
        NativeSequence<long> output = default;

        MeasureBlinq(() =>
        {
            output = source.Select<long, IntToLong>();
            output.Complete();
        })
        .SetUp(() => source = new NativeSequence<int>(sourceArr, Allocator.Persistent))
        .CleanUp(() => output.Dispose())
        .Run();
    }

    [Test, Performance]
    [Category("Performance")]
    public void LinqSelectNativeSequencePerformance()
    {
        NativeSequence<int> source = default;
        long[] output = null;

        MeasureLinq(() => output = LinqEnumerable.ToArray(LinqEnumerable.Select(source, default(IntToLong).Invoke)))
        .SetUp(() => source = new NativeSequence<int>(sourceArr, Allocator.Persistent))
        .CleanUp(() => source.Dispose())
        .Run();
    }
}
