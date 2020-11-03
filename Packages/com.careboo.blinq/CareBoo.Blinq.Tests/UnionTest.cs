using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;

internal class UnionTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayUnion([ArrayValues] int[] sourceArr, [ArrayValues] int[] secondArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var second = new NativeArray<int>(secondArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Union(source, second)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Union(source, second)));
        AssertAreEqual(expected, actual);
        source.Dispose();
        second.Dispose();
    }
}
