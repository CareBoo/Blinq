using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Blinq = CareBoo.Blinq.Sequence;
using Unity.Collections;
using static ValueFuncs;

internal class AggregateTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayAggregateWithAccumulateAndResult([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Aggregate<int, long, double>(
            source,
            0,
            LongSum.Invoke,
            LongToDouble.Invoke
        ));
        var actual = ExceptionAndValue(() => Blinq.Aggregate(ref source, 0, LongSum, LongToDouble));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayAggregateWithAccumulate([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Aggregate<int, long>(
            source,
            0,
            LongSum.Invoke
        ));
        var actual = ExceptionAndValue(() => Blinq.Aggregate(ref source, 0, LongSum));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayAggregate([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Aggregate(
            source,
            Sum.Invoke
        ));
        var actual = ExceptionAndValue(() => Blinq.Aggregate(ref source, Sum));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
