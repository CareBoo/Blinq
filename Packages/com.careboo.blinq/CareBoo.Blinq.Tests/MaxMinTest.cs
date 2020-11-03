using Unity.Collections;
using static ValueFuncs;
using static Utils;
using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;
using System;

internal partial class MaxMinTest
{
    internal struct Comparable : IComparable<Comparable>
    {
        public int Value;

        public int CompareTo(Comparable other)
        {
            return Value == other.Value
                ? 0
                : Value > other.Value
                ? 1
                : -1;
        }

        public static implicit operator Comparable(int val)
        {
            return new Comparable { Value = val };
        }
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayMaxComparable([ArrayValues] int[] sourceArr)
    {
        var arr = Linq.ToArray(Linq.Select(sourceArr, (i) => (Comparable)i));
        var srcNativeArray = new NativeArray<Comparable>(arr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Max(srcNativeArray));
        var actual = ExceptionAndValue(() => Blinq.Max(srcNativeArray));
        srcNativeArray.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayMaxComparableSelector([ArrayValues] int[] sourceArr)
    {
        var arr = Linq.ToArray(Linq.Select(sourceArr, (i) => (Comparable)i));
        var srcNativeArray = new NativeArray<Comparable>(arr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Max(srcNativeArray, SelectSelf<Comparable>().Invoke));
        var actual = ExceptionAndValue(() => Blinq.Max(srcNativeArray, SelectSelf<Comparable>()));
        srcNativeArray.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayMinComparable([ArrayValues] int[] sourceArr)
    {
        var arr = Linq.ToArray(Linq.Select(sourceArr, (i) => (Comparable)i));
        var srcNativeArray = new NativeArray<Comparable>(arr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Min(srcNativeArray));
        var actual = ExceptionAndValue(() => Blinq.Min(srcNativeArray));
        srcNativeArray.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayMinComparableSelector([ArrayValues] int[] sourceArr)
    {
        var arr = Linq.ToArray(Linq.Select(sourceArr, (i) => (Comparable)i));
        var srcNativeArray = new NativeArray<Comparable>(arr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.Min(srcNativeArray, SelectSelf<Comparable>().Invoke));
        var actual = ExceptionAndValue(() => Blinq.Min(srcNativeArray, SelectSelf<Comparable>()));
        srcNativeArray.Dispose();
    }
}
