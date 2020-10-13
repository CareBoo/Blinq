using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using static ValueFuncs;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;
using CareBoo.Blinq;

internal class SkipTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArraySkip([ArrayValues] int[] sourceArr)
    {
        var count = 5;
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Skip(source, count)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Skip(ref source, count)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArraySkipWhile([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.SkipWhile(source, EqualToIndex.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.SkipWhile(ref source, EqualToIndex)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqValueSequenceSkip([ArrayValues] int[] sourceArr)
    {
        var count = 5;
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Skip(source, count)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Skip(source, count)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqValueSequenceSkipWhile([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.SkipWhile(source, EqualToIndex.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.SkipWhile(source, EqualToIndex)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }
}
