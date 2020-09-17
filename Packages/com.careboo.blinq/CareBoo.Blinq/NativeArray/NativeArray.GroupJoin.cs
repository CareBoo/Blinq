using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, GroupJoinSequence<TOuter, NativeArraySequence<TOuter>, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>> GroupJoin<TOuter, TInner, TInnerSequence, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this ref NativeArray<TOuter> outer,
            ValueSequence<TInner, TInnerSequence> inner,
            ValueFunc<TOuter, TKey>.Impl<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Impl<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, NativeArray<TInner>, TResult>.Impl<TResultSelector> resultSelector
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
            return outer.ToValueSequence().GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static ValueSequence<TResult, GroupJoinSequence<TOuter, NativeArraySequence<TOuter>, TInner, NativeArraySequence<TInner>, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>> GroupJoin<TOuter, TInner, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this ref NativeArray<TOuter> outer,
            NativeArray<TInner> inner,
            ValueFunc<TOuter, TKey>.Impl<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Impl<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, NativeArray<TInner>, TResult>.Impl<TResultSelector> resultSelector
            )
            where TOuter : struct
            where TInner : struct
            where TKey : struct, IEquatable<TKey>
            where TOuterKeySelector : struct, IFunc<TOuter, TKey>
            where TInnerKeySelector : struct, IFunc<TInner, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TOuter, NativeArray<TInner>, TResult>
        {
            return outer.ToValueSequence().GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector);
        }
    }
}
