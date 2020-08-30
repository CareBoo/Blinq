﻿using System.Collections.Generic;
using CareBoo.Blinq;
using NUnit.Framework;
using Unity.Collections;
using LinqEnumerable = System.Linq.Enumerable;
using static Utils;

internal class AggregateTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeSequenceAggregateWithAccumulateAndResult([EnumerableValues] IEnumerable<int> source)
    {
        var sequence = new NativeSequence<int>(LinqEnumerable.ToArray(source), Allocator.Persistent);
        var expected = ExceptionOrValue(() => LinqEnumerable.Aggregate<int, long, double>(source, 0, default(LongSum).Invoke, default(ToDouble).Invoke));
        var actual = ExceptionOrValue(() => sequence.Aggregate<long, double, LongSum, ToDouble>(0));
        Assert.AreEqual(expected, actual);
        sequence.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeSequenceAggregateWithAccumulate([EnumerableValues] IEnumerable<int> source)
    {
        var sequence = new NativeSequence<int>(LinqEnumerable.ToArray(source), Allocator.Persistent);
        var expected = ExceptionOrValue(() => LinqEnumerable.Aggregate<int, long>(source, 0, default(LongSum).Invoke));
        var actual = ExceptionOrValue(() => sequence.Aggregate<long, LongSum>(0));
        Assert.AreEqual(expected, actual);
        sequence.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeSequenceAggregate([EnumerableValues] IEnumerable<int> source)
    {
        var sequence = new NativeSequence<int>(LinqEnumerable.ToArray(source), Allocator.Persistent);
        var expected = ExceptionOrValue(() => LinqEnumerable.Aggregate(source, default(Sum).Invoke));
        var actual = ExceptionOrValue(() => sequence.Aggregate<Sum>());
        Assert.AreEqual(expected, actual);
        sequence.Dispose();
    }
}
