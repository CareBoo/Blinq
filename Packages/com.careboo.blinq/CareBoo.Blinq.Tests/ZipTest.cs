using NUnit.Framework;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;
using Linq = System.Linq.Enumerable;
using static Utils;
using static ValueFuncs;
using CareBoo.Blinq;
using static CareBoo.Blinq.Sequence;

internal class ZipTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayZip([ArrayValues] int[] sourceArr, [ArrayValues] int[] secondArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var second = new NativeArray<int>(secondArr, Allocator.Persistent);
        var secondSequence = second.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Zip(source, secondSequence, Sum.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Zip(ref source, secondSequence, Sum)));
        AssertAreEqual(expected, actual);
        source.Dispose();
        second.Dispose();
    }
}
