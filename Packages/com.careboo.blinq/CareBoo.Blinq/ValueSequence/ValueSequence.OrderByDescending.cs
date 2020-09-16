using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, OrderByDescendingSequence<T, TSource, TKey, TKeySelector>> OrderByDescending<T, TSource, TKey, TKeySelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var seq = new OrderByDescendingSequence<T, TSource, TKey, TKeySelector> { Source = source.Source, KeySelector = keySelector };
            return new ValueSequence<T, OrderByDescendingSequence<T, TSource, TKey, TKeySelector>>(seq);
        }

        public static ValueSequence<T, OrderByComparerDescendingSequence<T, TSource, TKey, TKeySelector, TComparer>> OrderByDescending<T, TSource, TKey, TKeySelector, TComparer>(
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
            var seq = new OrderByComparerDescendingSequence<T, TSource, TKey, TKeySelector, TComparer> { Source = source.Source, KeySelector = keySelector, Comparer = comparer };
            return new ValueSequence<T, OrderByComparerDescendingSequence<T, TSource, TKey, TKeySelector, TComparer>>(seq);
        }
    }

    public struct OrderByDescendingSequence<T, TSource, TKey, TKeySelector>
        : IOrderedSequence<T, Descending<T, KeyComparer<T, TKey, TKeySelector>>>
        where T : struct
        where TSource : struct, ISequence<T>
        where TKey : struct, IComparable<TKey>
        where TKeySelector : struct, IFunc<T, TKey>
    {
        public TSource Source;
        public ValueFunc<T, TKey>.Impl<TKeySelector> KeySelector;

        public NativeList<T> Execute()
        {
            var list = Source.Execute();
            var comparer = GetComparer();
            list.Sort(comparer);
            return list;
        }

        public Descending<T, KeyComparer<T, TKey, TKeySelector>> GetComparer()
        {
            var comparer = new KeyComparer<T, TKey, TKeySelector> { KeySelector = KeySelector };
            return new Descending<T, KeyComparer<T, TKey, TKeySelector>> { Comparer = comparer };
        }
    }

    public struct OrderByComparerDescendingSequence<T, TSource, TKey, TKeySelector, TComparer>
        : IOrderedSequence<T, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>
        where T : struct
        where TSource : struct, ISequence<T>
        where TKey : struct
        where TKeySelector : struct, IFunc<T, TKey>
        where TComparer : struct, IComparer<TKey>
    {
        public TSource Source;
        public ValueFunc<T, TKey>.Impl<TKeySelector> KeySelector;
        public TComparer Comparer;

        public NativeList<T> Execute()
        {
            var list = Source.Execute();
            var comparer = GetComparer();
            list.Sort(comparer);
            return list;
        }

        public Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>> GetComparer()
        {
            var comparer = new KeyComparer<T, TKey, TKeySelector, TComparer> { KeySelector = KeySelector, Comparer = Comparer };
            return new Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>> { Comparer = comparer };
        }
    }

    public struct Descending<T, TComparer> : IComparer<T>
        where T : struct
        where TComparer : struct, IComparer<T>
    {
        public TComparer Comparer;

        public int Compare(T x, T y)
        {
            return Comparer.Compare(y, x);
        }
    }
}
