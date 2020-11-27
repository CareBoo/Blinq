using System;
using System.Collections.Generic;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            T,
            ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>,
            SequenceEnumerator<T, ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>>>
        ThenByDescending<T, TSource, TSourceEnumerator, TKey, TKeySelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector
            )
            where T : struct
            where TSource : struct, IOrderedSequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            var sourceSeq = source.Source;
            var comparer = default(DefaultComparer<TKey>);
            var keyComparer = KeyComparer.New(keySelector, in comparer);
            var descending = Descending<T>.New(in keyComparer);
            var seq = new ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>(in sourceSeq, in descending);
            return ValueSequence<T, SequenceEnumerator<T, ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>>>.New(in seq);
        }

        public static ValueSequence<
            T,
            ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>,
            SequenceEnumerator<T, ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>>>
        ThenByDescending<T, TSource, TSourceEnumerator, TKey, TKeySelector, TComparer>(
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
            var sourceSeq = source.Source;
            var keyComparer = KeyComparer.New(keySelector, in comparer);
            var descending = Descending<T>.New(in keyComparer);
            var seq = new ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>(in sourceSeq, in descending);
            return ValueSequence<T, SequenceEnumerator<T, ThenBySequence<T, TSource, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>>>.New(in seq);
        }
    }
}
