using NUnit.Framework;
using Unity.Collections;
using Blinq = CareBoo.Blinq.NativeArrayExtensions;
using Linq = System.Linq.Enumerable;
using static Utils;

internal class WhereTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayWhereWithIndex([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Where(source, default(EqualToIndex).Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.WhereWithIndex<int, EqualToIndex>(ref source)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayWhere([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Where(source, default(EqualsZero).Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Where<int, EqualsZero>(ref source)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }
}
