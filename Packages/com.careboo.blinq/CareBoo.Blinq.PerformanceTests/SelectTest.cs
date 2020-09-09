using NUnit.Framework;
using Unity.PerformanceTesting;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.NativeArrayExtensions;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
internal struct SelectJob : IJob
{
    [ReadOnly]
    public NativeArray<int> Source;

    public void Execute()
    {
        Blinq.Select<int, long, IntToLong>(ref Source).Execute();
    }
}

internal class SelectTest : BaseBlinqPerformanceTest
{

    [Test, Performance]
    [Category("Performance")]
    public void BlinqSelectNativeSequencePerformance()
    {
        MeasureBlinq(() => new SelectJob { Source = source }.Run()).Run();
    }

    [Test, Performance]
    [Category("Performance")]
    public void LinqSelectNativeSequencePerformance()
    {
        MeasureLinq(() => Linq.ToList(Linq.Select(source, default(IntToLong).Invoke))).Run();
    }
}
