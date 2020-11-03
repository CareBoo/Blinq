using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, JoinSequence<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>> Join<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this in ValueSequence<TOuter, TOuterSequence> outer,
            in ValueSequence<TInner, TInnerSequence> inner,
            in ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            in ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            in ValueFunc<TOuter, TInner, TResult>.Struct<TResultSelector> resultSelector
            )
            where TOuter : struct
            where TOuterSequence : struct, ISequence<TOuter>
            where TInner : struct
            where TInnerSequence : struct, ISequence<TInner>
            where TKey : struct, IEquatable<TKey>
            where TOuterKeySelector : struct, IFunc<TOuter, TKey>
            where TInnerKeySelector : struct, IFunc<TInner, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TOuter, TInner, TResult>
        {
            var outerSeq = outer.GetEnumerator();
            var innerSeq = inner.GetEnumerator();
            var seq = JoinSequence.New(ref outerSeq, ref innerSeq, outerKeySelector, innerKeySelector, resultSelector);
            return ValueSequence<TResult>.New(ref seq);
        }

        public static ValueSequence<TResult, JoinSequence<TOuter, TOuterSequence, TInner, NativeArraySequence<TInner>, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>> Join<TOuter, TOuterSequence, TInner, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this in ValueSequence<TOuter, TOuterSequence> outer,
            in NativeArray<TInner> inner,
            in ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            in ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            in ValueFunc<TOuter, TInner, TResult>.Struct<TResultSelector> resultSelector
            )
            where TOuter : struct
            where TOuterSequence : struct, ISequence<TOuter>
            where TInner : struct
            where TKey : struct, IEquatable<TKey>
            where TOuterKeySelector : struct, IFunc<TOuter, TKey>
            where TInnerKeySelector : struct, IFunc<TInner, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TOuter, TInner, TResult>
        {
            var outerSeq = outer.GetEnumerator();
            var innerSeq = inner.ToValueSequence().GetEnumerator();
            var seq = JoinSequence.New(ref outerSeq, ref innerSeq, outerKeySelector, innerKeySelector, resultSelector);
            return ValueSequence<TResult>.New(ref seq);
        }
    }

    public struct JoinSequence<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>
        : ISequence<TResult>
        where TOuter : struct
        where TOuterSequence : struct, ISequence<TOuter>
        where TInner : struct
        where TInnerSequence : struct, ISequence<TInner>
        where TKey : struct, IEquatable<TKey>
        where TOuterKeySelector : struct, IFunc<TOuter, TKey>
        where TInnerKeySelector : struct, IFunc<TInner, TKey>
        where TResult : struct
        where TResultSelector : struct, IFunc<TOuter, TInner, TResult>
    {
        readonly ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector;
        readonly ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector;
        readonly ValueFunc<TOuter, TInner, TResult>.Struct<TResultSelector> resultSelector;

        TOuterSequence outer;
        TInnerSequence inner;
        NativeHashMap<TKey, TOuter> map;
        NativeHashMap<TKey, TOuter>.Enumerator mapEnumerator;

        public JoinSequence(
            ref TOuterSequence outer,
            ref TInnerSequence inner,
            ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, TInner, TResult>.Struct<TResultSelector> resultSelector
            )
        {
            this.outer = outer;
            this.inner = inner;
            this.outerKeySelector = outerKeySelector;
            this.innerKeySelector = innerKeySelector;
            this.resultSelector = resultSelector;
            map = default;
            mapEnumerator = default;
            Current = default;
        }

        public TResult Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            outer.Dispose();
            inner.Dispose();
            map.Dispose();
        }

        public bool MoveNext()
        {
            if (!map.IsCreated)
                using (var outerList = outer.ToList())
                {
                    map = new NativeHashMap<TKey, TOuter>(outerList.Length, Allocator.Persistent);
                    for (var i = 0; i < outerList.Length; i++)
                    {
                        var val = outerList[i];
                        var key = outerKeySelector.Invoke(val);
                        map.Add(key, val);
                    }
                    mapEnumerator = map.GetEnumerator();
                }

            while (mapEnumerator.MoveNext())
                while (inner.MoveNext())
                {
                    var innerVal = inner.Current;
                    var key = innerKeySelector.Invoke(innerVal);
                    if (map.TryGetValue(key, out var outerVal))
                    {
                        Current = resultSelector.Invoke(outerVal, innerVal);
                        return true;
                    }
                }
            return false;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<TResult> ToList()
        {
            using (var outerList = outer.ToList())
            using (var innerList = inner.ToList())
            using (var outerHashMap = new NativeHashMap<TKey, TOuter>(outerList.Length, Allocator.Temp))
            {
                var resultList = new NativeList<TResult>(innerList.Length, Allocator.Temp);
                for (var i = 0; i < outerList.Length; i++)
                {
                    var val = outerList[i];
                    var key = outerKeySelector.Invoke(val);
                    outerHashMap.Add(key, val);
                }
                for (var i = 0; i < innerList.Length; i++)
                {
                    var innerVal = innerList[i];
                    var key = innerKeySelector.Invoke(innerVal);
                    if (outerHashMap.TryGetValue(key, out var outerVal))
                    {
                        var result = resultSelector.Invoke(outerVal, innerVal);
                        resultList.AddNoResize(result);
                    }
                }
                return resultList;
            }
        }
    }

    public static class JoinSequence
    {
        public static JoinSequence<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector> New<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            ref TOuterSequence outer,
            ref TInnerSequence inner,
            ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, TInner, TResult>.Struct<TResultSelector> resultSelector
            )
            where TOuter : struct
            where TOuterSequence : struct, ISequence<TOuter>
            where TInner : struct
            where TInnerSequence : struct, ISequence<TInner>
            where TKey : struct, IEquatable<TKey>
            where TOuterKeySelector : struct, IFunc<TOuter, TKey>
            where TInnerKeySelector : struct, IFunc<TInner, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TOuter, TInner, TResult>
        {
            return new JoinSequence<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>
            (
                ref outer,
                ref inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector
            );
        }
    }
}
