using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, JoinSequence<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>> Join<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this ValueSequence<TOuter, TOuterSequence> outer,
            ValueSequence<TInner, TInnerSequence> inner,
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
            var seq = JoinSequence.New(outer.Source, inner.Source, outerKeySelector, innerKeySelector, resultSelector);
            return ValueSequence<TResult>.New(seq);
        }

        public static ValueSequence<TResult, JoinSequence<TOuter, TOuterSequence, TInner, NativeArraySequence<TInner>, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>> Join<TOuter, TOuterSequence, TInner, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this ValueSequence<TOuter, TOuterSequence> outer,
            NativeArray<TInner> inner,
            ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, TInner, TResult>.Struct<TResultSelector> resultSelector
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
            var seq = JoinSequence.New(outer.Source, inner.ToValueSequence().Source, outerKeySelector, innerKeySelector, resultSelector);
            return ValueSequence<TResult>.New(seq);
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
        public TOuterSequence Outer;
        public TInnerSequence Inner;
        public ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> OuterKeySelector;
        public ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> InnerKeySelector;
        public ValueFunc<TOuter, TInner, TResult>.Struct<TResultSelector> ResultSelector;

        public NativeList<TResult> Execute()
        {
            using (var outer = Outer.Execute())
            using (var inner = Inner.Execute())
            using (var outerHashMap = new NativeHashMap<TKey, TOuter>(outer.Length, Allocator.Temp))
            {
                var resultList = new NativeList<TResult>(inner.Length, Allocator.Temp);
                for (var i = 0; i < outer.Length; i++)
                {
                    var val = outer[i];
                    var key = OuterKeySelector.Invoke(val);
                    outerHashMap.Add(key, val);
                }
                for (var i = 0; i < inner.Length; i++)
                {
                    var innerVal = inner[i];
                    var key = InnerKeySelector.Invoke(innerVal);
                    if (outerHashMap.TryGetValue(key, out var outerVal))
                    {
                        var result = ResultSelector.Invoke(outerVal, innerVal);
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
            TOuterSequence outer,
            TInnerSequence inner,
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
            {
                Outer = outer,
                Inner = inner,
                OuterKeySelector = outerKeySelector,
                InnerKeySelector = innerKeySelector,
                ResultSelector = resultSelector
            };
        }
    }
}
