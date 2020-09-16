using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ThenBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>> ThenBy<T, TSource, TKey, TKeySelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, IOrderedSequence<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var keyComparer = new KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>(keySelector);
            var seq = new ThenBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>(source.Source, keyComparer);
            return new ValueSequence<T, ThenBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>(seq);
        }

        public static ValueSequence<T, ThenBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, TComparer>>> ThenBy<T, TSource, TKey, TKeySelector, TComparer>(
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
            var seq = new ThenBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, TComparer>>(source.Source, keyComparer);
            return new ValueSequence<T, ThenBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, TComparer>>>(seq);
        }
    }

    public struct ThenBySequence<T, TSource, TComparer>
        : IOrderedSequence<T>
        where T : struct
        where TSource : struct, IOrderedSequence<T>
        where TComparer : struct, IComparer<T>
    {
        readonly TSource source;
        readonly ThenByComparer<T, TSource, TComparer> comparer;

        public ThenBySequence(TSource source, TComparer comparer)
        {
            this.source = source;
            this.comparer = new ThenByComparer<T, TSource, TComparer>(source, comparer);
        }

        public int Compare(T x, T y)
        {
            return comparer.Compare(x, y);
        }

        public NativeList<T> Execute()
        {
            var unordered = ExecuteUnordered();
            unordered.Sort(this);
            return unordered;
        }

        public NativeList<T> ExecuteUnordered()
        {
            return source.ExecuteUnordered();
        }
    }

    public struct ThenByComparer<T, TFirstComparer, TSecondComparer> : IComparer<T>
        where T : struct
        where TFirstComparer : struct, IComparer<T>
        where TSecondComparer : struct, IComparer<T>
    {
        readonly TFirstComparer firstComparer;
        readonly TSecondComparer secondComparer;

        public ThenByComparer(TFirstComparer firstComparer, TSecondComparer secondComparer)
        {
            this.firstComparer = firstComparer;
            this.secondComparer = secondComparer;
        }

        public int Compare(T x, T y)
        {
            var firstComparison = firstComparer.Compare(x, y);
            if (firstComparison != 0) return firstComparison;

            return secondComparer.Compare(x, y);
        }
    }
}
