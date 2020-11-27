using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            TResult,
            SelectManySequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TCollection, IgnoreIndex<T, NativeArray<TCollection>, TCollectionSelector>, TResult, TResultSelector>,
            SelectManySequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TCollection, IgnoreIndex<T, NativeArray<TCollection>, TCollectionSelector>, TResult, TResultSelector>.Enumerator>
        SelectMany<T, TCollection, TResult, TCollectionSelector, TResultSelector>(
            this in NativeArray<T> source,
             ValueFunc<T, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector,
             ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TCollection : struct
            where TResult : struct
            where TCollectionSelector : struct, IFunc<T, NativeArray<TCollection>>
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.SelectMany(collectionSelector, resultSelector);
        }

        public static ValueSequence<
            TResult,
            SelectManySequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TCollection, TCollectionSelector, TResult, TResultSelector>,
            SelectManySequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TCollection, TCollectionSelector, TResult, TResultSelector>.Enumerator>
        SelectMany<T, TCollection, TResult, TCollectionSelector, TResultSelector>(
            this in NativeArray<T> source,
            ValueFunc<T, int, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector,
            ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TCollection : struct
            where TResult : struct
            where TCollectionSelector : struct, IFunc<T, int, NativeArray<TCollection>>
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.SelectMany(collectionSelector, resultSelector);
        }

        public static ValueSequence<
            TResult,
            SelectManySequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TResult, IgnoreIndex<T, NativeArray<TResult>, TSelector>, TResult, RightSelector<T, TResult>>,
            SelectManySequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TResult, IgnoreIndex<T, NativeArray<TResult>, TSelector>, TResult, RightSelector<T, TResult>>.Enumerator>
        SelectMany<T, TResult, TSelector>(
            this in NativeArray<T> source,
             ValueFunc<T, NativeArray<TResult>>.Struct<TSelector> selector
            )
            where T : struct
            where TResult : struct
            where TSelector : struct, IFunc<T, NativeArray<TResult>>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.SelectMany(selector);
        }

        public static ValueSequence<
            TResult,
            SelectManySequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TResult, TSelector, TResult, RightSelector<T, TResult>>,
            SelectManySequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TResult, TSelector, TResult, RightSelector<T, TResult>>.Enumerator>
        SelectMany<T, TResult, TSelector>(
            this in NativeArray<T> source,
            ValueFunc<T, int, NativeArray<TResult>>.Struct<TSelector> selector
            )
            where T : struct
            where TResult : struct
            where TSelector : struct, IFunc<T, int, NativeArray<TResult>>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.SelectMany(selector);
        }
    }
}
