using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using static ValueFuncs;
using Unity.Collections;
using Blinq = CareBoo.Blinq.NativeArrayExtensions;

internal class SelectTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArraySelectWithIndex([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Select(source, AddToIndex.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Select(ref source, AddToIndex)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArraySelect([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Select(source, IntToLong.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Select(ref source, IntToLong)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
