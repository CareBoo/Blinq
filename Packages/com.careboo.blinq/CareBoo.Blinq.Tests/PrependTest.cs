using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;
using CareBoo.Blinq;

internal class PrependTest
{
    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayPrepend([ArrayValues] int[] sourceArr)
    {
        int item = 1;
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Prepend(source, item)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Prepend(source, item)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqValueSequencePrepend([ArrayValues] int[] sourceArr)
    {
        int item = 1;
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Prepend(source, item)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Prepend(source, item)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }
}
