using System;
using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, OrderBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>> OrderByDescending<T, TSource, TKey, TKeySelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var keyComparer = new KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>(keySelector);
            var descending = new Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>(keyComparer);
            var seq = new OrderBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>(source.Source, descending);
            return new ValueSequence<T, OrderBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>>(seq);
        }

        public static ValueSequence<T, OrderBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>> OrderByDescending<T, TSource, TKey, TKeySelector, TComparer>(
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
            var keyComparer = new KeyComparer<T, TKey, TKeySelector, TComparer>(keySelector, comparer);
            var descending = new Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>(keyComparer);
            var seq = new OrderBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>(source.Source, descending);
            return new ValueSequence<T, OrderBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>>(seq);
        }
    }

    public struct Descending<T, TComparer> : IComparer<T>
        where T : struct
        where TComparer : struct, IComparer<T>
    {
        readonly TComparer comparer;

        public Descending(TComparer comparer)
        {
            this.comparer = comparer;
        }

        public int Compare(T x, T y)
        {
            return comparer.Compare(y, x);
        }
    }

    public static class Descending<T>
        where T : struct
    {
        public static Descending<T, TComparer> New<TComparer>(TComparer comparer)
            where TComparer : struct, IComparer<T>
        {
            return new Descending<T, TComparer>(comparer);
        }
    }
}
