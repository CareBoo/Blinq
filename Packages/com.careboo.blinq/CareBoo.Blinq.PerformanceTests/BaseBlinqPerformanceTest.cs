
using System;
using NUnit.Framework;
using Unity.Collections;
using Unity.PerformanceTesting;
using Unity.PerformanceTesting.Measurements;

internal class BaseBlinqPerformanceTest
{
    protected const int NumElements = 16 << 10;
    protected NativeArray<int> source;

    [OneTimeSetUp]
    public virtual void SetUpSource()
    {
        var arr = new int[NumElements];
        for (var i = 0; i < NumElements; i++)
            arr[i] = NumElements - 1 - i;
        source = new NativeArray<int>(arr, Allocator.Persistent);
    }

    [OneTimeTearDown]
    public virtual void TearDownSource()
    {
        source.Dispose();
    }

    protected MethodMeasurement MeasureLinq(Action method)
    {
        return MakeMeasurement(method, "Linq");
    }

    protected MethodMeasurement MeasureBlinq(Action method)
    {
        return MakeMeasurement(method, "Blinq");
    }

    protected MethodMeasurement MakeMeasurement(Action method, string name)
    {
        return Measure.Method(method)
            .SampleGroup(name)
            .WarmupCount(100)
            .MeasurementCount(20)
            .IterationsPerMeasurement(100)
            .GC();
    }
}
