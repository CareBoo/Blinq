using NUnit.Framework;
using Unity.PerformanceTesting;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;
using static ValueFuncs;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using CareBoo.Burst.Delegates;

[BurstCompile]
internal struct AnyJob<TPredicate> : IJob
    where TPredicate : struct, IFunc<int, bool>
{
    [ReadOnly]
    public NativeArray<int> Source;

    [ReadOnly]
    public ValueFunc<int, bool>.Struct<TPredicate> Predicate;

    public void Execute()
    {
        Blinq.Any(Source, Predicate);
    }
}

internal class AnyPerformanceTest : BaseBlinqPerformanceTest
{
    [Test, Performance]
    [Category("Performance")]
    public void BlinqAnyNativeSequencePerformance()
    {
        bool result;

        MeasureBlinq(() => result = Blinq.Any(source)).Run();
    }

    [Test, Performance]
    [Category("Performance")]
    public void LinqAnyNativeSequencePerformance()
    {
        bool result;

        MeasureLinq(() => result = Linq.Any(source)).Run();
    }


    [Test, Performance]
    [Category("Performance")]
    public void BlinqAnyPredicateNativeSequencePerformance()
    {
        MeasureBlinq(() => new AnyJob<Functions.EqualsOne> { Source = source, Predicate = EqualsOne }.Run()).Run();
    }

    [Test, Performance]
    [Category("Performance")]
    public void LinqAnyPredicateNativeSequencePerformance()
    {
        bool result;

        MeasureLinq(() => result = Linq.Any(source, EqualsOne.Invoke)).Run();
    }
}
