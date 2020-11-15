using CareBoo.Burst.Delegates;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;
using NUnit.Framework;
using static ValueFuncs;
using Unity.PerformanceTesting;

[BurstCompile]
internal struct AggregateJob<TFunc, TResultSelector> : IJob
    where TFunc : struct, IFunc<int, int, int>
    where TResultSelector : struct, IFunc<int, long>
{
    [ReadOnly]
    public NativeArray<int> Source;

    public ValueFunc<int, int, int>.Struct<TFunc> Func;

    public ValueFunc<int, long>.Struct<TResultSelector> ResultSelector;

    public void Execute()
    {
        Blinq.Aggregate(Source, 0, Func, ResultSelector);
    }
}

internal class AggregatePerformanceTest : BaseBlinqPerformanceTest
{
    [Test, Performance, Category("Performance")]
    public void BlinqNativeSequence()
    {
        MeasureBlinq(() => new AggregateJob<Functions.Sum, Functions.IntToLong> { Source = source, Func = Sum, ResultSelector = IntToLong }.Run()).Run();
    }

    [Test, Performance, Category("Performance")]
    public void LinqNativeSequence()
    {
        MeasureLinq(() => Linq.Aggregate(source, 0, Sum.Invoke, IntToLong.Invoke)).Run();
    }
}
