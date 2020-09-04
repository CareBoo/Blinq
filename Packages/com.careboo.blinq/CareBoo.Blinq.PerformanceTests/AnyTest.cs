using NUnit.Framework;
using Unity.Collections;
using Unity.PerformanceTesting;
using LinqEnumerable = System.Linq.Enumerable;
using CareBoo.Blinq;

internal class AnyTest : BaseBlinqPerformanceTest
{
    NativeSequence<int> source;

    [OneTimeSetUp]
    public override void SetUpSource()
    {
        base.SetUpSource();
        source = new NativeSequence<int>(sourceArr, Allocator.Persistent);
    }

    [OneTimeTearDown]
    public override void TearDownSource()
    {
        base.TearDownSource();
        source.Dispose();
    }

    [Test, Performance]
    [Category("Performance")]
    public void BlinqAnyNativeSequencePerformance()
    {
        bool result;

        MeasureBlinq(() => result = source.Any()).Run();
    }

    [Test, Performance]
    [Category("Performance")]
    public void LinqAnyNativeSequencePerformance()
    {
        bool result;

        MeasureLinq(() => result = LinqEnumerable.Any(source)).Run();
    }


    [Test, Performance]
    [Category("Performance")]
    public void BlinqAnyPredicateNativeSequencePerformance()
    {
        bool result;

        MeasureBlinq(() => result = source.Any<EqualsOne>()).Run();
    }

    [Test, Performance]
    [Category("Performance")]
    public void LinqAnyPredicateNativeSequencePerformance()
    {
        bool result;

        MeasureLinq(() => result = LinqEnumerable.Any(source, default(EqualsOne).Invoke)).Run();
    }
}
