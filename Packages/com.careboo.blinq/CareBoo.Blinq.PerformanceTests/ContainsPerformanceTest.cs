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
    const int item = 100000;

    [Test, Performance, Category("Performance")]
    public void BlinqEquatableNativeSequence()
    {
        MakeMeasurement(() => new SequenceContainsJob { Source = source, Item = item }.Run(), "Blinq_Sequence").Run();
    }

    [Test, Performance, Category("Performance")]
    public void BlinqEquatableNativeArray()
    {
        MakeMeasurement(() => new ArrayContainsJob { Source = source, Item = item }.Run(), "Blinq_Array").Run();
    }

    [Test, Performance, Category("Performance")]
    public void LinqEquatableNativeArray()
    {
        MeasureLinq(() => Linq.Contains(source, item)).Run();
    }
}
