using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;

internal class ExceptTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayExcept([ArrayValues] int[] sourceArr, [ArrayValues] int[] secondArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var second = new NativeArray<int>(secondArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Except(source, second)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Except(source, second)));
        AssertAreEqual(expected, actual);
        source.Dispose();
        second.Dispose();
    }
}
