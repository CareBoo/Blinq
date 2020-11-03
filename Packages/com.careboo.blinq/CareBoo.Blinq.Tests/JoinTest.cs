using static ValueFuncs;
using static Utils;
using System.Collections;
using NUnit.Framework;
using Unity.Collections;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;

internal struct JoinA
{
    public int Id;
    public char Val;
}

internal struct JoinB
{
    public int Id;
    public char Val;
}

internal struct JointAB
{
    public char ValA;
    public char ValB;
}

internal class JoinAValues
{
    public static IEnumerable Values
    {
        get
        {
            yield return new[] { new JoinA { Id = 1, Val = 'a' }, new JoinA { Id = 2, Val = 'b' }, new JoinA { Id = 3, Val = 'c' } };
            yield return new JoinA[0];
        }
    }
}

internal class JoinBValues
{
    public static IEnumerable Values
    {
        get
        {
            yield return new[] { new JoinB { Id = 1, Val = 'a' }, new JoinB { Id = 2, Val = 'b' }, new JoinB { Id = 3, Val = 'c' } };
            yield return new JoinB[0];
        }
    }
}

internal class JoinTest
{
    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayJoinNativeArray(
        [ValueSource(typeof(JoinAValues), nameof(JoinAValues.Values))] JoinA[] outerArr,
        [ValueSource(typeof(JoinBValues), nameof(JoinBValues.Values))] JoinB[] innerArr
        )
    {
        var outer = new NativeArray<JoinA>(outerArr, Allocator.Persistent);
        var inner = new NativeArray<JoinB>(innerArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.Join(outer, inner, JoinAKeySelector.Invoke, JoinBKeySelector.Invoke, JointABSelector.Invoke)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.Join(outer, inner, JoinAKeySelector, JoinBKeySelector, JointABSelector)));
        AssertAreEqual(expected, actual);
        outer.Dispose();
        inner.Dispose();
    }
}
