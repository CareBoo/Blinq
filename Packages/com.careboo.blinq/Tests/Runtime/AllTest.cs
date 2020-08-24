using System.Collections.Generic;
using LinqEnumerable = System.Linq.Enumerable;
using BlinqEnumerable = CareBoo.Blinq.Enumerable;
using NUnit.Framework;
using Unity.Collections;
using System;
using CareBoo.Blinq.Tests;

internal class AllTest
{
    [Test, Parallelizable]
    public void BlinqAllArrayPredicateShouldEqualLinqAll([EnumerableValues] IEnumerable<int> source)
    {
        var arraySource = LinqEnumerable.ToArray(source);
        var expected = LinqEnumerable.All(arraySource, default(EqualsZero).Invoke);
        var actual = BlinqEnumerable.All<int, EqualsZero>(arraySource);
        Assert.AreEqual(expected, actual);
    }

    [Test, Parallelizable]
    public void BlinqAllListPredicateShouldEqualLinqAll([EnumerableValues] IEnumerable<int> source)
    {
        var listSource = LinqEnumerable.ToList(source);
        var expected = LinqEnumerable.All(listSource, default(EqualsZero).Invoke);
        var actual = BlinqEnumerable.All<int, EqualsZero>(listSource);
        Assert.AreEqual(expected, actual);
    }

    [Test, Parallelizable]
    public void BlinqAllNativePredicateShouldEqualLinqAll([EnumerableValues] IEnumerable<int> source)
    {
        var nativeArraySource = source.ToNativeArray(Allocator.Persistent);
        var expected = LinqEnumerable.All(nativeArraySource, default(EqualsZero).Invoke);
        var actual = BlinqEnumerable.All<int, EqualsZero>(nativeArraySource);
        Assert.AreEqual(expected, actual);
        nativeArraySource.Dispose();
    }
}
