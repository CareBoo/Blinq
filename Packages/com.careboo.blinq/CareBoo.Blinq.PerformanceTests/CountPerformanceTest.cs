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
internal struct CountArrayPredicateJob<TPredicate> : IJob
    where TPredicate : struct, IFunc<int, bool>
{
    [ReadOnly]
    public NativeArray<int> Source;

    public ValueFunc<int, bool>.Struct<TPredicate> Predicate;

    public void Execute()
    {
        Blinq.Count(Source, Predicate);
    }
}

[BurstCompile]
internal struct CountSequencePredicateJob<TPredicate> : IJob
    where TPredicate : struct, IFunc<int, bool>
{
    [ReadOnly]
    public NativeArray<int> Source;

    public ValueFunc<int, bool>.Struct<TPredicate> Predicate;

    public void Execute()
    {
        Blinq.Count(Blinq.ToValueSequence(Source), Predicate);
    }
}

internal class CountPerformanceTest : BaseBlinqPerformanceTest
{
    [Test, Performance, Category("Performance")]
    public void CountPredicatePerformance()
    {
        MakeMeasurement(() => new CountArrayPredicateJob<Functions.EqualsZero> { Source = source, Predicate = EqualsZero }.Run(), "Blinq_Array").Run();
        MakeMeasurement(() => new CountSequencePredicateJob<Functions.EqualsZero> { Source = source, Predicate = EqualsZero }.Run(), "Blinq_Sequence").Run();
        MakeMeasurement(() => Linq.Count(source, EqualsZero.Invoke), "Linq").Run();
    }
}
