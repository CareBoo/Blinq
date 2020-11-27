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
            OrderBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>,
            SequenceEnumerator<T, OrderBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>>
        OrderBy<T, TSource, TSourceEnumerator, TKey, TKeySelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var sourceSeq = source.Source;
            var comparer = default(DefaultComparer<TKey>);
            var keyComparer = KeyComparer.New(keySelector, in comparer);
            var seq = new OrderBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>(in sourceSeq, in keyComparer);
            return ValueSequence<T, SequenceEnumerator<T, OrderBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>>.New(in seq);
        }

        public static ValueSequence<
            T,
            OrderBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, TComparer>>,
            SequenceEnumerator<T, OrderBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, TComparer>>>>
        OrderBy<T, TSource, TSourceEnumerator, TKey, TKeySelector, TComparer>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in TComparer comparer
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TComparer : struct, IComparer<TKey>
        {
            var sourceSeq = source.Source;
            var keyComparer = KeyComparer.New(keySelector, in comparer);
            var seq = new OrderBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, TComparer>>(in sourceSeq, in keyComparer);
            return ValueSequence<T, SequenceEnumerator<T, OrderBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, TComparer>>>>.New(in seq);
        }
    }

    public struct OrderBySequence<T, TSource, TComparer>
        : IOrderedSequence<T, SequenceEnumerator<T, OrderBySequence<T, TSource, TComparer>>>
        where T : struct
        where TSource : struct, INativeListConvertible<T>
        where TComparer : struct, IComparer<T>
    {
        readonly TSource source;

        readonly TComparer comparer;

        public OrderBySequence(in TSource source, in TComparer comparer)
        {
            this.source = source;
            this.comparer = comparer;
        }

        public int Compare(T x, T y)
        {
            return comparer.Compare(x, y);
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var list = ToUnorderedList(allocator);
            list.Sort(this);
            return list;
        }

        public NativeList<T> ToUnorderedList(Allocator allocator)
        {
            return source.ToNativeList(allocator);
        }

        public SequenceEnumerator<T, OrderBySequence<T, TSource, TComparer>> GetEnumerator()
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
            in TComparer comparer
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
            in TComparer comparer
            )
            where T : struct
            where TKey : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TComparer : struct, IComparer<TKey>
        {
            return new KeyComparer<T, TKey, TKeySelector, TComparer>(keySelector, in comparer);
        }
    }
}
