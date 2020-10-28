using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, GroupJoinSequence<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>> GroupJoin<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this ValueSequence<TOuter, TOuterSequence> outer,
            ValueSequence<TInner, TInnerSequence> inner,
            ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector
            )
            where TOuter : struct
            where TOuterSequence : struct, ISequence<TOuter>
            where TInner : struct
            where TInnerSequence : struct, ISequence<TInner>
            where TKey : struct, IEquatable<TKey>
            where TOuterKeySelector : struct, IFunc<TOuter, TKey>
            where TInnerKeySelector : struct, IFunc<TInner, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TOuter, NativeArray<TInner>, TResult>
        {
            var seq = GroupJoinSequence.New(outer.Source, inner.Source, outerKeySelector, innerKeySelector, resultSelector);
            return ValueSequence<TResult>.New(seq);
        }

        public static ValueSequence<TResult, GroupJoinSequence<TOuter, TOuterSequence, TInner, NativeArraySequence<TInner>, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>> GroupJoin<TOuter, TOuterSequence, TInner, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this ValueSequence<TOuter, TOuterSequence> outer,
            NativeArray<TInner> inner,
            ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector
            )
            where TOuter : struct
            where TOuterSequence : struct, ISequence<TOuter>
            where TInner : struct
            where TKey : struct, IEquatable<TKey>
            where TOuterKeySelector : struct, IFunc<TOuter, TKey>
            where TInnerKeySelector : struct, IFunc<TInner, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TOuter, NativeArray<TInner>, TResult>
        {
            var seq = GroupJoinSequence.New(outer.Source, inner.ToValueSequence().Source, outerKeySelector, innerKeySelector, resultSelector);
            return ValueSequence<TResult>.New(seq);
        }
    }

    public struct GroupJoinSequence<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>
        : ISequence<TResult>
        where TOuter : struct
        where TOuterSequence : struct, ISequence<TOuter>
        where TInner : struct
        where TInnerSequence : struct, ISequence<TInner>
        where TKey : struct, IEquatable<TKey>
        where TOuterKeySelector : struct, IFunc<TOuter, TKey>
        where TInnerKeySelector : struct, IFunc<TInner, TKey>
        where TResult : struct
        where TResultSelector : struct, IFunc<TOuter, NativeArray<TInner>, TResult>
    {
        readonly TOuterSequence outer;
        readonly TInnerSequence inner;
        readonly ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector;
        readonly ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector;
        readonly ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector;

        NativeList<TInner> currentInners;
        NativeList<TInner> remainingInners;

        public GroupJoinSequence(
            TOuterSequence outer,
            TInnerSequence inner,
            ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector
            )
        {
            this.outer = outer;
            this.inner = inner;
            this.outerKeySelector = outerKeySelector;
            this.innerKeySelector = innerKeySelector;
            this.resultSelector = resultSelector;
            currentInners = default;
            remainingInners = default;
        }

        public TResult Current => resultSelector.Invoke(outer.Current, currentInners);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            if (remainingInners.IsCreated)
                remainingInners.Dispose();
            outer.Dispose();
            inner.Dispose();
        }

        public bool MoveNext()
        {
            if (!outer.MoveNext())
                return false;

            if (!remainingInners.IsCreated)
                using (var innerList = inner.ToList())
                {
                    remainingInners = new NativeList<TInner>(innerList.Length, Allocator.Persistent);
                    remainingInners.AddRangeNoResize(innerList);
                }

            var currentOuter = outer.Current;
            var outerKey = outerKeySelector.Invoke(currentOuter);
            if (currentInners.IsCreated)
                currentInners.Dispose();
            currentInners = new NativeList<TInner>(remainingInners.Length, Allocator.Persistent);
            for (var i = 0; i < remainingInners.Length; i++)
            {
                var currentInner = remainingInners[i];
                var innerKey = innerKeySelector.Invoke(currentInner);
                if (outerKey.Equals(innerKey))
                {
                    currentInners.Add(currentInner);
                    remainingInners.RemoveAt(i);
                    i--;
                }
            }
            return true;
        }

        public void Reset()
        {
            if (remainingInners.IsCreated)
                remainingInners.Dispose();
            remainingInners = default;
            outer.Reset();
            inner.Reset();
        }

        public NativeList<TResult> ToList()
        {
            using (var outerList = outer.ToList())
            using (var innerList = inner.ToList())
            using (var groupMap = new NativeMultiHashMap<TKey, TInner>(innerList.Length, Allocator.Temp))
            {
                return Execute(outerList, innerList, groupMap);
            }
        }

        private NativeList<TResult> Execute(
            NativeList<TOuter> outerList,
            NativeList<TInner> innerList,
            NativeMultiHashMap<TKey, TInner> groupMap)
        {
            var result = new NativeList<TResult>(Allocator.Temp);
            for (var i = 0; i < innerList.Length; i++)
            {
                var item = innerList[i];
                var key = innerKeySelector.Invoke(item);
                groupMap.Add(key, item);
            }
            for (var i = 0; i < outerList.Length; i++)
            {
                var item = outerList[i];
                var key = outerKeySelector.Invoke(item);
                using (var inners = GetInnerGroup(key, groupMap))
                    result.Add(resultSelector.Invoke(item, inners));
            }
            return result;
        }

        private NativeList<TInner> GetInnerGroup(TKey key, NativeMultiHashMap<TKey, TInner> groupMap)
        {
            var result = new NativeList<TInner>(Allocator.Temp);
            var innersEnum = groupMap.GetValuesForKey(key);
            while (innersEnum.MoveNext())
                result.Add(innersEnum.Current);
            return result;
        }
    }

    public static class GroupJoinSequence
    {
        public static GroupJoinSequence<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector> New<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            TOuterSequence outer,
            TInnerSequence inner,
            ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector
            )
            where TOuter : struct
            where TOuterSequence : struct, ISequence<TOuter>
            where TInner : struct
            where TInnerSequence : struct, ISequence<TInner>
            where TKey : struct, IEquatable<TKey>
            where TOuterKeySelector : struct, IFunc<TOuter, TKey>
            where TInnerKeySelector : struct, IFunc<TInner, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TOuter, NativeArray<TInner>, TResult>
        {
            return new GroupJoinSequence<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        }
    }
}
