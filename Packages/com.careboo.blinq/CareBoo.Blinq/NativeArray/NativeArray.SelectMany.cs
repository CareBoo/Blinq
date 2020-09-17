using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, SelectManySequence<T, NativeArraySequence<T>, TCollection, TResult, TCollectionSelector, TResultSelector>> SelectMany<T, TCollection, TResult, TCollectionSelector, TResultSelector>(
            this ref NativeArray<T> source,
            ValueFunc<T, NativeArray<TCollection>>.Impl<TCollectionSelector> collectionSelector,
            ValueFunc<T, TCollection, TResult>.Impl<TResultSelector> resultSelector
            )
            where T : struct
            where TCollection : struct
            where TResult : struct
            where TCollectionSelector : struct, IFunc<T, NativeArray<TCollection>>
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            return source.ToValueSequence().SelectMany(collectionSelector, resultSelector);
        }

        public static ValueSequence<TResult, SelectManyIndexSequence<T, NativeArraySequence<T>, TCollection, TResult, TCollectionSelector, TResultSelector>> SelectMany<T, TCollection, TResult, TCollectionSelector, TResultSelector>(
            this ref NativeArray<T> source,
            ValueFunc<T, int, NativeArray<TCollection>>.Impl<TCollectionSelector> collectionSelector,
            ValueFunc<T, TCollection, TResult>.Impl<TResultSelector> resultSelector
            )
            where T : struct
            where TCollection : struct
            where TResult : struct
            where TCollectionSelector : struct, IFunc<T, int, NativeArray<TCollection>>
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            return source.ToValueSequence().SelectMany(collectionSelector, resultSelector);
        }

        public static ValueSequence<TResult, SelectManySequence<T, NativeArraySequence<T>, TResult, TResult, TSelector, NoneResultSelector<T, TResult>>> SelectMany<T, TResult, TSelector>(
            this ref NativeArray<T> source,
            ValueFunc<T, NativeArray<TResult>>.Impl<TSelector> selector
            )
            where T : struct
            where TResult : struct
            where TSelector : struct, IFunc<T, NativeArray<TResult>>
        {
            return source.ToValueSequence().SelectMany(selector);
        }

        public static ValueSequence<TResult, SelectManyIndexSequence<T, NativeArraySequence<T>, TResult, TResult, TSelector, NoneResultSelector<T, TResult>>> SelectMany<T, TResult, TSelector>(
            this ref NativeArray<T> source,
            ValueFunc<T, int, NativeArray<TResult>>.Impl<TSelector> selector
            )
            where T : struct
            where TResult : struct
            where TSelector : struct, IFunc<T, int, NativeArray<TResult>>
        {
            return source.ToValueSequence().SelectMany(selector);
        }
    }
}
