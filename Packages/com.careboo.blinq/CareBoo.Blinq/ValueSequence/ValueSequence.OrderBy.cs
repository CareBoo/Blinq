using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, OrderBySequence<T, TSource, TKey, TKeySelector>> OrderBy<T, TSource, TKey, TKeySelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var seq = new OrderBySequence<T, TSource, TKey, TKeySelector> { Source = source.Source, KeySelector = keySelector };
            return new ValueSequence<T, OrderBySequence<T, TSource, TKey, TKeySelector>>(seq);
        }

        public static ValueSequence<T, OrderByComparerSequence<T, TSource, TKey, TKeySelector, TComparer>> OrderBy<T, TSource, TKey, TKeySelector, TComparer>(
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
            var seq = new OrderByComparerSequence<T, TSource, TKey, TKeySelector, TComparer> { Source = source.Source, KeySelector = keySelector, Comparer = comparer };
            return new ValueSequence<T, OrderByComparerSequence<T, TSource, TKey, TKeySelector, TComparer>>(seq);
        }
    }

    public struct OrderBySequence<T, TSource, TKey, TKeySelector>
        : IOrderedSequence<T, KeyComparer<T, TKey, TKeySelector>>
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

        public KeyComparer<T, TKey, TKeySelector> GetComparer()
        {
            return new KeyComparer<T, TKey, TKeySelector> { KeySelector = KeySelector };
        }
    }

    public struct OrderByComparerSequence<T, TSource, TKey, TKeySelector, TComparer>
        : IOrderedSequence<T, KeyComparer<T, TKey, TKeySelector, TComparer>>
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

        public KeyComparer<T, TKey, TKeySelector, TComparer> GetComparer()
        {
            return new KeyComparer<T, TKey, TKeySelector, TComparer> { KeySelector = KeySelector, Comparer = Comparer };
        }
    }

    public struct KeyComparer<T, TKey, TKeySelector> : IComparer<T>
        where T : struct
        where TKey : struct, IComparable<TKey>
        where TKeySelector : struct, IFunc<T, TKey>
    {
        public ValueFunc<T, TKey>.Impl<TKeySelector> KeySelector;

        public int Compare(T x, T y)
        {
            var xKey = KeySelector.Invoke(x);
            var yKey = KeySelector.Invoke(y);
            return xKey.CompareTo(yKey);
        }
    }

    public struct KeyComparer<T, TKey, TKeySelector, TComparer> : IComparer<T>
        where T : struct
        where TKey : struct
        where TKeySelector : struct, IFunc<T, TKey>
        where TComparer : struct, IComparer<TKey>
    {
        public ValueFunc<T, TKey>.Impl<TKeySelector> KeySelector;
        public TComparer Comparer;

        public int Compare(T x, T y)
        {
            var xKey = KeySelector.Invoke(x);
            var yKey = KeySelector.Invoke(y);
            return Comparer.Compare(xKey, yKey);
        }
    }
}
