using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;
using System;
using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            TResult,
            SelectManySequence<T, TSource, TSourceEnumerator, TCollection, IgnoreIndex<T, NativeArray<TCollection>, TCollectionSelector>, TResult, TResultSelector>,
            SelectManySequence<T, TSource, TSourceEnumerator, TCollection, IgnoreIndex<T, NativeArray<TCollection>, TCollectionSelector>, TResult, TResultSelector>.Enumerator>
        SelectMany<T, TSource, TSourceEnumerator, TCollection, TResult, TCollectionSelector, TResultSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector,
            ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TCollection : struct
            where TCollectionSelector : struct, IFunc<T, NativeArray<TCollection>>
            where TResult : struct
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            var indexSelector = UtilFunctions.IgnoreIndex(collectionSelector);
            var seq = SelectManySequence<T, TSourceEnumerator>.New(in source.Source, indexSelector, resultSelector);
            return ValueSequence<TResult, SelectManySequence<T, TSource, TSourceEnumerator, TCollection, IgnoreIndex<T, NativeArray<TCollection>, TCollectionSelector>, TResult, TResultSelector>.Enumerator>.New(in seq);
        }

        public static ValueSequence<
            TResult,
            SelectManySequence<T, TSource, TSourceEnumerator, TCollection, TCollectionSelector, TResult, TResultSelector>,
            SelectManySequence<T, TSource, TSourceEnumerator, TCollection, TCollectionSelector, TResult, TResultSelector>.Enumerator>
        SelectMany<T, TSource, TSourceEnumerator, TCollection, TResult, TCollectionSelector, TResultSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, int, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector,
            ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TCollection : struct
            where TCollectionSelector : struct, IFunc<T, int, NativeArray<TCollection>>
            where TResult : struct
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            var seq = SelectManySequence<T, TSourceEnumerator>.New(in source.Source, collectionSelector, resultSelector);
            return ValueSequence<TResult, SelectManySequence<T, TSource, TSourceEnumerator, TCollection, TCollectionSelector, TResult, TResultSelector>.Enumerator>.New(in seq);
        }

        public static ValueSequence<
            TResult,
            SelectManySequence<T, TSource, TSourceEnumerator, TResult, IgnoreIndex<T, NativeArray<TResult>, TSelector>, TResult, RightSelector<T, TResult>>,
            SelectManySequence<T, TSource, TSourceEnumerator, TResult, IgnoreIndex<T, NativeArray<TResult>, TSelector>, TResult, RightSelector<T, TResult>>.Enumerator>
        SelectMany<T, TSource, TSourceEnumerator, TResult, TSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, NativeArray<TResult>>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct
            where TSelector : struct, IFunc<T, NativeArray<TResult>>
        {
            var rightSelector = UtilFunctions.RightSelector<T, TResult>();
            var indexSelector = UtilFunctions.IgnoreIndex(selector);
            var seq = SelectManySequence<T, TSourceEnumerator>.New(in source.Source, indexSelector, rightSelector);
            return ValueSequence<TResult, SelectManySequence<T, TSource, TSourceEnumerator, TResult, IgnoreIndex<T, NativeArray<TResult>, TSelector>, TResult, RightSelector<T, TResult>>.Enumerator>.New(in seq);
        }

        public static ValueSequence<
            TResult,
            SelectManySequence<T, TSource, TSourceEnumerator, TResult, TSelector, TResult, RightSelector<T, TResult>>,
            SelectManySequence<T, TSource, TSourceEnumerator, TResult, TSelector, TResult, RightSelector<T, TResult>>.Enumerator>
        SelectMany<T, TSource, TSourceEnumerator, TResult, TSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, int, NativeArray<TResult>>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct
            where TSelector : struct, IFunc<T, int, NativeArray<TResult>>
        {
            var rightSelector = UtilFunctions.RightSelector<T, TResult>();
            var seq = SelectManySequence<T, TSourceEnumerator>.New(in source.Source, selector, rightSelector);
            return ValueSequence<TResult, SelectManySequence<T, TSource, TSourceEnumerator, TResult, TSelector, TResult, RightSelector<T, TResult>>.Enumerator>.New(in seq);
        }
    }

    public struct SelectManySequence<T, TSource, TSourceEnumerator, TCollection, TCollectionSelector, TResult, TResultSelector>
        : ISequence<TResult, SelectManySequence<T, TSource, TSourceEnumerator, TCollection, TCollectionSelector, TResult, TResultSelector>.Enumerator>
        where T : struct
        where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
        where TCollection : struct
        where TCollectionSelector : struct, IFunc<T, int, NativeArray<TCollection>>
        where TResult : struct
        where TResultSelector : struct, IFunc<T, TCollection, TResult>
    {
        public struct Enumerator : IEnumerator<TResult>
        {
            readonly ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector;
            readonly ValueFunc<T, int, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector;
            TSourceEnumerator sourceEnumerator;
            int currentIndex;
            NativeArray<TCollection> currentCollection;
            NativeArray<TCollection>.Enumerator currentEnumerator;

            public Enumerator(
                in TSource source,
                ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector,
                ValueFunc<T, int, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector
                )
            {
                sourceEnumerator = source.GetEnumerator();
                this.resultSelector = resultSelector;
                this.collectionSelector = collectionSelector;
                currentIndex = -1;
                currentCollection = default;
                currentEnumerator = default;
                Current = default;
            }

            public TResult Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                sourceEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                while (!currentEnumerator.MoveNext())
                {
                    if (!sourceEnumerator.MoveNext())
                        return false;
                    currentIndex += 1;
                    if (currentCollection.IsCreated)
                        currentCollection.Dispose();
                    currentCollection = collectionSelector.Invoke(sourceEnumerator.Current, currentIndex);
                    currentEnumerator = currentCollection.GetEnumerator();
                }
                Current = resultSelector.Invoke(sourceEnumerator.Current, currentEnumerator.Current);
                return true;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        readonly TSource source;
        readonly ValueFunc<T, int, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector;
        readonly ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector;

        public SelectManySequence(
            in TSource source,
            ValueFunc<T, int, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector,
            ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector
            )
        {
            this.source = source;
            this.collectionSelector = collectionSelector;
            this.resultSelector = resultSelector;
        }

        public NativeList<TResult> ToNativeList(Allocator allocator)
        {
            var srcList = source.ToNativeList(Allocator.Temp);
            var resultList = new NativeList<TResult>(allocator);
            for (var i = 0; i < srcList.Length; i++)
            {
                var srcElement = srcList[i];
                var results = GetResults(srcElement, i);
                resultList.AddRange(results);
                results.Dispose();
            }
            srcList.Dispose();
            return resultList;
        }

        private NativeArray<TResult> GetResults(T srcElement, int index)
        {
            var collectionArr = collectionSelector.Invoke(srcElement, index);
            var resultArr = new NativeArray<TResult>(collectionArr.Length, Allocator.Temp);
            for (var i = 0; i < collectionArr.Length; i++)
                resultArr[i] = resultSelector.Invoke(srcElement, collectionArr[i]);
            collectionArr.Dispose();
            return resultArr;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(in source, resultSelector, collectionSelector);
        }

        IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class SelectManySequence<T, TSourceEnumerator>
        where T : struct
        where TSourceEnumerator : struct, IEnumerator<T>
    {
        public static SelectManySequence<T, TSource, TSourceEnumerator, TCollection, TCollectionSelector, TResult, TResultSelector> New<TSource, TCollection, TCollectionSelector, TResult, TResultSelector>(
            in TSource source,
            ValueFunc<T, int, NativeArray<TCollection>>.Struct<TCollectionSelector> collectionSelector,
            ValueFunc<T, TCollection, TResult>.Struct<TResultSelector> resultSelector
            )
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TCollection : struct
            where TCollectionSelector : struct, IFunc<T, int, NativeArray<TCollection>>
            where TResult : struct
            where TResultSelector : struct, IFunc<T, TCollection, TResult>
        {
            return new SelectManySequence<T, TSource, TSourceEnumerator, TCollection, TCollectionSelector, TResult, TResultSelector>(in source, collectionSelector, resultSelector);
        }
    }
}
