using NUnit.Framework;
using Linq = System.Linq.Enumerable;
using static Utils;
using static ValueFuncs;
using Unity.Collections;
using Blinq = CareBoo.Blinq.Sequence;
using CareBoo.Blinq;
using System.Collections;
using System;

internal class OrderByTest
{
    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayOrderBy([OrderValues] Order[] sourceArr)
    {
        var source = new NativeArray<Order>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.OrderBy(source, SelectSecond.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.OrderBy(source, SelectSecond)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqValueSequenceOrderBy([OrderValues] Order[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<Order>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.OrderBy(source, SelectSecond.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.OrderBy(source, SelectSecond)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqNativeArrayOrderByDescending([OrderValues] Order[] sourceArr)
    {
        var source = new NativeArray<Order>(sourceArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.OrderByDescending(source, SelectFirst.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.OrderByDescending(source, SelectFirst)));
        AssertAreEqual(expected, actual);
        source.Dispose();
    }

    [Test, Parallelizable, Timeout(5000)]
    public void BlinqShouldEqualLinqValueSequenceOrderByDescending([OrderValues] Order[] sourceArr)
    {
        var sourceNativeArr = new NativeArray<Order>(sourceArr, Allocator.Persistent);
        var source = sourceNativeArr.ToValueSequence();
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.OrderByDescending(source, SelectFirst.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.OrderByDescending(source, SelectFirst)));
        AssertAreEqual(expected, actual);
        sourceNativeArr.Dispose();
    }
}

internal struct Order : IEquatable<Order>
{
    public int First;
    public int Second;

    public Order(int first, int second)
    {
        First = first;
        Second = second;
    }

    public bool Equals(Order other)
    {
        return other.First == First && other.Second == Second;
    }

    public override string ToString() => $"Order({First},{Second})";
}

internal class OrderValuesAttribute : ValueSourceAttribute
{
    public OrderValuesAttribute() : base(typeof(OrderValues), nameof(OrderValues.Values)) { }
}

internal class OrderValues
{
    public static IEnumerable Values
    {
        get
        {
            yield return Range(3);
            yield return Linq.ToArray(Linq.Concat(Range(2), Range(2)));
            yield return Range(2);
        }
    }

    static Order[] Range(int length)
    {
        var orders = new Order[length * length];
        for (var i = 0; i < length; i++)
            for (var j = 0; j < length; j++)
                orders[length * i + j] = new Order(i, j);
        return orders;
    }
}
