using NUnit.Framework;
using Unity.PerformanceTesting;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using static ValueFuncs;
using CareBoo.Burst.Delegates;
using System.Collections.Generic;

[BurstCompile]
internal struct GroupByPerformanceJob<TKeySelector, TResultSelector> : IJob
    where TKeySelector : struct, IFunc<int, int>
    where TResultSelector : struct, IFunc<int, NativeMultiHashMap<int, int>, int>
{
    [ReadOnly]
    public NativeArray<int> Source;

    public ValueFunc<int, int>.Struct<TKeySelector> KeySelector;

    public ValueFunc<int, NativeMultiHashMap<int, int>, int>.Struct<TResultSelector> ResultSelector;

    public void Execute()
    {
        var result = Blinq.GroupBy(Source, KeySelector, ResultSelector).ToNativeList(Allocator.Temp);
        result.Dispose();
    }
}

internal class GroupByPerformanceTest : BaseBlinqPerformanceTest
{
    int GroupByLinqSelector(int key, IEnumerable<int> values)
    {
        return key + Linq.Count(values);
    }

    [Test, Performance, Category("Performance")]
    public void BlinqArray()
    {
        MakeMeasurement(() => new GroupByPerformanceJob<Functions.SelectSelf<int>, Functions.SelectGrouping> { Source = source }.Run(), "Blinq").Run();
    }

    [Test, Performance, Category("Performance")]
    public void LinqArray()
    {
        MakeMeasurement(() => Linq.ToList(Linq.GroupBy(source, SelectSelf<int>().Invoke, GroupByLinqSelector)), "Linq").Run();
    }
}
