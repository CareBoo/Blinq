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
            this in ValueSequence<T, TSource> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, IOrderedSequence<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var sourceSeq = source.GetEnumerator();
            var comparer = default(DefaultComparer<TKey>);
            var keyComparer = KeyComparer.New(in keySelector, in comparer);
            var seq = ThenBySequence<T>.New(ref sourceSeq, in keyComparer);
            return ValueSequence<T>.New(ref seq);
        }

        public static ValueSequence<T, ThenBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, TComparer>>> ThenBy<T, TSource, TKey, TKeySelector, TComparer>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in TComparer comparer
            )
            where T : struct
            where TSource : struct, IOrderedSequence<T>
            where TKey : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TComparer : struct, IComparer<TKey>
        {
            var sourceSeq = source.GetEnumerator();
            var keyComparer = KeyComparer.New(in keySelector, in comparer);
            var seq = ThenBySequence<T>.New(ref sourceSeq, in keyComparer);
            return ValueSequence<T>.New(ref seq);
        }
    }

    public struct ThenBySequence<T, TSource, TComparer>
        : IOrderedSequence<T>
        where T : struct
        where TSource : struct, IOrderedSequence<T>
        where TComparer : struct, IComparer<T>
    {
        TSource source;
        readonly ThenByComparer<T, TSource, TComparer> comparer;

        int currentIndex;
        NativeList<T> list;

        public T Current => list[currentIndex];

        object IEnumerator.Current => Current;

        public ThenBySequence(ref TSource source, in TComparer comparer)
        {
            this.source = source;
            this.comparer = ThenByComparer<T>.New(in source, in comparer);
            currentIndex = default;
            list = default;
        }

        public int Compare(T x, T y)
        {
            return comparer.Compare(x, y);
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var unordered = ToUnorderedList(allocator);
            unordered.Sort(this);
            return unordered;
        }

        public NativeList<T> ToUnorderedList(Allocator allocator)
        {
            return source.ToUnorderedList(allocator);
        }

        public bool MoveNext()
        {
            if (!list.IsCreated)
                list = ToNativeList(Allocator.Temp);
            else
                currentIndex += 1;
            return currentIndex < list.Length;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
            source.Dispose();
            if (list.IsCreated)
                list.Dispose();
        }
    }

    public static class ThenBySequence<T>
        where T : struct
    {
        public static ThenBySequence<T, TSource, TComparer> New<TSource, TComparer>(ref TSource source, in TComparer comparer)
            where TSource : struct, IOrderedSequence<T>
            where TComparer : struct, IComparer<T>
        {
            return new ThenBySequence<T, TSource, TComparer>(ref source, in comparer);
        }
    }

    public struct ThenByComparer<T, TFirstComparer, TSecondComparer> : IComparer<T>
        where T : struct
        where TFirstComparer : struct, IComparer<T>
        where TSecondComparer : struct, IComparer<T>
    {
        readonly TFirstComparer firstComparer;
        readonly TSecondComparer secondComparer;

        public ThenByComparer(in TFirstComparer firstComparer, in TSecondComparer secondComparer)
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
            in TFirstComparer firstComparer,
            in TSecondComparer secondComparer
            )
            where TFirstComparer : struct, IComparer<T>
            where TSecondComparer : struct, IComparer<T>
        {
            return new ThenByComparer<T, TFirstComparer, TSecondComparer>(in firstComparer, in secondComparer);
        }
    }
}
