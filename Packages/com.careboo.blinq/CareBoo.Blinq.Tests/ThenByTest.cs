using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using static ValueFuncs;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;
using CareBoo.Blinq;

internal class ThenByTest
{

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqValueSequenceThenBy([OrderValues] Order[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<Order>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.ThenBy(Linq.OrderBy(source, SelectSecond.Invoke), SelectFirst.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.ThenBy(Blinq.OrderBy(source, SelectSecond), SelectFirst)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqValueSequenceThenByDescending([OrderValues] Order[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<Order>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.ThenByDescending(Linq.OrderBy(source, SelectFirst.Invoke), SelectSecond.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.ThenByDescending(Blinq.OrderBy(source, SelectFirst), SelectSecond)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }
}
