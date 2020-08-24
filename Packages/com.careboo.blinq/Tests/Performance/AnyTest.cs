using NUnit.Framework;
using Unity.Collections;
using Unity.PerformanceTesting;
using LinqEnumerable = System.Linq.Enumerable;
using BlinqEnumerable = CareBoo.Blinq.Enumerable;
using System;
using System.Collections.Generic;

internal class AnyTest
{
    const int NumElements = 16 << 10;
    int[] sourceArray;
    List<int> sourceList;
    NativeArray<int> sourceNativeArray;

    [OneTimeSetUp]
    public void SetUpSources()
    {
        sourceArray = new int[NumElements];
        sourceArray[NumElements / 2] = 1;
        sourceList = LinqEnumerable.ToList(sourceArray);
        sourceNativeArray = new NativeArray<int>(sourceArray, Allocator.Persistent);
    }

    [OneTimeTearDown]
    public void TearDownSources()
    {
        sourceArray = null;
        sourceList = null;
        sourceNativeArray.Dispose();
    }

    // [Test, Performance]
    // [Category("Performance")]
    // public void BlinqAnyArrayPerformanceComparedToLinqAny()
    // {
    //     bool result;

    //     MeasureLinq(() => result = LinqEnumerable.Any(sourceArray));
    //     MeasureBlinq(() => result = BlinqEnumerable.Any(sourceArray));
    // }

    // [Test, Performance]
    // [Category("Performance")]
    // public void BlinqAnyListPerformanceComparedToLinqAny()
    // {
    //     bool result;

    //     MeasureLinq(() => result = LinqEnumerable.Any(sourceList));
    //     MeasureBlinq(() => result = BlinqEnumerable.Any(sourceList));
    // }

    [Test, Performance]
    [Category("Performance")]
    public void BlinqAnyNativeArrayPerformanceComparedToLinqAny()
    {
        bool result;

        MeasureLinq(() => result = LinqEnumerable.Any(sourceNativeArray));
        MeasureBlinq(() => result = BlinqEnumerable.Any(sourceNativeArray));
    }

    // [Test, Performance]
    // [Category("Performance")]
    // public void BlinqAnyArrayPredicatePerformanceComparedToLinqAny()
    // {
    //     bool result;

    //     MeasureLinq(() => result = LinqEnumerable.Any(sourceArray, default(EqualsOne).Invoke));
    //     MeasureBlinq(() => result = BlinqEnumerable.Any<int, EqualsOne>(sourceArray));
    // }

    // [Test, Performance]
    // [Category("Performance")]
    // public void BlinqAnyListPredicatePerformanceComparedToLinqAny()
    // {
    //     bool result;

    //     MeasureLinq(() => result = LinqEnumerable.Any(sourceList, default(EqualsOne).Invoke));
    //     MeasureBlinq(() => result = BlinqEnumerable.Any<int, EqualsOne>(sourceList));
    // }

    [Test, Performance]
    [Category("Performance")]
    public void BlinqAnyNativeArrayPredicatePerformanceComparedToLinqAny()
    {
        bool result;

        MeasureLinq(() => result = LinqEnumerable.Any(sourceNativeArray, default(EqualsOne).Invoke));
        MeasureBlinq(() => result = BlinqEnumerable.Any<int, EqualsOne>(sourceNativeArray));
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
