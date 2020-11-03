using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;
using CareBoo.Blinq;

internal class SequenceEqualTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArraySequenceEqualArray([ArrayValues] int[] sourceArr, [ArrayValues] int[] secondArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var secondNativeArr = new NativeArray<int>(secondArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.SequenceEqual(sourceNativeArr, secondNativeArr));
        var actual = ExceptionAndValue(() => Blinq.SequenceEqual(sourceNativeArr, secondNativeArr));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
        secondNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqValueSequenceEqualArray([ArrayValues] int[] sourceArr, [ArrayValues] int[] secondArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var secondNativeArr = new NativeArray<int>(secondArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.SequenceEqual(source, secondNativeArr));
        var actual = ExceptionAndValue(() => Blinq.SequenceEqual(source, secondNativeArr));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
        secondNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArraySequenceEqualSequence([ArrayValues] int[] sourceArr, [ArrayValues] int[] secondArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var secondNativeArr = new NativeArray<int>(secondArr, Allocator.Persistent);
        var second = secondNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.SequenceEqual(sourceNativeArr, second));
        var actual = ExceptionAndValue(() => Blinq.SequenceEqual(sourceNativeArr, second));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
        secondNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqValueSequenceEqualSequence([ArrayValues] int[] sourceArr, [ArrayValues] int[] secondArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var secondNativeArr = new NativeArray<int>(secondArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var second = secondNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.SequenceEqual(source, second));
        var actual = ExceptionAndValue(() => Blinq.SequenceEqual(source, second));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
        secondNativeArr.Dispose();
    }
}
