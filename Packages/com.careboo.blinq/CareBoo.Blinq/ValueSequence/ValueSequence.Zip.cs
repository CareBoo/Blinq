using Unity.Collections;
using Unity.Mathematics;
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
            ZipSequence<T, TSource, TSourceEnumerator, TSecondElement, TSecond, TSecondEnumerator, TResult, TResultSelector>,
            ZipSequence<T, TSource, TSourceEnumerator, TSecondElement, TSecond, TSecondEnumerator, TResult, TResultSelector>.Enumerator>
        Zip<T, TSource, TSourceEnumerator, TSecondElement, TSecond, TSecondEnumerator, TResult, TResultSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            in ValueSequence<TSecondElement, TSecond, TSecondEnumerator> second,
            ValueFunc<T, TSecondElement, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TSecondElement : struct
            where TSecond : struct, ISequence<TSecondElement, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<TSecondElement>
            where TResult : struct
            where TResultSelector : struct, IFunc<T, TSecondElement, TResult>
        {
            var sourceSeq = source.Source;
            var secondSeq = second.Source;
            var seq = new ZipSequence<T, TSource, TSourceEnumerator, TSecondElement, TSecond, TSecondEnumerator, TResult, TResultSelector>(in sourceSeq, in secondSeq, resultSelector);
            return ValueSequence<TResult, ZipSequence<T, TSource, TSourceEnumerator, TSecondElement, TSecond, TSecondEnumerator, TResult, TResultSelector>.Enumerator>.New(in seq);
        }
    }

    public struct ZipSequence<T, TSource, TSourceEnumerator, TSecondElement, TSecond, TSecondEnumerator, TResult, TResultSelector>
        : ISequence<TResult, ZipSequence<T, TSource, TSourceEnumerator, TSecondElement, TSecond, TSecondEnumerator, TResult, TResultSelector>.Enumerator>
        where T : struct
        where TSource : struct, ISequence<T, TSourceEnumerator>
        where TSourceEnumerator : struct, IEnumerator<T>
        where TSecondElement : struct
        where TSecond : struct, ISequence<TSecondElement, TSecondEnumerator>
        where TSecondEnumerator : struct, IEnumerator<TSecondElement>
        where TResult : struct
        where TResultSelector : struct, IFunc<T, TSecondElement, TResult>
    {
        public struct Enumerator : IEnumerator<TResult>
        {
            readonly ValueFunc<T, TSecondElement, TResult>.Struct<TResultSelector> resultSelector;
            TSourceEnumerator sourceEnumerator;
            TSecondEnumerator secondEnumerator;

            public Enumerator(
                in TSource source,
                in TSecond second,
                ValueFunc<T, TSecondElement, TResult>.Struct<TResultSelector> resultSelector
                )
            {
                this.resultSelector = resultSelector;
                sourceEnumerator = source.GetEnumerator();
                secondEnumerator = second.GetEnumerator();
                Current = default;
            }

            public TResult Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                sourceEnumerator.Dispose();
                secondEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                if (sourceEnumerator.MoveNext() && secondEnumerator.MoveNext())
                {
                    Current = resultSelector.Invoke(sourceEnumerator.Current, secondEnumerator.Current);
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        TSource source;

        TSecond second;

        readonly ValueFunc<T, TSecondElement, TResult>.Struct<TResultSelector> resultSelector;

        public ZipSequence(
            in TSource source,
            in TSecond second,
            ValueFunc<T, TSecondElement, TResult>.Struct<TResultSelector> resultSelector
            )
        {
            this.source = source;
            this.second = second;
            this.resultSelector = resultSelector;
        }

        public NativeList<TResult> ToNativeList(Allocator allocator)
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            var secondList = second.ToNativeList(Allocator.Temp);
            var length = math.min(sourceList.Length, secondList.Length);
            var result = new NativeList<TResult>(length, allocator);
            for (var i = 0; i < length; i++)
                result.AddNoResize(resultSelector.Invoke(sourceList[i], secondList[i]));
            sourceList.Dispose();
            secondList.Dispose();
            return result;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(in source, in second, resultSelector);
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
