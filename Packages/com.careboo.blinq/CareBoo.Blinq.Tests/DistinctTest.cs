using NUnit.Framework;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;
using Linq = System.Linq.Enumerable;
using static Utils;

internal class DistinctTest
{
    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayDistinct([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Distinct(source)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Distinct(source)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
