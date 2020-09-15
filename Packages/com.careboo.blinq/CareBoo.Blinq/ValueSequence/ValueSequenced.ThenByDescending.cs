using System;
using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, OrderByDescendingSequence<T, TSource, TKey, TKeySelector>> ThenByDescending<T, TSource, TKey, TKeySelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            return source.OrderByDescending(keySelector);
        }

        public static ValueSequence<T, OrderByComparerDescendingSequence<T, TSource, TKey, TKeySelector, TComparer>> ThenByDescending<T, TSource, TKey, TKeySelector, TComparer>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector,
            TComparer comparer
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TComparer : struct, IComparer<TKey>
        {
            return source.OrderByDescending(keySelector, comparer);
        }
    }
}
