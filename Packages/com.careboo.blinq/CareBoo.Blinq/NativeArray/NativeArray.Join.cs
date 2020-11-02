using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, JoinSequence<TOuter, NativeArraySequence<TOuter>, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>> Join<TOuter, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this ref NativeArray<TOuter> outer,
            ValueSequence<TInner, TInnerSequence> inner,
            ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, TInner, TResult>.Struct<TResultSelector> resultSelector
            )
            where TOuter : struct
            where TInner : struct
            where TInnerSequence : struct, ISequence<TInner>
            where TKey : struct, IEquatable<TKey>
            where TOuterKeySelector : struct, IFunc<TOuter, TKey>
            where TInnerKeySelector : struct, IFunc<TInner, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TOuter, TInner, TResult>
        {
            var outerSeq = outer.ToValueSequence();
            return outerSeq.Join(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static ValueSequence<TResult, JoinSequence<TOuter, NativeArraySequence<TOuter>, TInner, NativeArraySequence<TInner>, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>> Join<TOuter, TInner, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this ref NativeArray<TOuter> outer,
            NativeArray<TInner> inner,
            ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, TInner, TResult>.Struct<TResultSelector> resultSelector
            )
            where TOuter : struct
            where TInner : struct
            where TKey : struct, IEquatable<TKey>
            where TOuterKeySelector : struct, IFunc<TOuter, TKey>
            where TInnerKeySelector : struct, IFunc<TInner, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TOuter, TInner, TResult>
        {
            var outerSeq = outer.ToValueSequence();
            return outerSeq.Join(inner, outerKeySelector, innerKeySelector, resultSelector);
        }
    }

}
