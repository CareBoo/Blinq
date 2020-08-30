using System.Collections.Generic;
using LinqEnumerable = System.Linq.Enumerable;
using NUnit.Framework;
using Unity.Collections;
using CareBoo.Blinq;

internal class AnyTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeSequenceAny([EnumerableValues] IEnumerable<int> source)
    {
        var sequence = new NativeSequence<int>(LinqEnumerable.ToArray(source), Allocator.Persistent);
        var expected = LinqEnumerable.Any(sequence);
        var actual = sequence.Any();
        Assert.AreEqual(expected, actual);
        sequence.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeSequenceAnyPredicate([EnumerableValues] IEnumerable<int> source)
    {
        var sequence = new NativeSequence<int>(LinqEnumerable.ToArray(source), Allocator.Persistent);
        var expected = LinqEnumerable.Any(sequence, default(EqualsZero).Invoke);
        var actual = sequence.Any<EqualsZero>();
        Assert.AreEqual(expected, actual);
        sequence.Dispose();
    }
}
