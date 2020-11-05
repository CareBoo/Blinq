using CareBoo.Burst.Delegates;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;
using NUnit.Framework;
using static ValueFuncs;
using Unity.PerformanceTesting;

[BurstCompile]
internal struct AllJob<TPredicate> : IJob
    where TPredicate : struct, IFunc<int, bool>
{
    [ReadOnly]
    public NativeArray<int> Source;

    public ValueFunc<int, bool>.Struct<TPredicate> Predicate;

    public void Execute()
    {
        Blinq.All(Source, Predicate);
    }
}

internal class AllPerformanceTest : BaseBlinqPerformanceTest
{
    [Test, Performance, Category("Performance")]
    public void AllNativeSequencePerformance()
    {
        MeasureBlinq(() => new AllJob<Functions.GreaterThanOrEqualToZero> { Source = source, Predicate = GreaterThanOrEqualToZero }.Run()).Run();
        MeasureLinq(() => Linq.All(source, GreaterThanOrEqualToZero.Invoke)).Run();
    }
}
