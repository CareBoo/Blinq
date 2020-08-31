using System.Collections.Generic;
using NUnit.Framework;
using LinqEnumerable = System.Linq.Enumerable;
using static Utils;
using Unity.Collections;
using Unity.Jobs;

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

    public struct CopyJob : IJob
    {
        [ReadOnly, DeallocateOnJobCompletion]
        public NativeArray<int> Input;

        [WriteOnly]
        public NativeList<int> Output;

        public void Execute()
        {
            for (var i = 0; i < Input.Length; i++)
                Output.Add(Input[i]);
        }
    }

    public struct CopyJob2 : IJob
    {
        [ReadOnly, DeallocateOnJobCompletion]
        public NativeArray<int> Input;

        [WriteOnly]
        public NativeArray<int> Output;

        public void Execute()
        {
            for (var i = 0; i < Input.Length; i++)
                Output[i] = Input[i];
        }
    }
}
