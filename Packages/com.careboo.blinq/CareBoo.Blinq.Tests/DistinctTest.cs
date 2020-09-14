using NUnit.Framework;
using Unity.Collections;
using Blinq = CareBoo.Blinq.NativeArrayExtensions;
using Linq = System.Linq.Enumerable;
using static Utils;

internal class DistinctTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayDistinct([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Distinct(source)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Distinct(ref source)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
