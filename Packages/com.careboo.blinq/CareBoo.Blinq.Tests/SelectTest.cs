using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using Blinq = CareBoo.Blinq.NativeArrayExtensions;

internal class SelectTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArraySelectWithIndex([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Select(source, default(AddToIndex).Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.SelectWithIndex<int, long, AddToIndex>(ref source)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArraySelect([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Select(source, default(IntToLong).Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Select<int, long, IntToLong>(ref source)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
