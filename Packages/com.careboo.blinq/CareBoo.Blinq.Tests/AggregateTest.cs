using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Blinq = CareBoo.Blinq.NativeArrayExtensions;
using Unity.Collections;

internal class AggregateTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayAggregateWithAccumulateAndResult([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Aggregate<int, long, double>(
            source,
            0,
            default(LongSum).Invoke,
            default(LongToDouble).Invoke
        ));
        var actual = ExceptionAndValue(() => Blinq.Aggregate<int, long, double, LongSum, LongToDouble>(ref source, 0));
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
            default(LongSum).Invoke
        ));
        var actual = ExceptionAndValue(() => Blinq.Aggregate<int, long, LongSum>(ref source, 0));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayAggregate([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Aggregate(
            source,
            default(Sum).Invoke
        ));
        var actual = ExceptionAndValue(() => Blinq.Aggregate<int, Sum>(ref source));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
