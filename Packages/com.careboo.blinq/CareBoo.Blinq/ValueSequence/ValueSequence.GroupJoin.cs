﻿using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, GroupJoinSequence<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>> GroupJoin<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this in ValueSequence<TOuter, TOuterSequence> outer,
            in ValueSequence<TInner, TInnerSequence> inner,
            in ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            in ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            in ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector
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
            var outerSeq = outer.GetEnumerator();
            var innerSeq = inner.GetEnumerator();
            var seq = GroupJoinSequence.New(ref outerSeq, ref innerSeq, in outerKeySelector, in innerKeySelector, in resultSelector);
            return ValueSequence<TResult>.New(ref seq);
        }

        public static ValueSequence<TResult, GroupJoinSequence<TOuter, TOuterSequence, TInner, NativeArraySequence<TInner>, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>> GroupJoin<TOuter, TOuterSequence, TInner, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this in ValueSequence<TOuter, TOuterSequence> outer,
            in NativeArray<TInner> inner,
            in ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            in ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            in ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector
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
            var outerSeq = outer.GetEnumerator();
            var innerSeq = inner.ToValueSequence().GetEnumerator();
            var seq = GroupJoinSequence.New(ref outerSeq, ref innerSeq, outerKeySelector, innerKeySelector, resultSelector);
            return ValueSequence<TResult>.New(ref seq);
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
        TOuterSequence outer;
        TInnerSequence inner;
        readonly ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector;
        readonly ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector;
        readonly ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector;

        NativeMultiHashMap<TKey, TInner> innerMap;

        public GroupJoinSequence(
            ref TOuterSequence outer,
            ref TInnerSequence inner,
            in ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            in ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            in ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector
            )
        {
            this.outer = outer;
            this.inner = inner;
            this.outerKeySelector = outerKeySelector;
            this.innerKeySelector = innerKeySelector;
            this.resultSelector = resultSelector;
            innerMap = default;
            Current = default;
        }

        public TResult Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            outer.Dispose();
            inner.Dispose();
            if (innerMap.IsCreated)
                innerMap.Dispose();
        }

        public bool MoveNext()
        {
            if (!outer.MoveNext())
                return false;

            if (!innerMap.IsCreated)
            {
                var innerList = inner.ToNativeList(Allocator.Temp);
                innerMap = new NativeMultiHashMap<TKey, TInner>(innerList.Length, Allocator.Temp);
                for (var i = 0; i < innerList.Length; i++)
                    innerMap.Add(innerKeySelector.Invoke(innerList[i]), innerList[i]);
                innerList.Dispose();
            }

            var currentOuter = outer.Current;
            var key = outerKeySelector.Invoke(currentOuter);
            var innerGroup = GetInnerGroup(key, innerMap);
            Current = resultSelector.Invoke(currentOuter, innerGroup);
            innerGroup.Dispose();
            return true;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<TResult> ToNativeList(Allocator allocator)
        {
            var outerList = outer.ToNativeList(Allocator.Temp);
            var innerList = inner.ToNativeList(Allocator.Temp);
            var groupMap = new NativeMultiHashMap<TKey, TInner>(innerList.Length, Allocator.Temp);
            var result = ToNativeList(outerList, innerList, groupMap, allocator);
            outerList.Dispose();
            innerList.Dispose();
            groupMap.Dispose();
            return result;

        }

        private NativeList<TResult> ToNativeList(
            NativeList<TOuter> outerList,
            NativeList<TInner> innerList,
            NativeMultiHashMap<TKey, TInner> groupMap,
            Allocator allocator)
        {
            var result = new NativeList<TResult>(allocator);
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
                var inners = GetInnerGroup(key, groupMap);
                result.Add(resultSelector.Invoke(item, inners));
                inners.Dispose();
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
            ref TOuterSequence outer,
            ref TInnerSequence inner,
            in ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            in ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            in ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector
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
            return new GroupJoinSequence<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(ref outer, ref inner, in outerKeySelector, in innerKeySelector, in resultSelector);
        }
    }
}
