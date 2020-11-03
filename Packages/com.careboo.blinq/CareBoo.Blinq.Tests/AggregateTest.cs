using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Blinq = CareBoo.Blinq.Sequence;
using Unity.Collections;
using static ValueFuncs;

internal class AggregateTest
{
    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayAggregateWithAccumulateAndResult([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Aggregate<int, long, double>(
            source,
            0,
            LongSum.Invoke,
            LongToDouble.Invoke
        ));
        var actual = ExceptionAndValue(() => Blinq.Aggregate(source, 0, LongSum, LongToDouble));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayAggregateWithAccumulate([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Aggregate<int, long>(
            source,
            0,
            LongSum.Invoke
        ));
        var actual = ExceptionAndValue(() => Blinq.Aggregate(source, 0, LongSum));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayAggregate([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Aggregate(
            source,
            Sum.Invoke
        ));
        var actual = ExceptionAndValue(() => Blinq.Aggregate(source, Sum));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
