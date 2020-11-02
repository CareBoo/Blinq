﻿using System;
using System.Collections.Generic;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, OrderBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>> OrderByDescending<T, TSource, TKey, TKeySelector>(
            this ref ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var comparer = default(DefaultComparer<TKey>);
            var keyComparer = KeyComparer.New(in keySelector, in comparer);
            var descending = Descending<T>.New(in keyComparer);
            var sourceSeq = source.GetEnumerator();
            var seq = OrderBySequence<T>.New(ref sourceSeq, in descending);
            return ValueSequence<T>.New(ref seq);
        }

        public static ValueSequence<T, OrderBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>> OrderByDescending<T, TSource, TKey, TKeySelector, TComparer>(
            this ref ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in TComparer comparer
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TKey : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TComparer : struct, IComparer<TKey>
        {
            var keyComparer = KeyComparer.New(in keySelector, in comparer);
            var descending = Descending<T>.New(in keyComparer);
            var sourceSeq = source.GetEnumerator();
            var seq = OrderBySequence<T>.New(ref sourceSeq, in descending);
            return ValueSequence<T>.New(ref seq);
        }
    }

    public struct Descending<T, TComparer> : IComparer<T>
        where T : struct
        where TComparer : struct, IComparer<T>
    {
        readonly TComparer comparer;

        public Descending(in TComparer comparer)
        {
            this.comparer = comparer;
        }

        public int Compare(T x, T y)
        {
            return comparer.Compare(y, x);
        }
    }

    public static class Descending<T>
        where T : struct
    {
        public static Descending<T, TComparer> New<TComparer>(in TComparer comparer)
            where TComparer : struct, IComparer<T>
        {
            return new Descending<T, TComparer>(in comparer);
        }
    }
}
