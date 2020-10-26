using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ThenBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>> ThenBy<T, TSource, TKey, TKeySelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, IOrderedSequence<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var keyComparer = KeyComparer.New(keySelector, default(DefaultComparer<TKey>));
            var seq = ThenBySequence<T>.New(source.Source, keyComparer);
            return ValueSequence<T>.New(seq);
        }

        public static ValueSequence<T, ThenBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, TComparer>>> ThenBy<T, TSource, TKey, TKeySelector, TComparer>(
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
            var seq = ThenBySequence<T>.New(source.Source, keyComparer);
            return ValueSequence<T>.New(seq);
        }
    }

    public struct ThenBySequence<T, TSource, TComparer>
        : IOrderedSequence<T>
        where T : struct
        where TSource : struct, IOrderedSequence<T>
        where TComparer : struct, IComparer<T>
    {
        readonly TSource Source;
        readonly ThenByComparer<T, TSource, TComparer> Comparer;

        int currentIndex;
        NativeList<T> list;

        public T Current => list[currentIndex];

        object IEnumerator.Current => Current;

        public ThenBySequence(TSource source, TComparer comparer)
        {
            Source = source;
            Comparer = ThenByComparer<T>.New(source, comparer);
            currentIndex = default;
            list = default;
        }

        public int Compare(T x, T y)
        {
            return Comparer.Compare(x, y);
        }

        public NativeList<T> ToList()
        {
            var unordered = ToUnorderedList();
            unordered.Sort(this);
            return unordered;
        }

        public NativeList<T> ToUnorderedList()
        {
            return Source.ToUnorderedList();
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

    public static class ThenBySequence<T>
        where T : struct
    {
        public static ThenBySequence<T, TSource, TComparer> New<TSource, TComparer>(TSource source, TComparer comparer)
            where TSource : struct, IOrderedSequence<T>
            where TComparer : struct, IComparer<T>
        {
            return new ThenBySequence<T, TSource, TComparer>(source, comparer);
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

    public static class ThenByComparer<T>
        where T : struct
    {
        public static ThenByComparer<T, TFirstComparer, TSecondComparer> New<TFirstComparer, TSecondComparer>(
            TFirstComparer firstComparer,
            TSecondComparer secondComparer
            )
            where TFirstComparer : struct, IComparer<T>
            where TSecondComparer : struct, IComparer<T>
        {
            return new ThenByComparer<T, TFirstComparer, TSecondComparer>(firstComparer, secondComparer);
        }
    }
}
