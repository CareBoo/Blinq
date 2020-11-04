using System;
using System.Collections.Generic;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>> ThenByDescending<T, TSource, TKey, TKeySelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, TKey>.Struct<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, IOrderedSequence<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var sourceSeq = source.GetEnumerator();
            var comparer = default(DefaultComparer<TKey>);
            var keyComparer = KeyComparer.New(in keySelector, in comparer);
            var descending = Descending<T>.New(in keyComparer);
            var seq = new ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>(ref sourceSeq, in descending);
            return ValueSequence<T>.New(ref seq);
        }

        public static ValueSequence<T, ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>> ThenByDescending<T, TSource, TKey, TKeySelector, TComparer>(
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
            var descending = Descending<T>.New(in keyComparer);
            var seq = new ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>(ref sourceSeq, in descending);
            return ValueSequence<T>.New(ref seq);
        }
    }
}
