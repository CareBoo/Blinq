using System;
using System.Collections.Generic;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>> ThenByDescending<T, TSource, TKey, TKeySelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, IOrderedSequence<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var keyComparer = KeyComparer.New(keySelector, default(DefaultComparer<TKey>));
            var descending = Descending<T>.New(keyComparer);
            var seq = new ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>(source.Source, descending);
            return ValueSequence<T>.New(seq);
        }

        public static ValueSequence<T, ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>> ThenByDescending<T, TSource, TKey, TKeySelector, TComparer>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            TComparer comparer
            )
            where T : struct
            where TSource : struct, IOrderedSequence<T>
            where TKey : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TComparer : struct, IComparer<TKey>
        {
            var keyComparer = KeyComparer.New(keySelector, comparer);
            var descending = Descending<T>.New(keyComparer);
            var seq = new ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>(source.Source, descending);
            return ValueSequence<T>.New(seq);
        }
    }
}
