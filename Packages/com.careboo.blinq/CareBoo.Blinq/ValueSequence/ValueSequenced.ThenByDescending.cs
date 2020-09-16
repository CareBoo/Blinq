using System;
using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>> ThenByDescending<T, TSource, TKey, TKeySelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, IOrderedSequence<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var keyComparer = new KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>(keySelector);
            var descending = new Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>(keyComparer);
            var seq = new ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>(source.Source, descending);
            return new ValueSequence<T, ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>>(seq);
        }

        public static ValueSequence<T, ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>> ThenByDescending<T, TSource, TKey, TKeySelector, TComparer>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector,
            TComparer comparer
            )
            where T : struct
            where TSource : struct, IOrderedSequence<T>
            where TKey : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TComparer : struct, IComparer<TKey>
        {
            var keyComparer = new KeyComparer<T, TKey, TKeySelector, TComparer>(keySelector, comparer);
            var descending = new Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>(keyComparer);
            var seq = new ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>(source.Source, descending);
            return new ValueSequence<T, ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>>(seq);
        }
    }
}
