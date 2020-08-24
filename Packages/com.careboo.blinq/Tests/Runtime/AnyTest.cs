using System.Collections.Generic;
using LinqEnumerable = System.Linq.Enumerable;
using BlinqEnumerable = CareBoo.Blinq.Enumerable;
using NUnit.Framework;
using Unity.Collections;
using System;
using CareBoo.Blinq.Tests;

internal class AnyTest
{
    // [Test, Parallelizable]
    // public void BlinqAnyArrayShouldEqualLinqAny<T>([EnumerableValues] IEnumerable<T> source)
    //     where T : unmanaged
    // {
    //     var arraySource = LinqEnumerable.ToArray(source);
    //     var expected = LinqEnumerable.Any(arraySource);
    //     var actual = BlinqEnumerable.Any(arraySource);
    //     Assert.AreEqual(expected, actual);
    // }

    // [Test, Parallelizable]
    // public void BlinqAnyListShouldEqualLinqAny<T>([EnumerableValues] IEnumerable<T> source)
    //     where T : unmanaged
    // {
    //     var listSource = LinqEnumerable.ToList(source);
    //     var expected = LinqEnumerable.Any(listSource);
    //     var actual = BlinqEnumerable.Any(listSource);
    //     Assert.AreEqual(expected, actual);
    // }

    [Test, Parallelizable]
    public void BlinqAnyNativeArrayShouldEqualLinqAny<T>([EnumerableValues] IEnumerable<T> source)
        where T : unmanaged
    {
        var nativeArraySource = source.ToNativeArray(Allocator.Persistent);
        var expected = LinqEnumerable.Any(nativeArraySource);
        var actual = BlinqEnumerable.Any(nativeArraySource);
        Assert.AreEqual(expected, actual);
        nativeArraySource.Dispose();
    }

    // [Test, Parallelizable]
    // public void BlinqAnyArrayPredicateShouldEqualLinqAny<T>([EnumerableValues] IEnumerable<T> source)
    //     where T : unmanaged, IEquatable<T>
    // {
    //     var arraySource = LinqEnumerable.ToArray(source);
    //     var expected = LinqEnumerable.Any(arraySource, EqualsDefault<T>.Func);
    //     var actual = BlinqEnumerable.Any<T, EqualsDefault<T>>(arraySource);
    //     Assert.AreEqual(expected, actual);
    // }

    // [Test, Parallelizable]
    // public void BlinqAnyListPredicateShouldEqualLinqAny<T>([EnumerableValues] IEnumerable<T> source)
    //     where T : unmanaged, IEquatable<T>
    // {
    //     var listSource = LinqEnumerable.ToList(source);
    //     var expected = LinqEnumerable.Any(listSource, EqualsDefault<T>.Func);
    //     var actual = BlinqEnumerable.Any<T, EqualsDefault<T>>(listSource);
    //     Assert.AreEqual(expected, actual);
    // }

    [Test, Parallelizable]
    public void BlinqAnyNativeArrayPredicateShouldEqualLinqAny([EnumerableValues] IEnumerable<int> source)
    {
        var nativeArraySource = source.ToNativeArray(Allocator.Persistent);
        var expected = LinqEnumerable.Any(nativeArraySource, default(EqualsZero).Invoke);
        var actual = BlinqEnumerable.Any<int, EqualsZero>(nativeArraySource);
        Assert.AreEqual(expected, actual);
        nativeArraySource.Dispose();
    }
}
