using NUnit.Framework;
using Unity.Collections;
using static ValueFuncs;
using Blinq = CareBoo.Blinq.Sequence;
using Linq = System.Linq.Enumerable;
using static Utils;

internal class WhereTest
{
    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayWhereWithIndex([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Where(source, EqualToIndex.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Where(source, EqualToIndex)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayWhere([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Where(source, EqualsZero.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Where(source, EqualsZero)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
