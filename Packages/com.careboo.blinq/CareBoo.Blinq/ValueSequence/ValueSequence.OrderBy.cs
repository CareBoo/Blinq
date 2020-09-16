using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, OrderBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>> OrderBy<T, TSource, TKey, TKeySelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var keyComparer = KeyComparer.New(keySelector, default(DefaultComparer<TKey>));
            var seq = OrderBySequence<T>.New(source.Source, keyComparer);
            return ValueSequence<T>.New(seq);
        }

        public static ValueSequence<T, OrderBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, TComparer>>> OrderBy<T, TSource, TKey, TKeySelector, TComparer>(
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
            var keyComparer = KeyComparer.New(keySelector, comparer);
            var seq = OrderBySequence<T>.New(source.Source, keyComparer);
            return ValueSequence<T>.New(seq);
        }
    }

    public struct OrderBySequence<T, TSource, TComparer>
        : IOrderedSequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
        where TComparer : struct, IComparer<T>
    {
        readonly TSource source;
        readonly TComparer comparer;

        public OrderBySequence(TSource source, TComparer comparer)
        {
            this.source = source;
            this.comparer = comparer;
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
            return source.Execute();
        }
    }

    public static class OrderBySequence<T>
        where T : struct
    {
        public static OrderBySequence<T, TSource, TComparer> New<TSource, TComparer>(
            TSource source,
            TComparer comparer
            )
            where TSource : struct, ISequence<T>
            where TComparer : struct, IComparer<T>
        {
            return new OrderBySequence<T, TSource, TComparer>(source, comparer);
        }
    }

    public struct DefaultComparer<T> : IComparer<T>
        where T : struct, IComparable<T>
    {
        public int Compare(T x, T y)
        {
            return x.CompareTo(y);
        }
    }

    public struct KeyComparer<T, TKey, TKeySelector, TComparer> : IComparer<T>
        where T : struct
        where TKey : struct
        where TKeySelector : struct, IFunc<T, TKey>
        where TComparer : struct, IComparer<TKey>
    {
        readonly ValueFunc<T, TKey>.Impl<TKeySelector> keySelector;
        readonly TComparer comparer;

        public KeyComparer(
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector,
            TComparer comparer
            )
        {
            this.keySelector = keySelector;
            this.comparer = comparer;
        }

        public int Compare(T x, T y)
        {
            var xKey = keySelector.Invoke(x);
            var yKey = keySelector.Invoke(y);
            return comparer.Compare(xKey, yKey);
        }
    }

    public static class KeyComparer
    {
        public static KeyComparer<T, TKey, TKeySelector, TComparer> New<T, TKey, TKeySelector, TComparer>(
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector,
            TComparer comparer
            )
            where T : struct
            where TKey : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TComparer : struct, IComparer<TKey>
        {
            return new KeyComparer<T, TKey, TKeySelector, TComparer>(keySelector, comparer);
        }
    }
}
