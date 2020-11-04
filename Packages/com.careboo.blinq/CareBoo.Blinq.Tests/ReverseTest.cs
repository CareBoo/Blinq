using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using static ValueFuncs;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;
using CareBoo.Blinq;

internal class ReverseTest
{
    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayReverse([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Reverse(source)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Reverse(source)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqValueSequenceReverse([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Reverse(source)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Reverse(source)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }
}
