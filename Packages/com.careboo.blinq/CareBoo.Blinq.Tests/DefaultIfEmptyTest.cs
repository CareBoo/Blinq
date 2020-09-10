using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using Blinq = CareBoo.Blinq.NativeArrayExtensions;

internal class DefaultIfEmptyTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqValueSequenceDefaultIfEmpty([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = Blinq.ToValueSequence(ref sourceNativeArr);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.DefaultIfEmpty(source, 1)));
        var actual = ExceptionAndValue(() => Linq.ToArray(source.DefaultIfEmpty(1)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayDefaultIfEmpty([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.DefaultIfEmpty(source, 1)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.DefaultIfEmpty(ref source, 1)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
