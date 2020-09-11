using NUnit.Framework;
using Unity.PerformanceTesting;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.NativeArrayExtensions;
using static ValueFuncs;
using Unity.Burst;
using Unity.Collections;
using CareBoo.Blinq;
using Unity.Jobs;

[BurstCompile]
internal struct AnyJob : IJob
{
    [ReadOnly]
    public NativeArray<int> Source;

    [ReadOnly]
    public ValueFunc<int, bool> Predicate;

    public void Execute()
    {
        Blinq.Any(ref Source, Predicate);
    }
}

internal class AnyTest : BaseBlinqPerformanceTest
{
    [Test, Performance]
    [Category("Performance")]
    public void BlinqAnyNativeSequencePerformance()
    {
        bool result;

        MeasureBlinq(() => result = Blinq.Any(ref source)).Run();
    }

    [Test, Performance]
    [Category("Performance")]
    public void LinqAnyNativeSequencePerformance()
    {
        bool result;

        MeasureLinq(() => result = Linq.Any(source)).Run();
    }


    [Test, Performance]
    [Category("Performance")]
    public void BlinqAnyPredicateNativeSequencePerformance()
    {
        MeasureBlinq(() => new AnyJob { Source = source, Predicate = EqualsOne }.Run()).Run();
    }

    [Test, Performance]
    [Category("Performance")]
    public void LinqAnyPredicateNativeSequencePerformance()
    {
        bool result;

        MeasureLinq(() => result = Linq.Any(source, Functions.EqualsOne)).Run();
    }
}
