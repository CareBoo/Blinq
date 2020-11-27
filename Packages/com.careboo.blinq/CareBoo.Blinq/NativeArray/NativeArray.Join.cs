using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            TResult,
            JoinSequence<TOuter, NativeArraySequence<TOuter>, NativeArray<TOuter>.Enumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>,
            JoinSequence<TOuter, NativeArraySequence<TOuter>, NativeArray<TOuter>.Enumerator, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>.Enumerator>
        Join<TOuter, TInner, TInnerSequence, TInnerEnumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this in NativeArray<TOuter> outer,
            in ValueSequence<TInner, TInnerSequence, TInnerEnumerator> inner,
            ValueFunc<TOuter, TKey>.Struct<TOuterKeySelector> outerKeySelector,
            ValueFunc<TInner, TKey>.Struct<TInnerKeySelector> innerKeySelector,
            ValueFunc<TOuter, TInner, TResult>.Struct<TResultSelector> resultSelector
            )
            where TOuter : struct
            where TInner : struct
            where TInnerSequence : struct, ISequence<TInner, TInnerEnumerator>
            where TInnerEnumerator : struct, IEnumerator<TInner>
            where TKey : struct, IEquatable<TKey>
            where TOuterKeySelector : struct, IFunc<TOuter, TKey>
            where TInnerKeySelector : struct, IFunc<TInner, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TOuter, TInner, TResult>
        {
            return outer.ToValueSequence().Join(in inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static ValueSequence<
            TResult,
            JoinSequence<TOuter, NativeArraySequence<TOuter>, NativeArray<TOuter>.Enumerator, TInner, NativeArraySequence<TInner>, NativeArray<TInner>.Enumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>,
            JoinSequence<TOuter, NativeArraySequence<TOuter>, NativeArray<TOuter>.Enumerator, TInner, NativeArraySequence<TInner>, NativeArray<TInner>.Enumerator, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>.Enumerator>
        Join<TOuter, TInner, TKey, TOuterKeySelector, TInnerKeySelector, TResult, TResultSelector>(
            this in NativeArray<TOuter> outer,
            in NativeArray<TInner> inner,
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
            return outer.ToValueSequence().Join(in inner, outerKeySelector, innerKeySelector, resultSelector);
        }
    }

}
