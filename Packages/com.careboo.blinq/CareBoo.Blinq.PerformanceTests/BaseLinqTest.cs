
using System;
using NUnit.Framework;
using Unity.PerformanceTesting;
using Unity.PerformanceTesting.Measurements;

internal class BaseBlinqPerformanceTest
{
    protected const int NumElements = 16 << 10;
    protected int[] sourceArr;

    [OneTimeSetUp]
    public virtual void SetUpSource()
    {
        sourceArr = new int[NumElements];
        for (var i = 0; i < NumElements; i++)
            sourceArr[i] = NumElements - 1 - i;
    }

    [OneTimeTearDown]
    public virtual void TearDownSource()
    {
        sourceArr = null;
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
