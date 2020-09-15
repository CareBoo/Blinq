using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using static ValueFuncs;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;
using CareBoo.Blinq;

internal class TakeTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayTake([ArrayValues] int[] sourceArr)
    {
        var count = 5;
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Take(source, count)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Take(ref source, count)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayTakeWhile([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.TakeWhile(source, EqualToIndex.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.TakeWhile(ref source, EqualToIndex)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqValueSequenceTake([ArrayValues] int[] sourceArr)
    {
        var count = 5;
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Take(source, count)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Take(source, count)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqValueSequenceTakeWhile([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.TakeWhile(source, EqualToIndex.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.TakeWhile(source, EqualToIndex)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }
}
