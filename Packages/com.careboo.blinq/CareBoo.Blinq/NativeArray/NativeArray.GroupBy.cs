using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, GroupBySequence<T, NativeArraySequence<T>, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>> GroupBy<T, TKey, TKeySelector, TElement, TElementSelector, TResult, TResultSelector>(
            this ref NativeArray<T> source,
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector,
            ValueFunc<T, TElement>.Impl<TElementSelector> elementSelector,
            ValueFunc<TKey, NativeMultiHashMap<TKey, TElement>, TResult>.Impl<TResultSelector> resultSelector
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

        // public static ValueSequence<ValueGrouping<TKey, TElement>, GroupBySequence<T, NativeArraySequence<T>, TKey, TKeySelector, TElement, TElementSelector, ValueGrouping<TKey, TElement>, GroupingSelector<TKey, TElement>>> GroupBy<T, TKey, TKeySelector, TElement, TElementSelector>(
        //     this ref NativeArray<T> source,
        //     ValueFunc<T, TKey>.Impl<TKeySelector> keySelector,
        //     ValueFunc<T, TElement>.Impl<TElementSelector> elementSelector
        //     )
        //     where T : struct
        //     where TKey : struct, IEquatable<TKey>
        //     where TKeySelector : struct, IFunc<T, TKey>
        //     where TElement : struct
        //     where TElementSelector : struct, IFunc<T, TElement>
        // {
        //     return source.ToValueSequence().GroupBy(keySelector, elementSelector);
        // }

        public static ValueSequence<TResult, GroupBySequence<T, NativeArraySequence<T>, TKey, TKeySelector, T, SameSelector<T>, TResult, TResultSelector>> GroupBy<T, TKey, TKeySelector, TResult, TResultSelector>(
            this ref NativeArray<T> source,
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector,
            ValueFunc<TKey, NativeMultiHashMap<TKey, T>, TResult>.Impl<TResultSelector> resultSelector
            )
            where T : struct
            where TKey : unmanaged, IEquatable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
            where TResult : struct
            where TResultSelector : struct, IFunc<TKey, NativeMultiHashMap<TKey, T>, TResult>
        {
            return source.ToValueSequence().GroupBy(keySelector, resultSelector);
        }

        // public static ValueSequence<ValueGrouping<TKey, T>, GroupBySequence<T, NativeArraySequence<T>, TKey, TKeySelector, T, SameSelector<T>, ValueGrouping<TKey, T>, GroupingSelector<TKey, T>>> GroupBy<T, TKey, TKeySelector>(
        //     this ref NativeArray<T> source,
        //     ValueFunc<T, TKey>.Impl<TKeySelector> keySelector
        //     )
        //     where T : struct
        //     where TKey : struct, IEquatable<TKey>
        //     where TKeySelector : struct, IFunc<T, TKey>
        // {
        //     return source.ToValueSequence().GroupBy(keySelector);
        // }
    }
}
