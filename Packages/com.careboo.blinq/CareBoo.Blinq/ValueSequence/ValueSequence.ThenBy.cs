using System;
using System.Collections.Generic;

namespace CareBoo.Blinq
{
    // TODO: Need a custom thenby sequence that can leverage a previous orderby being called
    public static partial class Sequence
    {
        public static ValueSequence<T, OrderBySequence<T, TSource, TKey, TKeySelector>> ThenBy<T, TSource, TKey, TKeySelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            return source.OrderBy(keySelector);
        }

        public static ValueSequence<T, OrderByComparerSequence<T, TSource, TKey, TKeySelector, TComparer>> ThenBy<T, TSource, TKey, TKeySelector, TComparer>(
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
            return source.OrderBy(keySelector, comparer);
        }
    }
}
