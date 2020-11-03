using Unity.Collections;
using CareBoo.Burst.Delegates;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, SelectManySequence<T, NativeArraySequence<T>, TCollection, TResult, TCollectionSelector, TResultSelector>> SelectMany<T, TCollection, TResult, TCollectionSelector, TResultSelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector,
            in ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TCollection : struct
            where TResult : struct
            where TCollectionSelector : struct, IFunc<T, NativeArray<TCollection>>
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.SelectMany(in collectionSelector, in resultSelector);
        }

        public static ValueSequence<TResult, SelectManyIndexSequence<T, NativeArraySequence<T>, TCollection, TResult, TCollectionSelector, TResultSelector>> SelectMany<T, TCollection, TResult, TCollectionSelector, TResultSelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, int, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector,
            in ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TCollection : struct
            where TResult : struct
            where TCollectionSelector : struct, IFunc<T, int, NativeArray<TCollection>>
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.SelectMany(in collectionSelector, in resultSelector);
        }

        public static ValueSequence<TResult, SelectManySequence<T, NativeArraySequence<T>, TResult, TResult, TSelector, RightSelector<T, TResult>>> SelectMany<T, TResult, TSelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, NativeArray<TResult>>.Struct<TSelector> selector
            )
            where T : struct
            where TResult : struct
            where TSelector : struct, IFunc<T, NativeArray<TResult>>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.SelectMany(in selector);
        }

        public static ValueSequence<TResult, SelectManyIndexSequence<T, NativeArraySequence<T>, TResult, TResult, TSelector, RightSelector<T, TResult>>> SelectMany<T, TResult, TSelector>(
            this in NativeArray<T> source,
            in ValueFunc<T, int, NativeArray<TResult>>.Struct<TSelector> selector
            )
            where T : struct
            where TResult : struct
            where TSelector : struct, IFunc<T, int, NativeArray<TResult>>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.SelectMany(in selector);
        }
    }
}
