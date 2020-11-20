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
            in ValueFunc<TKey, ValueGroupingValues<TKey, TElement>, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TKey : unmanaged, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
            where TElement : struct
            where TElementSelector : struct, IFunc<T, TElement>
            where TResult : struct
            where TResultSelector : struct, IFunc<TKey, ValueGroupingValues<TKey, TElement>, TResult>
        {
            return source.ToValueSequence().GroupBy(keySelector, elementSelector, resultSelector);
        }

        public static ValueSequence<TResult, GroupBySequence<T, NativeArraySequence<T>, TKey, TKeySelector, T, SameSelector<T>, TResult, TResultSelector>> GroupBy<T, TKey, TKeySelector, TResult, TResultSelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in ValueFunc<TKey, ValueGroupingValues<TKey, T>, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TKey : unmanaged, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TKey, ValueGroupingValues<TKey, T>, TResult>
        {
            return source.ToValueSequence().GroupBy(keySelector, resultSelector);
        }

        public static ValueSequence<ValueGrouping<TKey, TElement>, GroupBySequence<T, NativeArraySequence<T>, TKey, TKeySelector, TElement, TElementSelector, ValueGrouping<TKey, TElement>, GroupSelector<TKey, TElement>>> GroupBy<T, TKey, TKeySelector, TElement, TElementSelector>(
            this in NativeArray<T> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Struct<TElementSelector> elementSelector
            )
            where T : struct
            where TKey : unmanaged, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
            where TElement : struct
            where TElementSelector : struct, IFunc<T, TElement>
        {
            return source.ToValueSequence().GroupBy(keySelector, elementSelector);
        }

        public static ValueSequence<ValueGrouping<TKey, T>, GroupBySequence<T, NativeArraySequence<T>, TKey, TKeySelector, T, SameSelector<T>, ValueGrouping<TKey, T>, GroupSelector<TKey, T>>> GroupBy<T, TKey, TKeySelector>(
            this NativeArray<T> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector
            )
            where T : struct
            where TKey : unmanaged, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            return source.ToValueSequence().GroupBy(keySelector);
        }
    }
}
