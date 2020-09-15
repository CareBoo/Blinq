using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, JoinSequence<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>> Join<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this ValueSequence<TOuter, TOuterSequence> outer,
            ValueSequence<TInner, TInnerSequence> inner,
            ValueFunc<TOuter, TKey>.Impl<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Impl<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, TInner, TResult>.Impl<TResultSelector> resultSelector
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
            var seq = new JoinSequence<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>
            {
                Outer = outer.Source,
                Inner = inner.Source,
                OuterKeySelector = outerKeySelector,
                InnerKeySelector = innerKeySelector,
                ResultSelector = resultSelector
            };
            return new ValueSequence<TResult, JoinSequence<TOuter, TOuterSequence, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>>(seq);
        }

        public static ValueSequence<TResult, JoinSequence<TOuter, TOuterSequence, TInner, NativeArraySequence<TInner>, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>> Join<TOuter, TOuterSequence, TInner, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this ValueSequence<TOuter, TOuterSequence> outer,
            NativeArray<TInner> inner,
            ValueFunc<TOuter, TKey>.Impl<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Impl<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, TInner, TResult>.Impl<TResultSelector> resultSelector
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
            var seq = new JoinSequence<TOuter, TOuterSequence, TInner, NativeArraySequence<TInner>, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>
            {
                Outer = outer.Source,
                Inner = inner.ToValueSequence().Source,
                OuterKeySelector = outerKeySelector,
                InnerKeySelector = innerKeySelector,
                ResultSelector = resultSelector
            };
            return new ValueSequence<TResult, JoinSequence<TOuter, TOuterSequence, TInner, NativeArraySequence<TInner>, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>>(seq);
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
        public ValueFunc<TOuter, TKey>.Impl<TOuterKeySelector> OuterKeySelector;
        public ValueFunc<TInner, TKey>.Impl<TInnerKeySelector> InnerKeySelector;
        public ValueFunc<TOuter, TInner, TResult>.Impl<TResultSelector> ResultSelector;

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
}
