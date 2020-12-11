using NUnit.Framework;
using Unity.PerformanceTesting;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;
using static ValueFuncs;
using CareBoo.Blinq;

internal class CountPerformanceTest : BaseBlinqPerformanceTest
{
    [Test, Performance, Category("Performance")]
    public void BlinqArrayPredicate()
    {
        MakeMeasurement(() => Blinq.RunCount(in source, EqualsZero), "Blinq_Array").Run();
    }

    [Test, Performance, Category("Performance")]
    public void BlinqSequencePredicate()
    {
        var sourceSequence = source.ToValueSequence();
        MakeMeasurement(() => Blinq.RunCount(in sourceSequence, EqualsZero), "Blinq_Sequence").Run();
    }

    [Test, Performance, Category("Performance")]
    public void LinqArrayPredicate()
    {
        MakeMeasurement(() => Linq.Count(source, EqualsZero.Invoke), "Linq").Run();
    }
}
