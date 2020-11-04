using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using static ValueFuncs;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;

internal class SelectTest
{
    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArraySelectWithIndex([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Select(source, AddToIndex.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Select(source, AddToIndex)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArraySelect([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Select(source, IntToLong.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Select(source, IntToLong)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
