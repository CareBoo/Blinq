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
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var sourceSeq = source.GetEnumerator();
            var comparer = default(DefaultComparer<TKey>);
            var keyComparer = KeyComparer.New(in keySelector, in comparer);
            var seq = OrderBySequence<T>.New(ref sourceSeq, in keyComparer);
            return ValueSequence<T>.New(ref seq);
        }

        public static ValueSequence<T, OrderBySequence<T, TSource, KeyComparer<T, TKey, TKeySelector, TComparer>>> OrderBy<T, TSource, TKey, TKeySelector, TComparer>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in TComparer comparer
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TComparer : struct, IComparer<TKey>
        {
            var sourceSeq = source.GetEnumerator();
            var keyComparer = KeyComparer.New(in keySelector, in comparer);
            var seq = OrderBySequence<T>.New(ref sourceSeq, in keyComparer);
            return ValueSequence<T>.New(ref seq);
        }
    }

    public struct OrderBySequence<T, TSource, TComparer>
        : IOrderedSequence<T>
        where T : struct
        where TSource : struct, ISequence<T>
        where TComparer : struct, IComparer<T>
    {
        TSource source;
        readonly TComparer comparer;

        int currentIndex;
        NativeList<T> list;

        public OrderBySequence(ref TSource source, in TComparer comparer)
        {
            this.source = source;
            this.comparer = comparer;
            currentIndex = -1;
            list = default;
        }

        public T Current => list[currentIndex];

        object IEnumerator.Current => Current;

        public int Compare(T x, T y)
        {
            return comparer.Compare(x, y);
        }

        public NativeList<T> ToList()
        {
            var list = ToUnorderedList();
            list.Sort(this);
            return list;
        }

        public NativeList<T> ToUnorderedList()
        {
            return source.ToList();
        }

        public bool MoveNext()
        {
            if (!list.IsCreated)
                list = ToList();
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
            list.Dispose();
        }
    }

    public static class OrderBySequence<T>
        where T : struct
    {
        public static OrderBySequence<T, TSource, TComparer> New<TSource, TComparer>(
            ref TSource source,
            in TComparer comparer
            )
            where TSource : struct, ISequence<T>
            where TComparer : struct, IComparer<T>
        {
            return new OrderBySequence<T, TSource, TComparer>(ref source, in comparer);
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
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
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
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in TComparer comparer
            )
            where T : struct
            where TKey : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TComparer : struct, IComparer<TKey>
        {
            return new KeyComparer<T, TKey, TKeySelector, TComparer>(in keySelector, in comparer);
        }
    }
}
