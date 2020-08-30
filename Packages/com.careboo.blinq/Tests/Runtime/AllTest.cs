using System.Collections.Generic;
using LinqEnumerable = System.Linq.Enumerable;
using NUnit.Framework;
using static Utils;

internal class AllTest
{
    [Test, Parallelizable]
    public void BlinqAllShouldEqualLinqAllNativeSequence([EnumerableValues] IEnumerable<int> source)
    {
        var sequence = InitSequence(source);
        var expected = ExceptionOrValue(() => LinqEnumerable.All(sequence, default(EqualsZero).Invoke));
        var actual = ExceptionOrValue(() => sequence.All<EqualsZero>());
        Assert.AreEqual(expected, actual);
        sequence.Dispose();
    }
}
