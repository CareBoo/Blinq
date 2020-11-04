//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Unity.PerformanceTesting;
using Unity.PerformanceTesting.Measurements;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using CareBoo.Burst.Delegates;
using System.Collections;
using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;

internal class AverageTest
{
    protected MethodMeasurement MakeMeasurement(string name, Action method)
    {
        return Measure.Method(method)
            .SampleGroup(name)
            .WarmupCount(100)
            .MeasurementCount(20)
            .IterationsPerMeasurement(20)
            .GC();
    }

    internal class SelectValues
    {
        public static IEnumerable Values
        {
            get
            {
                yield return new int[1];
                yield return Linq.ToArray(Linq.Repeat(1, 100000));
            }
        }
    }

    [BurstCompile]
    internal struct AverageJob_int : IJob
    {
        [ReadOnly]
        public NativeArray<int> Source;

        public void Execute()
        {
            Blinq.Average(Source);
        }
    }

    [BurstCompile]
    internal struct AverageSelectJob_int<TSelector> : IJob
        where TSelector : struct, IFunc<int, int>
    {
        [ReadOnly]
        public NativeArray<int> Source;

        public ValueFunc<int, int>.Struct<TSelector> Selector;

        public void Execute()
        {
            Blinq.Average(Source, Selector);
        }
    }

    internal class Values_int
    {
        public static IEnumerable Values
        {
            get
            {
                yield return new int[1];
                yield return Linq.ToArray(Linq.Repeat((int)1, 100000));
            }
        }
    }

    internal struct StructIntTo_int : IFunc<int, int>
    {
        public int Invoke(int val) => (int)val;
    }

    internal static readonly ValueFunc<int, int>.Struct<StructIntTo_int> IntTo_int =
        ValueFunc<int, int>.New<StructIntTo_int>();

    [Test, Performance, Category("Performance")]
    public void AveragePerformance_int(
        [ValueSource(typeof(Values_int), nameof(Values_int.Values))] int[] sourceArr
        )
    {
        var src = new NativeArray<int>(sourceArr, Allocator.Persistent);
        MakeMeasurement("Blinq_int", () => new AverageJob_int { Source = src, }.Run()).Run();
        MakeMeasurement("Linq_int", () => Linq.Average(src)).Run();
        src.Dispose();
    }

    [Test, Performance, Category("Performance")]
    public void AverageSelectPerformance_int(
        [ValueSource(typeof(SelectValues), nameof(SelectValues.Values))] int[] sourceArr
        )
    {
        var src = new NativeArray<int>(sourceArr, Allocator.Persistent);
        MakeMeasurement("Blinq_int", () => new AverageSelectJob_int<StructIntTo_int> { Source = src, Selector = IntTo_int }.Run()).Run();
        MakeMeasurement("Linq_int", () => Linq.Average(src, IntTo_int.Invoke)).Run();
        src.Dispose();
    }

    [BurstCompile]
    internal struct AverageJob_float : IJob
    {
        [ReadOnly]
        public NativeArray<float> Source;

        public void Execute()
        {
            Blinq.Average(Source);
        }
    }

    [BurstCompile]
    internal struct AverageSelectJob_float<TSelector> : IJob
        where TSelector : struct, IFunc<int, float>
    {
        [ReadOnly]
        public NativeArray<int> Source;

        public ValueFunc<int, float>.Struct<TSelector> Selector;

        public void Execute()
        {
            Blinq.Average(Source, Selector);
        }
    }

    internal class Values_float
    {
        public static IEnumerable Values
        {
            get
            {
                yield return new float[1];
                yield return Linq.ToArray(Linq.Repeat((float)1, 100000));
            }
        }
    }

    internal struct StructIntTo_float : IFunc<int, float>
    {
        public float Invoke(int val) => (float)val;
    }

    internal static readonly ValueFunc<int, float>.Struct<StructIntTo_float> IntTo_float =
        ValueFunc<int, float>.New<StructIntTo_float>();

    [Test, Performance, Category("Performance")]
    public void AveragePerformance_float(
        [ValueSource(typeof(Values_float), nameof(Values_float.Values))] float[] sourceArr
        )
    {
        var src = new NativeArray<float>(sourceArr, Allocator.Persistent);
        MakeMeasurement("Blinq_float", () => new AverageJob_float { Source = src, }.Run()).Run();
        MakeMeasurement("Linq_float", () => Linq.Average(src)).Run();
        src.Dispose();
    }

    [Test, Performance, Category("Performance")]
    public void AverageSelectPerformance_float(
        [ValueSource(typeof(SelectValues), nameof(SelectValues.Values))] int[] sourceArr
        )
    {
        var src = new NativeArray<int>(sourceArr, Allocator.Persistent);
        MakeMeasurement("Blinq_float", () => new AverageSelectJob_float<StructIntTo_float> { Source = src, Selector = IntTo_float }.Run()).Run();
        MakeMeasurement("Linq_float", () => Linq.Average(src, IntTo_float.Invoke)).Run();
        src.Dispose();
    }

    [BurstCompile]
    internal struct AverageJob_double : IJob
    {
        [ReadOnly]
        public NativeArray<double> Source;

        public void Execute()
        {
            Blinq.Average(Source);
        }
    }

    [BurstCompile]
    internal struct AverageSelectJob_double<TSelector> : IJob
        where TSelector : struct, IFunc<int, double>
    {
        [ReadOnly]
        public NativeArray<int> Source;

        public ValueFunc<int, double>.Struct<TSelector> Selector;

        public void Execute()
        {
            Blinq.Average(Source, Selector);
        }
    }

    internal class Values_double
    {
        public static IEnumerable Values
        {
            get
            {
                yield return new double[1];
                yield return Linq.ToArray(Linq.Repeat((double)1, 100000));
            }
        }
    }

    internal struct StructIntTo_double : IFunc<int, double>
    {
        public double Invoke(int val) => (double)val;
    }

    internal static readonly ValueFunc<int, double>.Struct<StructIntTo_double> IntTo_double =
        ValueFunc<int, double>.New<StructIntTo_double>();

    [Test, Performance, Category("Performance")]
    public void AveragePerformance_double(
        [ValueSource(typeof(Values_double), nameof(Values_double.Values))] double[] sourceArr
        )
    {
        var src = new NativeArray<double>(sourceArr, Allocator.Persistent);
        MakeMeasurement("Blinq_double", () => new AverageJob_double { Source = src, }.Run()).Run();
        MakeMeasurement("Linq_double", () => Linq.Average(src)).Run();
        src.Dispose();
    }

    [Test, Performance, Category("Performance")]
    public void AverageSelectPerformance_double(
        [ValueSource(typeof(SelectValues), nameof(SelectValues.Values))] int[] sourceArr
        )
    {
        var src = new NativeArray<int>(sourceArr, Allocator.Persistent);
        MakeMeasurement("Blinq_double", () => new AverageSelectJob_double<StructIntTo_double> { Source = src, Selector = IntTo_double }.Run()).Run();
        MakeMeasurement("Linq_double", () => Linq.Average(src, IntTo_double.Invoke)).Run();
        src.Dispose();
    }

}
