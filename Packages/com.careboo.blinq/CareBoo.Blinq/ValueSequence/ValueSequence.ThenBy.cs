using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            T,
            ThenBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>,
            SequenceEnumerator<T, ThenBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>>
        ThenBy<T, TSource, TSourceEnumerator, TKey, TKeySelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, IOrderedSequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var sourceSeq = source.GetEnumerator();
            var comparer = default(DefaultComparer<TKey>);
            var keyComparer = KeyComparer.New(keySelector, in comparer);
            var seq = ThenBySequence<T>.New(in source.Source, in keyComparer);
            return ValueSequence<T, SequenceEnumerator<T, ThenBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>>.New(in seq);
        }

        public static ValueSequence<
            T,
            ThenBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, TComparer>>,
            SequenceEnumerator<T, ThenBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, TComparer>>>>
        ThenBy<T, TSource, TSourceEnumerator, TKey, TKeySelector, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in TComparer comparer
            )
            where T : struct
            where TSource : struct, IOrderedSequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TComparer : struct, IComparer<TKey>
        {
            var keyComparer = KeyComparer.New(keySelector, in comparer);
            var seq = ThenBySequence<T>.New(in source.Source, in keyComparer);
            return ValueSequence<T, SequenceEnumerator<T, ThenBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, TComparer>>>>.New(in seq);
        }
    }

    public struct ThenBySequence<T, TSource, TComparer>
        : IOrderedSequence<T, SequenceEnumerator<T, ThenBySequence<T, TSource, TComparer>>>
        where T : struct
        where TSource : struct, IOrderedNativeListConvertible<T>
        where TComparer : struct, IComparer<T>
    {
        readonly TSource source;
        readonly ThenByComparer<T, TSource, TComparer> comparer;

        public ThenBySequence(in TSource source, in TComparer comparer)
        {
            this.source = source;
            this.comparer = ThenByComparer<T>.New(in source, in comparer);
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

        public SequenceEnumerator<T, ThenBySequence<T, TSource, TComparer>> GetEnumerator()
        {
            return SequenceEnumerator<T>.New(in this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class ThenBySequence<T>
        where T : struct
    {
        public static ThenBySequence<T, TSource, TComparer> New<TSource, TComparer>(in TSource source, in TComparer comparer)
            where TSource : struct, IOrderedNativeListConvertible<T>
            where TComparer : struct, IComparer<T>
        {
            return new ThenBySequence<T, TSource, TComparer>(in source, in comparer);
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
