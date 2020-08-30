using System.Collections.Generic;
using NUnit.Framework;
using LinqEnumerable = System.Linq.Enumerable;
using static Utils;

internal class WhereTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqWhereWithIndex([EnumerableValues] IEnumerable<int> source)
    {
        var sequence = InitSequence(source);
        var expected = ExceptionOrValue(() => LinqEnumerable.Where(source, default(LessThanIndex).Invoke));
        var actual = ExceptionOrValue(() => sequence.WhereWithIndex<LessThanIndex>());
        Assert.AreEqual(expected, actual);
        sequence.Dispose();
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqWhere([EnumerableValues] IEnumerable<int> source)
    {
        var sequence = InitSequence(source);
        var expected = ExceptionOrValue(() => LinqEnumerable.Where(source, default(EqualsZero).Invoke));
        var actual = ExceptionOrValue(() => sequence.Where<EqualsZero>());
        Assert.AreEqual(expected, actual);
        sequence.Dispose();
    }
}
