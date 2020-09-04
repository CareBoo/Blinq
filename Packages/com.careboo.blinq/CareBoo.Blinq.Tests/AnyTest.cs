using System.Collections.Generic;
using LinqEnumerable = System.Linq.Enumerable;
using NUnit.Framework;
using static Utils;

internal class AnyTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeSequenceAny([EnumerableValues] IEnumerable<int> source)
    {
        var sequence = InitSequence(source);
        var expected = ExceptionOrValue(() => LinqEnumerable.Any(sequence));
        var actual = ExceptionOrValue(() => sequence.Any());
        Assert.AreEqual(expected, actual);
        sequence.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeSequenceAnyPredicate([EnumerableValues] IEnumerable<int> source)
    {
        var sequence = InitSequence(source);
        var expected = ExceptionOrValue(() => LinqEnumerable.Any(sequence, default(EqualsZero).Invoke));
        var actual = ExceptionOrValue(() => sequence.Any<EqualsZero>());
        Assert.AreEqual(expected, actual);
        sequence.Dispose();
    }
}
