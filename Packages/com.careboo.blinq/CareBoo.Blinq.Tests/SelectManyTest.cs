using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using static ValueFuncs;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;
using CareBoo.Blinq;

internal class SelectManyTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArraySelectMany([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.SelectMany(sourceNativeArr, (x) => RepeatAmount.Invoke(x))));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.SelectMany(ref sourceNativeArr, RepeatAmount)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArraySelectManyIndex([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.SelectMany(sourceNativeArr, (x, i) => RepeatAmountPlusIndex.Invoke(x, i))));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.SelectMany(ref sourceNativeArr, RepeatAmountPlusIndex)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArraySelectManyResultSelector([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.SelectMany(sourceNativeArr, (x) => RepeatAmount.Invoke(x), AddToIndex.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.SelectMany(ref sourceNativeArr, RepeatAmount, AddToIndex)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArraySelectManyIndexResultSelector([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.SelectMany(sourceNativeArr, (x, i) => RepeatAmountPlusIndex.Invoke(x, i), AddToIndex.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.SelectMany(ref sourceNativeArr, RepeatAmountPlusIndex, AddToIndex)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqValueSequenceSelectMany([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.SelectMany(source, (x) => RepeatAmount.Invoke(x))));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.SelectMany(source, RepeatAmount)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqValueSequenceSelectManyIndex([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.SelectMany(source, (x, i) => RepeatAmountPlusIndex.Invoke(x, i))));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.SelectMany(source, RepeatAmountPlusIndex)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqValueSequenceSelectManyResultSelector([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.SelectMany(source, (x) => RepeatAmount.Invoke(x), AddToIndex.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.SelectMany(source, RepeatAmount, AddToIndex)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqValueSequenceSelectManyIndexResultSelector([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.SelectMany(source, (x, i) => RepeatAmountPlusIndex.Invoke(x, i), AddToIndex.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.SelectMany(source, RepeatAmountPlusIndex, AddToIndex)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }
}
