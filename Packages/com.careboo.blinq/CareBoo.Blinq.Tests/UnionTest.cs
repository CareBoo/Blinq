using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using Blinq = CareBoo.Blinq.NativeArrayExtensions;

internal class UnionTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayUnion([ArrayValues] int[] sourceArr, [ArrayValues] int[] secondArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var second = new NativeArray<int>(secondArr, Allocator.Persistent);
        var (expectedException, expectedValue) = ExceptionOrValue(() => Linq.ToArray(Linq.Union(source, second)));
        var (actualException, actualValue) = ExceptionOrValue(() => Linq.ToArray(Blinq.Union(ref source, second)));
        Assert.AreEqual(expectedException, actualException);
        Assert.AreEqual(expectedValue, actualValue);
        source.Dispose();
        second.Dispose();
    }
}
