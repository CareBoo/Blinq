using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;
using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            TResult,
            GroupJoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>,
            GroupJoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>.Enumerator>
        GroupJoin<TOuter, TOuterSequence, TOuterEnumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this in ValueSequence<TOuter, TOuterSequence, TOuterEnumerator> outer,
            in ValueSequence<TInner, TInnerSequence, TInnerEnumerator> inner,
            ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector
            )
            where TOuter : struct
            where TOuterSequence : struct, ISequence<TOuter, TOuterEnumerator>
            where TOuterEnumerator : struct, IEnumerator<TOuter>
            where TInner : struct
            where TInnerSequence : struct, ISequence<TInner, TInnerEnumerator>
            where TInnerEnumerator : struct, IEnumerator<TInner>
            where TKey : struct, IEquatable<TKey>
            where TOuterKeySelector : struct, IFunc<TOuter, TKey>
            where TInnerKeySelector : struct, IFunc<TInner, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TOuter, NativeArray<TInner>, TResult>
        {
            var outerSeq = outer.Source;
            var innerSeq = inner.Source;
            var seq = new GroupJoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(in outerSeq, in innerSeq, outerKeySelector, innerKeySelector, resultSelector);
            return ValueSequence<TResult, GroupJoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>.Enumerator>.New(in seq);
        }

        public static ValueSequence<
            TResult,
            GroupJoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, NativeArraySequence<TInner>, NativeArray<TInner>.Enumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>,
            GroupJoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, NativeArraySequence<TInner>, NativeArray<TInner>.Enumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>.Enumerator>
        GroupJoin<TOuter, TOuterSequence, TOuterEnumerator, TInner, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this in ValueSequence<TOuter, TOuterSequence, TOuterEnumerator> outer,
            in NativeArray<TInner> inner,
            ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector
            )
            where TOuter : struct
            where TOuterSequence : struct, ISequence<TOuter, TOuterEnumerator>
            where TOuterEnumerator : struct, IEnumerator<TOuter>
            where TInner : struct
            where TKey : struct, IEquatable<TKey>
            where TOuterKeySelector : struct, IFunc<TOuter, TKey>
            where TInnerKeySelector : struct, IFunc<TInner, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TOuter, NativeArray<TInner>, TResult>
        {
            var innerSeq = inner.ToValueSequence();
            return outer.GroupJoin(in innerSeq, outerKeySelector, innerKeySelector, resultSelector);
        }
    }

    public struct GroupJoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>
        : ISequence<TResult, GroupJoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>.Enumerator>
        where TOuter : struct
        where TOuterSequence : struct, ISequence<TOuter, TOuterEnumerator>
        where TOuterEnumerator : struct, IEnumerator<TOuter>
        where TInner : struct
        where TInnerSequence : struct, ISequence<TInner, TInnerEnumerator>
        where TInnerEnumerator : struct, IEnumerator<TInner>
        where TKey : struct, IEquatable<TKey>
        where TOuterKeySelector : struct, IFunc<TOuter, TKey>
        where TInnerKeySelector : struct, IFunc<TInner, TKey>
        where TResult : struct
        where TResultSelector : struct, IFunc<TOuter, NativeArray<TInner>, TResult>
    {
        public struct Enumerator : IEnumerator<TResult>
        {
            TOuterEnumerator outerEnumerator;
            NativeMultiHashMap<TKey, TInner> innerMap;
            readonly ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector;
            readonly ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector;
            readonly ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector;

            public Enumerator(
                in TOuterSequence outer,
                in TInnerSequence inner,
                ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
                ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
                ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector
                )
            {
                outerEnumerator = outer.GetEnumerator();
                this.outerKeySelector = outerKeySelector;
                this.innerKeySelector = innerKeySelector;
                this.resultSelector = resultSelector;
                var innerList = inner.ToNativeList(Allocator.Temp);
                innerMap = new NativeMultiHashMap<TKey, TInner>(innerList.Length, Allocator.Temp);
                for (var i = 0; i < innerList.Length; i++)
                    innerMap.Add(innerKeySelector.Invoke(innerList[i]), innerList[i]);
                innerList.Dispose();
                Current = default;
            }

            public TResult Current { get; private set; }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (!outerEnumerator.MoveNext())
                    return false;

                var currentOuter = outerEnumerator.Current;
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

            public void Dispose()
            {
                outerEnumerator.Dispose();
                innerMap.Dispose();
            }
        }

        readonly TOuterSequence outer;
        readonly TInnerSequence inner;
        readonly ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector;
        readonly ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector;
        readonly ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector;

        public GroupJoinSequence(
            in TOuterSequence outer,
            in TInnerSequence inner,
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

        private static NativeList<TInner> GetInnerGroup(TKey key, NativeMultiHashMap<TKey, TInner> groupMap)
        {
            var result = new NativeList<TInner>(Allocator.Temp);
            var innersEnum = groupMap.GetValuesForKey(key);
            while (innersEnum.MoveNext())
                result.Add(innersEnum.Current);
            return result;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(in outer, in inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
