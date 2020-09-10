using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using Blinq = CareBoo.Blinq.NativeArrayExtensions;

internal class AllTest
{
    [Test, Parallelizable]
    public void BlinqAllShouldEqualLinqNativeArrayAll([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.All(source, default(EqualsZero).Invoke));
        var actual = ExceptionAndValue(() => Blinq.All<int, EqualsZero>(ref source));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
