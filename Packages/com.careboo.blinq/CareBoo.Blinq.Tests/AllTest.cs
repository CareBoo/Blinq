using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using static ValueFuncs;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;

internal class AllTest
{
    [Test, Parallelizable]
    public void BlinqAllShouldEqualLinqNativeArrayAll([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.All(source, EqualsZero.Invoke));
        var actual = ExceptionAndValue(() => Blinq.All(ref source, EqualsZero));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
