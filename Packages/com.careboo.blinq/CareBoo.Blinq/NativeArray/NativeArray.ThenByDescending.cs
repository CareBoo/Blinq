using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, OrderByDescendingSequence<T, NativeArraySequence<T>, TKey, TKeySelector>> ThenByDescending<T, TKey, TKeySelector>(
            this ref NativeArray<T> source,
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector
            )
            where T : struct
            where TKey : struct, IComparable<TKey>
            where TKeySelector : struct, IFunc<T, TKey>
        {
            return source.ToValueSequence().OrderByDescending(keySelector);
        }

        public static ValueSequence<T, OrderByComparerDescendingSequence<T, NativeArraySequence<T>, TKey, TKeySelector, TComparer>> ThenByDescending<T, TKey, TKeySelector, TComparer>(
            this ref NativeArray<T> source,
            ValueFunc<T, TKey>.Impl<TKeySelector> keySelector,
            TComparer comparer
            )
            where T : struct
            where TKey : struct
            where TKeySelector : struct, IFunc<T, TKey>
            where TComparer : struct, IComparer<TKey>
        {
            return source.ToValueSequence().OrderByDescending(keySelector, comparer);
        }
    }
}
