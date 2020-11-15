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
internal struct GroupJoinPerformanceJob<TOuterKeySelector, TInnerKeySelector, TResultSelector> : IJob
    where TOuterKeySelector : struct, IFunc<JoinA, int>
    where TInnerKeySelector : struct, IFunc<JoinB, int>
    where TResultSelector : struct, IFunc<JoinA, NativeArray<JoinB>, int>
{
    [ReadOnly]
    public NativeArray<JoinA> Outer;

    [ReadOnly]
    public NativeArray<JoinB> Inner;

    public ValueFunc<JoinA, int>.Struct<TOuterKeySelector> OuterKeySelector;
    public ValueFunc<JoinB, int>.Struct<TInnerKeySelector> InnerKeySelector;
    public ValueFunc<JoinA, NativeArray<JoinB>, int>.Struct<TResultSelector> ResultSelector;

    public void Execute()
    {
        var result = Blinq.GroupJoin(Outer, Inner, OuterKeySelector, InnerKeySelector, ResultSelector).ToNativeList(Allocator.Temp);
        result.Dispose();
    }
}

internal class GroupJoinPerformanceTest : BaseBlinqPerformanceTest
{
    NativeArray<JoinA> outer;
    NativeArray<JoinB> inner;

    public override void SetUpSource()
    {
        outer = new NativeArray<JoinA>(1000, Allocator.Persistent);
        for (var i = 0; i < outer.Length; i++)
            outer[i] = new JoinA { Id = i, Val = (char)(i % 26 + (int)'a') };
        inner = new NativeArray<JoinB>(10000, Allocator.Persistent);
        for (var i = 0; i < inner.Length; i++)
            inner[i] = new JoinB { Id = i % 1000, Val = (char)(i % 26 + (int)'A') };
    }

    public override void TearDownSource()
    {
        if (outer.IsCreated)
            outer.Dispose();
        if (inner.IsCreated)
            inner.Dispose();
    }

    int GroupJoinLinqSelector(JoinA o, IEnumerable<JoinB> b)
    {
        return Linq.Count(b);
    }

    [Test, Performance, Category("Performance")]
    public void BlinqArray()
    {
        MakeMeasurement(() => new GroupJoinPerformanceJob<Functions.JoinAKeySelector, Functions.JoinBKeySelector, Functions.GroupJoinABSelector> { Outer = outer, Inner = inner }.Run(), "Blinq").Run();
    }

    [Test, Performance, Category("Performance")]
    public void LinqArray()
    {
        MakeMeasurement(() => Linq.ToList(Linq.GroupJoin<JoinA, JoinB, int, int>(outer, inner, JoinAKeySelector.Invoke, JoinBKeySelector.Invoke, GroupJoinLinqSelector)), "Linq").Run();
    }
}
