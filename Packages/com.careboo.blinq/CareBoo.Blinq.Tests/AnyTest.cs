using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using static ValueFuncs;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;

internal class AnyTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayAny([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Any(source));
        var actual = ExceptionAndValue(() => Blinq.Any(ref source));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayAnyPredicate([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Any(source, EqualsZero.Invoke));
        var actual = ExceptionAndValue(() => Blinq.Any(ref source, EqualsZero));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
