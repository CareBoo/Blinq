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

    public struct OrderByDescendingSequence<T, TSource, TKey, TKeySelector> : ISequence<T>
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
            var comparer = new KeyComparer<T, TKey, TKeySelector> { KeySelector = KeySelector };
            var descending = new Descending<T, KeyComparer<T, TKey, TKeySelector>> { Comparer = comparer };
            list.Sort(descending);
            return list;
        }
    }

    public struct OrderByComparerDescendingSequence<T, TSource, TKey, TKeySelector, TComparer> : ISequence<T>
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
            var comparer = new KeyComparer<T, TKey, TKeySelector, TComparer> { KeySelector = KeySelector, Comparer = Comparer };
            var descending = new Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>> { Comparer = comparer };
            list.Sort(descending);
            return list;
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
