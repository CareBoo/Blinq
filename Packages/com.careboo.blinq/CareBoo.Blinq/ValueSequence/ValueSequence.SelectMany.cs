using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, SelectManySequence<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>> SelectMany<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, NativeArray<TCollection>>.Impl<TCollectionSelector> collectionSelector,
            ValueFunc<T, TCollection, TResult>.Impl<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TCollection : struct
            where TResult : struct
            where TCollectionSelector : struct, IFunc<T, NativeArray<TCollection>>
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            var seq = SelectManySequence.New(source.Source, collectionSelector, resultSelector);
            return ValueSequence<TResult>.New(seq);
        }

        public static ValueSequence<TResult, SelectManyIndexSequence<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>> SelectMany<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, int, NativeArray<TCollection>>.Impl<TCollectionSelector> collectionSelector,
            ValueFunc<T, TCollection, TResult>.Impl<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TCollection : struct
            where TResult : struct
            where TCollectionSelector : struct, IFunc<T, int, NativeArray<TCollection>>
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            var seq = SelectManySequence.New(source.Source, collectionSelector, resultSelector);
            return ValueSequence<TResult>.New(seq);
        }

        public static ValueSequence<TResult, SelectManySequence<T, TSource, TResult, TResult, TSelector, RightSelector<T, TResult>>> SelectMany<T, TSource, TResult, TSelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, NativeArray<TResult>>.Impl<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TResult : struct
            where TSelector : struct, IFunc<T, NativeArray<TResult>>
        {
            var seq = SelectManySequence.New(source.Source, selector, UtilFunctions.RightSelector<T, TResult>());
            return ValueSequence<TResult>.New(seq);
        }

        public static ValueSequence<TResult, SelectManyIndexSequence<T, TSource, TResult, TResult, TSelector, RightSelector<T, TResult>>> SelectMany<T, TSource, TResult, TSelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, int, NativeArray<TResult>>.Impl<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TResult : struct
            where TSelector : struct, IFunc<T, int, NativeArray<TResult>>
        {
            var seq = SelectManySequence.New(source.Source, selector, UtilFunctions.RightSelector<T, TResult>());
            return ValueSequence<TResult>.New(seq);
        }
    }

    public struct SelectManySequence<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>
        : ISequence<TResult>
        where T : struct
        where TSource : struct, ISequence<T>
        where TCollection : struct
        where TResult : struct
        where TCollectionSelector : struct, IFunc<T, NativeArray<TCollection>>
        where TResultSelector : struct, IFunc<T, TCollection, TResult>
    {
        readonly TSource source;
        readonly ValueFunc<T, NativeArray<TCollection>>.Impl<TCollectionSelector> collectionSelector;
        readonly ValueFunc<T, TCollection, TResult>.Impl<TResultSelector> resultSelector;

        public SelectManySequence(
            TSource source,
            ValueFunc<T, NativeArray<TCollection>>.Impl<TCollectionSelector> collectionSelector,
            ValueFunc<T, TCollection, TResult>.Impl<TResultSelector> resultSelector
            )
        {
            this.source = source;
            this.collectionSelector = collectionSelector;
            this.resultSelector = resultSelector;
        }

        public NativeList<TResult> Execute()
        {
            using (var srcList = source.Execute())
                return Execute(srcList);
        }

        private NativeList<TResult> Execute(NativeList<T> srcList)
        {
            var output = new NativeList<TResult>(Allocator.Temp);
            for (var i = 0; i < srcList.Length; i++)
            {
                var srcElement = srcList[i];
                using (var resultArr = GetResults(srcElement))
                    output.AddRange(resultArr);
            }
            return output;
        }

        private NativeArray<TResult> GetResults(T srcElement)
        {
            using (var collectionArr = collectionSelector.Invoke(srcElement))
            {
                var resultArr = new NativeArray<TResult>(collectionArr.Length, Allocator.Temp);
                for (var i = 0; i < collectionArr.Length; i++)
                    resultArr[i] = resultSelector.Invoke(srcElement, collectionArr[i]);
                return resultArr;
            }
        }
    }

    public struct SelectManyIndexSequence<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>
        : ISequence<TResult>
        where T : struct
        where TSource : struct, ISequence<T>
        where TCollection : struct
        where TResult : struct
        where TCollectionSelector : struct, IFunc<T, int, NativeArray<TCollection>>
        where TResultSelector : struct, IFunc<T, TCollection, TResult>
    {
        readonly TSource source;
        readonly ValueFunc<T, int, NativeArray<TCollection>>.Impl<TCollectionSelector> collectionSelector;
        readonly ValueFunc<T, TCollection, TResult>.Impl<TResultSelector> resultSelector;

        public SelectManyIndexSequence(
            TSource source,
            ValueFunc<T, int, NativeArray<TCollection>>.Impl<TCollectionSelector> collectionSelector,
            ValueFunc<T, TCollection, TResult>.Impl<TResultSelector> resultSelector
            )
        {
            this.source = source;
            this.collectionSelector = collectionSelector;
            this.resultSelector = resultSelector;
        }

        public NativeList<TResult> Execute()
        {
            using (var srcList = source.Execute())
                return Execute(srcList);
        }

        private NativeList<TResult> Execute(NativeList<T> srcList)
        {
            var output = new NativeList<TResult>(Allocator.Temp);
            for (var i = 0; i < srcList.Length; i++)
            {
                var srcElement = srcList[i];
                using (var resultArr = GetResults(srcElement, i))
                    output.AddRange(resultArr);
            }
            return output;
        }

        private NativeArray<TResult> GetResults(T srcElement, int index)
        {
            using (var collectionArr = collectionSelector.Invoke(srcElement, index))
            {
                var resultArr = new NativeArray<TResult>(collectionArr.Length, Allocator.Temp);
                for (var i = 0; i < collectionArr.Length; i++)
                    resultArr[i] = resultSelector.Invoke(srcElement, collectionArr[i]);
                return resultArr;
            }
        }
    }

    public static class SelectManySequence
    {
        public static SelectManySequence<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector> New<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>(
            TSource source,
            ValueFunc<T, NativeArray<TCollection>>.Impl<TCollectionSelector> collectionSelector,
            ValueFunc<T, TCollection, TResult>.Impl<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TCollection : struct
            where TResult : struct
            where TCollectionSelector : struct, IFunc<T, NativeArray<TCollection>>
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            return new SelectManySequence<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>(source, collectionSelector, resultSelector);
        }

        public static SelectManyIndexSequence<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector> New<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>(
            TSource source,
            ValueFunc<T, int, NativeArray<TCollection>>.Impl<TCollectionSelector> collectionSelector,
            ValueFunc<T, TCollection, TResult>.Impl<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TCollection : struct
            where TResult : struct
            where TCollectionSelector : struct, IFunc<T, int, NativeArray<TCollection>>
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            return new SelectManyIndexSequence<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>(source, collectionSelector, resultSelector);
        }
    }
}
