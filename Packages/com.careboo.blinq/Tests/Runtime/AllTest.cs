using System.Collections.Generic;
using LinqEnumerable = System.Linq.Enumerable;
using NUnit.Framework;
using Unity.Collections;
using CareBoo.Blinq;
using static CareBoo.Blinq.Tests.Predicates;

internal class AllTest
{
    [Test, Parallelizable]
    public void BlinqAllShouldEqualLinqAllNativeSequence([EnumerableValues] IEnumerable<int> source)
    {
        var sequence = new NativeSequence<int>(LinqEnumerable.ToArray(source), Allocator.Persistent);
        var expected = LinqEnumerable.All(sequence.Copy(Allocator.Persistent), EqualsZero);
        var actual = sequence.All(EqualsZeroFunc);
        Assert.AreEqual(expected, actual);
    }
}
