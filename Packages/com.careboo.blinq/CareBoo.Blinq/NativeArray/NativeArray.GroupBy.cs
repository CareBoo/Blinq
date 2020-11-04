using System;
using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, GroupBySequence<T, NativeArraySequence<T>, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>> GroupBy<T, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector,
            in ValueFunc<TKey, NativeMultiHashMap<TKey, TElement>, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TKey : unmanaged, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
            where TElement : struct
            where TElementSelector : struct, IFunc<T, TElement>
            where TResult : struct
            where TResultSelector : struct, IFunc<TKey, NativeMultiHashMap<TKey, TElement>, TResult>
        {
            return source.ToValueSequence().GroupBy(keySelector, elementSelector, resultSelector);
        }

        public static ValueSequence<TResult, GroupBySequence<T, NativeArraySequence<T>, TKey, TKeySelector, T, SameSelector<T>, TResult, TResultSelector>> GroupBy<T, TKey, TKeySelector, TResult, TResultSelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in ValueFunc<TKey, NativeMultiHashMap<TKey, T>, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TKey : unmanaged, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TKey, NativeMultiHashMap<TKey, T>, TResult>
        {
            return source.ToValueSequence().GroupBy(keySelector, resultSelector);
        }
    }
}
