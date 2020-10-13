using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using static ValueFuncs;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;
using CareBoo.Blinq;

internal class CountTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArraySequenceCount([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Count(sourceNativeArr));
        var actual = ExceptionAndValue(() => Blinq.Count(ref sourceNativeArr));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqValueSequenceCount([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.Count(source));
        var actual = ExceptionAndValue(() => Blinq.Count(source));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArraySequenceCountPredicate([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Count(sourceNativeArr, EqualsOne.Invoke));
        var actual = ExceptionAndValue(() => Blinq.Count(ref sourceNativeArr, EqualsOne));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqValueSequenceCountPredicate([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.Count(source, EqualsOne.Invoke));
        var actual = ExceptionAndValue(() => Blinq.Count(source, EqualsOne));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }
}
