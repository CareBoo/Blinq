using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;
using CareBoo.Blinq;

internal class ConcatTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayAppendArray([ArrayValues] int[] sourceArr, [ArrayValues] int[] secondArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var second = new NativeArray<int>(secondArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Concat(source, second)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Concat(source, second)));
        AssertAreEqual(expected, actual);
        source.Dispose();
        second.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqValueSequenceAppendArray([ArrayValues] int[] sourceArr, [ArrayValues] int[] secondArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var second = new NativeArray<int>(secondArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Concat(source, second)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Concat(source, second)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
        second.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayAppendSequence([ArrayValues] int[] sourceArr, [ArrayValues] int[] secondArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var secondNativeArr = new NativeArray<int>(secondArr, Allocator.Persistent);
        var second = secondNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Concat(source, second)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Concat(source, second)));
        AssertAreEqual(expected, actual);
        source.Dispose();
        secondNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqValueSequenceAppendSequence([ArrayValues] int[] sourceArr, [ArrayValues] int[] secondArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var secondNativeArr = new NativeArray<int>(secondArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var second = secondNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Concat(source, second)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Concat(source, second)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
        secondNativeArr.Dispose();
    }
}
