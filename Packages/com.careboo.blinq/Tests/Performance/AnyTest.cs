using NUnit.Framework;
using Unity.Collections;
using Unity.PerformanceTesting;
using LinqEnumerable = System.Linq.Enumerable;
using System;
using CareBoo.Blinq;
using Unity.Burst;

internal class AnyTest
{
    const int NumElements = 16 << 10;
    NativeSequence<int> source;

    [OneTimeSetUp]
    public void SetUpSources()
    {
        var sourceArray = new int[NumElements];
        sourceArray[NumElements / 2] = 1;
        source = new NativeSequence<int>(sourceArray, Allocator.Persistent);
    }

    [OneTimeTearDown]
    public void TearDownSources()
    {
        source.Dispose();
    }

    [Test, Performance]
    [Category("Performance")]
    public void BlinqAnyNativeSequencePerformance()
    {
        bool result;

        MeasureBlinq(() => result = source.Any());
    }

    [Test, Performance]
    [Category("Performance")]
    public void LinqAnyNativeSequencePerformance()
    {
        bool result;

        MeasureLinq(() => result = LinqEnumerable.Any(source));
    }


    [Test, Performance]
    [Category("Performance")]
    public void BlinqAnyPredicateNativeSequencePerformance()
    {
        bool result;

        MeasureBlinq(() => result = source.Any<EqualsOne>());
    }

    [Test, Performance]
    [Category("Performance")]
    public void LinqAnyPredicateNativeSequencePerformance()
    {
        bool result;

        MeasureLinq(() => result = LinqEnumerable.Any(source, default(EqualsOne).Invoke));
    }

    private void MeasureLinq(Action method)
    {
        MakeMeasurement(method, "Linq");
    }

    private void MeasureBlinq(Action method)
    {
        MakeMeasurement(method, "Blinq");
    }

    private void MakeMeasurement(Action method, string name)
    {
        Measure.Method(method)
            .SampleGroup(name)
            .WarmupCount(100)
            .MeasurementCount(20)
            .IterationsPerMeasurement(100)
            .GC()
            .Run();
    }
}
