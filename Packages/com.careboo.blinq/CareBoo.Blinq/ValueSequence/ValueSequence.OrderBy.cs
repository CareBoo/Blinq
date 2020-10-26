using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using CareBoo.Burst.Delegates;
namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, OrderBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>> OrderBy<T, TSource, TKey, TKeySelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector
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
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
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
        readonly TSource Source;
        readonly TComparer Comparer;

        int currentIndex;
        NativeList<T> list;

        public T Current => list[currentIndex];

        object IEnumerator.Current => Current;

        public OrderBySequence(TSource source, TComparer comparer)
        {
            Source = source;
            Comparer = comparer;
            currentIndex = default;
            list = default;
        }

        public int Compare(T x, T y)
        {
            return Comparer.Compare(x, y);
        }

        public NativeList<T> ToList()
        {
            var list = ToUnorderedList();
            list.Sort(this);
            return list;
        }

        public NativeList<T> ToUnorderedList()
        {
            return Source.ToList();
        }

        public bool MoveNext()
        {
            if (!list.IsCreated)
                list = ToList();
            else
                currentIndex += 1;
            return currentIndex < list.Length;
        }

        public void Reset()
        {
            if (list.IsCreated)
                list.Dispose();
            list = default;
            currentIndex = default;
        }

        public void Dispose()
        {
            if (list.IsCreated)
                list.Dispose();
            Source.Dispose();
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
        readonly ValueFunc<T, TKey>.Struct<TKeySelector> keySelector;
        readonly TComparer comparer;

        public KeyComparer(
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
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
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
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
