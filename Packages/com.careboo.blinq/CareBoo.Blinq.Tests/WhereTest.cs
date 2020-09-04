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
        var (expectedException, expectedValue) = ExceptionOrValue(() => LinqEnumerable.ToArray(LinqEnumerable.Where(sequence, default(EqualToIndex).Invoke)));
        var (actualException, actualValue) = ExceptionOrValue(() => LinqEnumerable.ToArray(sequence.WhereWithIndex<EqualToIndex>()));
        Assert.AreEqual(expectedException, actualException);
        Assert.AreEqual(expectedValue, actualValue);
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqWhere([EnumerableValues] IEnumerable<int> source)
    {
        var sequence = InitSequence(source);
        var (expectedException, expectedValue) = ExceptionOrValue(() => LinqEnumerable.ToArray(LinqEnumerable.Where(sequence, default(EqualsZero).Invoke)));
        var (actualException, actualValue) = ExceptionOrValue(() => LinqEnumerable.ToArray(sequence.Where<EqualsZero>()));
        Assert.AreEqual(expectedException, actualException);
        Assert.AreEqual(expectedValue, actualValue);
    }
}
