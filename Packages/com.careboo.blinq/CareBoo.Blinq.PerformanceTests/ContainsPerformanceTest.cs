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
internal struct ArrayContainsJob : IJob
{
    [ReadOnly]
    public NativeArray<int> Source;

    public int Item;

    public void Execute()
    {
        Blinq.Contains(Source, Item);
    }
}

[BurstCompile]
internal struct SequenceContainsJob : IJob
{
    [ReadOnly]
    public NativeArray<int> Source;

    public int Item;

    public void Execute()
    {
        Blinq.Contains(Blinq.ToValueSequence(Source), Item);
    }
}

internal class ContainsPerformanceTest : BaseBlinqPerformanceTest
{
    [Test, Performance, Category("Performance")]
    public void ContainsEquatablePerformance()
    {
        var item = 100000;
        MakeMeasurement(() => new ArrayContainsJob { Source = source, Item = item }.Run(), "Blinq_Array").Run();
        MakeMeasurement(() => new SequenceContainsJob { Source = source, Item = item }.Run(), "Blinq_Sequence").Run();
        MeasureLinq(() => Linq.Contains(source, item)).Run();
    }
}
