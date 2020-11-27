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
            SelectSequence<T, TSource, TSourceEnumerator, TResult, TSelector>,
            SelectSequence<T, TSource, TSourceEnumerator, TResult, TSelector>.Enumerator>
        Select<T, TSource, TSourceEnumerator, TResult, TSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, int, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct
            where TSelector : struct, IFunc<T, int, TResult>
        {
            var sourceSeq = source.Source;
            var seq = new SelectSequence<T, TSource, TSourceEnumerator, TResult, TSelector>(in sourceSeq, selector);
            return ValueSequence<TResult, SelectSequence<T, TSource, TSourceEnumerator, TResult, TSelector>.Enumerator>.New(in seq);
        }

        public static ValueSequence<
            TResult,
            SelectSequence<T, TSource, TSourceEnumerator, TResult, IgnoreIndex<T, TResult, TSelector>>,
            SelectSequence<T, TSource, TSourceEnumerator, TResult, IgnoreIndex<T, TResult, TSelector>>.Enumerator>
        Select<T, TSource, TSourceEnumerator, TResult, TSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct
            where TSelector : struct, IFunc<T, TResult>
        {
            var indexSelector = UtilFunctions.IgnoreIndex(selector);
            return source.Select(indexSelector);
        }
    }

    public struct SelectSequence<T, TSource, TSourceEnumerator, TResult, TSelector>
        : ISequence<TResult, SelectSequence<T, TSource, TSourceEnumerator, TResult, TSelector>.Enumerator>
        where T : struct
        where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
        where TResult : struct
        where TSelector : struct, IFunc<T, int, TResult>
    {
        public struct Enumerator : IEnumerator<TResult>
        {
            readonly ValueFunc<T, int, TResult>.Struct<TSelector> selector;
            TSourceEnumerator sourceEnumerator;

            int currentIndex;

            public Enumerator(
                in TSource source,
                ValueFunc<T, int, TResult>.Struct<TSelector> selector
                )
            {
                sourceEnumerator = source.GetEnumerator();
                this.selector = selector;
                currentIndex = -1;
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
                if (sourceEnumerator.MoveNext())
                {
                    currentIndex += 1;
                    Current = selector.Invoke(sourceEnumerator.Current, currentIndex);
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        readonly TSource source;

        readonly ValueFunc<T, int, TResult>.Struct<TSelector> selector;

        public SelectSequence(
            in TSource source,
            ValueFunc<T, int, TResult>.Struct<TSelector> selector
            )
        {
            this.source = source;
            this.selector = selector;
        }

        public NativeList<TResult> ToNativeList(Allocator allocator)
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            var newList = new NativeList<TResult>(sourceList.Length, allocator);
            for (var i = 0; i < sourceList.Length; i++)
                newList.AddNoResize(selector.Invoke(sourceList[i], i));
            sourceList.Dispose();
            return newList;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(in source, selector);
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
}
