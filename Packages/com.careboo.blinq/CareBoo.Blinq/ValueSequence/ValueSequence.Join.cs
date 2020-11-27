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
            JoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>,
            JoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>.Enumerator>
        Join<TOuter, TOuterSequence, TOuterEnumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this in ValueSequence<TOuter, TOuterSequence, TOuterEnumerator> outer,
            in ValueSequence<TInner, TInnerSequence, TInnerEnumerator> inner,
            ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, TInner, TResult>.Struct<TResultSelector> resultSelector
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
            where TResultSelector : struct, IFunc<TOuter, TInner, TResult>
        {
            var outerSeq = outer.Source;
            var innerSeq = inner.Source;
            var seq = JoinSequence<TOuter, TOuterEnumerator, TInner, TInnerEnumerator>.New(in outerSeq, in innerSeq, outerKeySelector, innerKeySelector, resultSelector);
            return ValueSequence<TResult, JoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>.Enumerator>.New(in seq);
        }

        public static ValueSequence<
            TResult,
            JoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, NativeArraySequence<TInner>, NativeArray<TInner>.Enumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>,
            JoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, NativeArraySequence<TInner>, NativeArray<TInner>.Enumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>.Enumerator>
        Join<TOuter, TOuterSequence, TOuterEnumerator, TInner, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this in ValueSequence<TOuter, TOuterSequence, TOuterEnumerator> outer,
            in NativeArray<TInner> inner,
            ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, TInner, TResult>.Struct<TResultSelector> resultSelector
            )
            where TOuter : struct
            where TOuterSequence : struct, ISequence<TOuter, TOuterEnumerator>
            where TOuterEnumerator : struct, IEnumerator<TOuter>
            where TInner : struct
            where TKey : struct, IEquatable<TKey>
            where TOuterKeySelector : struct, IFunc<TOuter, TKey>
            where TInnerKeySelector : struct, IFunc<TInner, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TOuter, TInner, TResult>
        {
            var outerSeq = outer.Source;
            var innerSeq = inner.ToValueSequence().Source;
            var seq = JoinSequence<TOuter, TOuterEnumerator, TInner, NativeArray<TInner>.Enumerator>.New(in outerSeq, in innerSeq, outerKeySelector, innerKeySelector, resultSelector);
            return ValueSequence<TResult, JoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, NativeArraySequence<TInner>, NativeArray<TInner>.Enumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>.Enumerator>.New(in seq);
        }
    }


    public struct JoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>
        : ISequence<TResult, JoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>.Enumerator>
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
        where TResultSelector : struct, IFunc<TOuter, TInner, TResult>
    {
        public struct Enumerator : IEnumerator<TResult>
        {
            readonly ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector;
            readonly ValueFunc<TOuter, TInner, TResult>.Struct<TResultSelector> resultSelector;
            TInnerEnumerator innerEnumerator;
            NativeHashMap<TKey, TOuter> outerMap;
            NativeHashMap<TKey, TOuter>.Enumerator outerMapEnumerator;

            public Enumerator(
                in TOuterSequence outer,
                in TInnerSequence inner,
                ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
                ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
                ValueFunc<TOuter, TInner, TResult>.Struct<TResultSelector> resultSelector
                )
            {
                this.innerKeySelector = innerKeySelector;
                this.resultSelector = resultSelector;
                innerEnumerator = inner.GetEnumerator();
                var outerList = outer.ToNativeList(Allocator.Temp);
                outerMap = new NativeHashMap<TKey, TOuter>(outerList.Length, Allocator.Temp);
                for (var i = 0; i < outerList.Length; i++)
                {
                    var val = outerList[i];
                    var key = outerKeySelector.Invoke(val);
                    outerMap.Add(key, val);
                }
                outerMapEnumerator = outerMap.GetEnumerator();
                outerList.Dispose();
                Current = default;
            }

            public TResult Current { get; private set; }
            object IEnumerator.Current => Current;

            public void Dispose()
            {
                outerMap.Dispose();
                outerMapEnumerator.Dispose();
                innerEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                while (outerMapEnumerator.MoveNext())
                    while (innerEnumerator.MoveNext())
                    {
                        var innerVal = innerEnumerator.Current;
                        var key = innerKeySelector.Invoke(innerVal);
                        if (outerMap.TryGetValue(key, out var outerVal))
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
        }

        readonly TOuterSequence outer;
        readonly TInnerSequence inner;
        readonly ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector;
        readonly ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector;
        readonly ValueFunc<TOuter, TInner, TResult>.Struct<TResultSelector> resultSelector;

        public JoinSequence(
            in TOuterSequence outer,
            in TInnerSequence inner,
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
        }

        public NativeList<TResult> ToNativeList(Allocator allocator)
        {
            var outerList = outer.ToNativeList(Allocator.Temp);
            var innerList = inner.ToNativeList(Allocator.Temp);
            var outerHashMap = new NativeHashMap<TKey, TOuter>(outerList.Length, Allocator.Temp);
            var resultList = new NativeList<TResult>(innerList.Length, allocator);
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
            outerList.Dispose();
            innerList.Dispose();
            outerHashMap.Dispose();
            return resultList;
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

    public static class JoinSequence<TOuter, TOuterEnumerator, TInner, TInnerEnumerator>
        where TOuter : struct
        where TOuterEnumerator : struct, IEnumerator<TOuter>
        where TInner : struct
        where TInnerEnumerator : struct, IEnumerator<TInner>
    {
        public static JoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector> New<TOuterSequence, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            in TOuterSequence outer,
            in TInnerSequence inner,
            ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, TInner, TResult>.Struct<TResultSelector> resultSelector
            )
            where TOuterSequence : struct, ISequence<TOuter, TOuterEnumerator>
            where TInnerSequence : struct, ISequence<TInner, TInnerEnumerator>
            where TKey : struct, IEquatable<TKey>
            where TOuterKeySelector : struct, IFunc<TOuter, TKey>
            where TInnerKeySelector : struct, IFunc<TInner, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TOuter, TInner, TResult>
        {
            return new JoinSequence<TOuter, TOuterSequence, TOuterEnumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
                in outer,
                in inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector
                );
        }
    }
}
