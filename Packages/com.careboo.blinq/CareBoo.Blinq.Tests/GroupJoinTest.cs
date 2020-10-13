using static ValueFuncs;
using static Utils;
using System.Collections;
using NUnit.Framework;
using Unity.Collections;
using Linq = System.Linq.Enumerable;
using Blinq = CareBoo.Blinq.Sequence;
using System.Collections.Generic;

internal class GroupJoinTest
{
    internal class GroupJoinOuterValues
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

    internal class GroupJoinInnerValues
    {
        public static IEnumerable Values
        {
            get
            {
                yield return new[] { new JoinB { Id = 1, Val = 'a' }, new JoinB { Id = 1, Val = 'b' }, new JoinB { Id = 3, Val = 'c' } };
                yield return new JoinB[0];
            }
        }
    }

    public int GroupJoinABLinqSelector(JoinA outer, IEnumerable<JoinB> innerEnum)
    {
        return Linq.Count(innerEnum);
    }

    [Test, Parallelizable]
    public void BlinqShouldEqualLinqNativeArrayJoinNativeArray(
        [ValueSource(typeof(GroupJoinOuterValues), nameof(GroupJoinOuterValues.Values))] JoinA[] outerArr,
        [ValueSource(typeof(GroupJoinInnerValues), nameof(GroupJoinInnerValues.Values))] JoinB[] innerArr
        )
    {
        var outer = new NativeArray<JoinA>(outerArr, Allocator.Persistent);
        var inner = new NativeArray<JoinB>(innerArr, Allocator.Persistent);
        var expected = ExceptionAndValue(() => Linq.ToArray(Linq.GroupJoin(outer, inner, JoinAKeySelector.Invoke, JoinBKeySelector.Invoke, GroupJoinABLinqSelector)));
        var actual = ExceptionAndValue(() => Linq.ToArray(Blinq.GroupJoin(ref outer, inner, JoinAKeySelector, JoinBKeySelector, GroupJoinABSelector)));
        AssertAreEqual(expected, actual);
        outer.Dispose();
        inner.Dispose();
    }
}
