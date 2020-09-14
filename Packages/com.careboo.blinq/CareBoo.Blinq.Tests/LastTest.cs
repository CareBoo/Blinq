using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using CareBoo.Blinq;
using static ValueFuncs;

internal class LastTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayLast([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Last(source));
        var actual = ExceptionAndValue(() => source.Last());
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayLastPredicate([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Last(source, EqualsZero.Invoke));
        var actual = ExceptionAndValue(() => source.Last(EqualsZero));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeSequenceLast([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.Last(source));
        var actual = ExceptionAndValue(() => source.Last());
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeSequenceLastPredicate([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.Last(source, EqualsZero.Invoke));
        var actual = ExceptionAndValue(() => source.Last(EqualsZero));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayLastOrDefault([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.LastOrDefault(source));
        var actual = ExceptionAndValue(() => source.LastOrDefault());
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayLastOrDefaultPredicate([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.LastOrDefault(source, EqualsZero.Invoke));
        var actual = ExceptionAndValue(() => source.LastOrDefault(EqualsZero));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeSequenceLastOrDefault([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.LastOrDefault(source));
        var actual = ExceptionAndValue(() => source.LastOrDefault());
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeSequenceLastOrDefaultPredicate([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.LastOrDefault(source, EqualsZero.Invoke));
        var actual = ExceptionAndValue(() => source.LastOrDefault(EqualsZero));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }
}
