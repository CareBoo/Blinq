using System.Collections.Generic;
using LinqEnumerable = System.Linq.Enumerable;
using NUnit.Framework;
using Unity.Collections;
using CareBoo.Blinq.Tests;
using CareBoo.Blinq;

internal class AllTest
{
    [Test, Parallelizable]
    public void BlinqAllShouldEqualLinqAllNativeSequence([EnumerableValues] IEnumerable<int> source)
    {
        var sequence = new NativeSequence<int>(LinqEnumerable.ToArray(source), Allocator.Persistent);
        var expected = LinqEnumerable.All(sequence, default(EqualsZero).Invoke);
        var actual = sequence.All<EqualsZero>();
        Assert.AreEqual(expected, actual);
    }
}
