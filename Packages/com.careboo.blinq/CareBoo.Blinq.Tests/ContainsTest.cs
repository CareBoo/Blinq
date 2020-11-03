using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using System.Collections.Generic;
using CareBoo.Blinq;

internal class ContainsTest
{
    public struct RemainderTwoComparer : IEqualityComparer<int>
    {
        public bool Equals(int x, int y)
        {
            return (x % 2).Equals(y % 2);
        }

        public int GetHashCode(int obj)
        {
            return obj;
        }
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqValueSequenceContains([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var sourceSeq = sourceNativeArr.ToValueSequence();
        var (expectedException, expectedValue) = ExceptionAndValue(() => Linq.Contains(sourceSeq, 7));
        var (actualException, actualValue) = ExceptionAndValue(() => sourceSeq.Contains(7));
        Assert.AreEqual(expectedException, actualException);
        Assert.AreEqual(expectedValue, actualValue);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayContains([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var (expectedException, expectedValue) = ExceptionAndValue(() => Linq.Contains(source, 7));
        var (actualException, actualValue) = ExceptionAndValue(() => source.Contains(7));
        Assert.AreEqual(expectedException, actualException);
        Assert.AreEqual(expectedValue, actualValue);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqValueSequenceContainsComparer([ArrayValues] int[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var sourceSeq = sourceNativeArr.ToValueSequence();
        var (expectedException, expectedValue) = ExceptionAndValue(() => Linq.Contains(sourceSeq, 7, default(RemainderTwoComparer)));
        var (actualException, actualValue) = ExceptionAndValue(() => sourceSeq.Contains(7, default(RemainderTwoComparer)));
        Assert.AreEqual(expectedException, actualException);
        Assert.AreEqual(expectedValue, actualValue);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayContainsComparer([ArrayValues] int[] sourceArr)
    {
        var source = new NativeArray<int>(sourceArr, Allocator.Persistent);
        var (expectedException, expectedValue) = ExceptionAndValue(() => Linq.Contains(source, 7, default(RemainderTwoComparer)));
        var (actualException, actualValue) = ExceptionAndValue(() => source.Contains(7, default(RemainderTwoComparer)));
        Assert.AreEqual(expectedException, actualException);
        Assert.AreEqual(expectedValue, actualValue);
        source.Dispose();
    }
}
