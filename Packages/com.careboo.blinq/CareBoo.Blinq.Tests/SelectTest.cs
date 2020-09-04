using System.Collections.Generic;
using NUnit.Framework;
using LinqEnumerable = System.Linq.Enumerable;
using static Utils;

internal class SelectTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqSelectWithIndex([EnumerableValues] IEnumerable<int> source)
    {
        var sequence = InitSequence(source);
        var (expectedException, expectedValue) = ExceptionOrValue(() => LinqEnumerable.ToArray(LinqEnumerable.Select(sequence, default(AddToIndex).Invoke)));
        var (actualException, actualValue) = ExceptionOrValue(() => LinqEnumerable.ToArray(sequence.SelectWithIndex<long, AddToIndex>()));
        Assert.AreEqual(expectedException, actualException);
        Assert.AreEqual(expectedValue, actualValue);
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqSelect([EnumerableValues] IEnumerable<int> source)
    {
        var sequence = InitSequence(source);
        var (expectedException, expectedValue) = ExceptionOrValue(() => LinqEnumerable.ToArray(LinqEnumerable.Select(sequence, default(IntToLong).Invoke)));
        var (actualException, actualValue) = ExceptionOrValue(() => LinqEnumerable.ToArray(sequence.Select<long, IntToLong>()));
        Assert.AreEqual(expectedException, actualException);
        Assert.AreEqual(expectedValue, actualValue);
    }
}
