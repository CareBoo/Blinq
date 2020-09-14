using NUnit.Framework;
using Unity.Collections;
using static ValueFuncs;
using Blinq = CareBoo.Blinq.Sequence;
using Linq = System.Linq.Enumerable;
using static Utils;

internal class WhereTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayWhereWithIndex([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Where(source, EqualToIndex.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Where(ref source, EqualToIndex)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayWhere([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Where(source, EqualsZero.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Where(ref source, EqualsZero)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
