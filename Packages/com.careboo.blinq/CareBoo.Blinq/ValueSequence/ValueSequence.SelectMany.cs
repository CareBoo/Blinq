﻿using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;
using System;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, SelectManySequence<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>> SelectMany<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector,
            in ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TCollection : struct
            where TResult : struct
            where TCollectionSelector : struct, IFunc<T, NativeArray<TCollection>>
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            var sourceSeq = source.GetEnumerator();
            var seq = SelectManySequence.New(ref sourceSeq, in collectionSelector, in resultSelector);
            return ValueSequence<TResult>.New(ref seq);
        }

        public static ValueSequence<TResult, SelectManyIndexSequence<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>> SelectMany<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, int, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector,
            in ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TCollection : struct
            where TResult : struct
            where TCollectionSelector : struct, IFunc<T, int, NativeArray<TCollection>>
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            var sourceSeq = source.GetEnumerator();
            var seq = SelectManySequence.New(ref sourceSeq, in collectionSelector, in resultSelector);
            return ValueSequence<TResult>.New(ref seq);
        }

        public static ValueSequence<TResult, SelectManySequence<T, TSource, TResult, TResult, TSelector, RightSelector<T, TResult>>> SelectMany<T, TSource, TResult, TSelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, NativeArray<TResult>>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TResult : struct
            where TSelector : struct, IFunc<T, NativeArray<TResult>>
        {
            var sourceSeq = source.GetEnumerator();
            var rightSelector = UtilFunctions.RightSelector<T, TResult>();
            var seq = SelectManySequence.New(ref sourceSeq, in selector, in rightSelector);
            return ValueSequence<TResult>.New(ref seq);
        }

        public static ValueSequence<TResult, SelectManyIndexSequence<T, TSource, TResult, TResult, TSelector, RightSelector<T, TResult>>> SelectMany<T, TSource, TResult, TSelector>(
            this in ValueSequence<T, TSource> source,
            in ValueFunc<T, int, NativeArray<TResult>>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TResult : struct
            where TSelector : struct, IFunc<T, int, NativeArray<TResult>>
        {
            var sourceSeq = source.GetEnumerator();
            var rightSelector = UtilFunctions.RightSelector<T, TResult>();
            var seq = SelectManySequence.New(ref sourceSeq, in selector, in rightSelector);
            return ValueSequence<TResult>.New(ref seq);
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
        TSource source;
        readonly ValueFunc<T, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector;
        readonly ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector;

        NativeArray<TCollection> currentCollection;
        NativeArray<TCollection>.Enumerator currentEnumerator;

        public SelectManySequence(
            ref TSource source,
            in ValueFunc<T, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector,
            in ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector
            )
        {
            this.source = source;
            this.collectionSelector = collectionSelector;
            this.resultSelector = resultSelector;
            currentCollection = default;
            currentEnumerator = default;
        }

        public TResult Current => resultSelector.Invoke(source.Current, currentEnumerator.Current);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            if (currentCollection.IsCreated)
                currentCollection.Dispose();
            source.Dispose();
        }

        public bool MoveNext()
        {
            while (!currentEnumerator.MoveNext())
            {
                if (!source.MoveNext())
                    return false;
                if (currentCollection.IsCreated)
                    currentCollection.Dispose();
                currentCollection = collectionSelector.Invoke(source.Current);
                currentEnumerator = currentCollection.GetEnumerator();
            }
            return true;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<TResult> ToList()
        {
            using (var srcList = source.ToList())
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
        TSource source;
        readonly ValueFunc<T, int, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector;
        readonly ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector;

        int currentIndex;
        NativeArray<TCollection> currentCollection;
        NativeArray<TCollection>.Enumerator currentEnumerator;

        public SelectManyIndexSequence(
            ref TSource source,
            in ValueFunc<T, int, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector,
            in ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector
            )
        {
            this.source = source;
            this.collectionSelector = collectionSelector;
            this.resultSelector = resultSelector;
            currentIndex = -1;
            currentCollection = default;
            currentEnumerator = default;
        }

        public TResult Current => resultSelector.Invoke(source.Current, currentEnumerator.Current);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            if (currentCollection.IsCreated)
                currentCollection.Dispose();
            source.Dispose();
        }

        public bool MoveNext()
        {
            while (!currentEnumerator.MoveNext())
            {
                if (!source.MoveNext())
                    return false;
                currentIndex += 1;
                if (currentCollection.IsCreated)
                    currentCollection.Dispose();
                currentCollection = collectionSelector.Invoke(source.Current, currentIndex);
                currentEnumerator = currentCollection.GetEnumerator();
            }
            return true;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<TResult> ToList()
        {
            using (var srcList = source.ToList())
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
            ref TSource source,
            in ValueFunc<T, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector,
            in ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TCollection : struct
            where TResult : struct
            where TCollectionSelector : struct, IFunc<T, NativeArray<TCollection>>
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            return new SelectManySequence<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>(ref source, in collectionSelector, in resultSelector);
        }

        public static SelectManyIndexSequence<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector> New<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>(
            ref TSource source,
            in ValueFunc<T, int, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector,
            in ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TCollection : struct
            where TResult : struct
            where TCollectionSelector : struct, IFunc<T, int, NativeArray<TCollection>>
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            return new SelectManyIndexSequence<T, TSource, TCollection, TResult, TCollectionSelector, TResultSelector>(ref source, in collectionSelector, in resultSelector);
        }
    }
}
