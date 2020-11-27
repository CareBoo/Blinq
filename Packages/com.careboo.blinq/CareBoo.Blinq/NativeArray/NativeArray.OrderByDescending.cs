using System;
using System.Collections.Generic;
using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            T,
            OrderBySequence<T, NativeArraySequence<T>, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>,
            SequenceEnumerator<T, OrderBySequence<T, NativeArraySequence<T>, Descending<T, KeyComparer<T, TKey, TKeySelector, DefaultComparer<TKey>>>>>>
        OrderByDescending<T, TKey, TKeySelector>(
            this in NativeArray<T> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector
            )
            where T : struct
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            return source.ToValueSequence().OrderByDescending(keySelector);
        }

        public static ValueSequence<
            T,
            OrderBySequence<T, NativeArraySequence<T>, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>,
            SequenceEnumerator<T, OrderBySequence<T, NativeArraySequence<T>, Descending<T, KeyComparer<T, TKey, TKeySelector, TComparer>>>>>
        OrderByDescending<T, TKey, TKeySelector, TComparer>(
            this in NativeArray<T> source,
            ValueFunc<T, TKey>.Struct<TKeySelector> keySelector,
            in TComparer comparer
            )
            where T : struct
            where TKey : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TComparer : struct, IComparer<TKey>
        {
            return source.ToValueSequence().OrderByDescending(keySelector, in comparer);
        }
    }
}
