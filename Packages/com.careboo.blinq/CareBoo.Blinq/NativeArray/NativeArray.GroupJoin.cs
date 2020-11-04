using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, GroupJoinSequence<TOuter, NativeArraySequence<TOuter>, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>> GroupJoin<TOuter, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this in NativeArray<TOuter> outer,
            in ValueSequence<TInner, TInnerSequence> inner,
            in ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            in ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            in ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector
            )
            where TOuter : struct
            where TInner : struct
            where TInnerSequence : struct, ISequence<TInner>
            where TKey : struct, IEquatable<TKey>
            where TOuterKeySelector : struct, IFunc<TOuter, TKey>
            where TInnerKeySelector : struct, IFunc<TInner, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TOuter, NativeArray<TInner>, TResult>
        {
            var outerSeq = outer.ToValueSequence();
            return outerSeq.GroupJoin(in inner, in outerKeySelector, in innerKeySelector, in resultSelector);
        }

        public static ValueSequence<TResult, GroupJoinSequence<TOuter, NativeArraySequence<TOuter>, TInner, NativeArraySequence<TInner>, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>> GroupJoin<TOuter, TInner, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this in NativeArray<TOuter> outer,
            in NativeArray<TInner> inner,
            in ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            in ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            in ValueFunc<TOuter, NativeArray<TInner>, TResult>.Struct<TResultSelector> resultSelector
            )
            where TOuter : struct
            where TInner : struct
            where TKey : struct, IEquatable<TKey>
            where TOuterKeySelector : struct, IFunc<TOuter, TKey>
            where TInnerKeySelector : struct, IFunc<TInner, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TOuter, NativeArray<TInner>, TResult>
        {
            var outerSeq = outer.ToValueSequence();
            return outerSeq.GroupJoin(in inner, in outerKeySelector, in innerKeySelector, in resultSelector);
        }
    }
}
