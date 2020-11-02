using Unity.Collections;
using Unity.Mathematics;
using CareBoo.Burst.Delegates;
using System.Collections;
using System;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, ZipSequence<T, TSource, TSecondElement, TSecond, TResult, TResultSelector>> Zip<T, TSource, TSecondElement, TResult, TSecond, TResultSelector>(
            this ValueSequence<T, TSource> source,
            ValueSequence<TSecondElement, TSecond> second,
            ValueFunc<T, TSecondElement, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TSecondElement : struct
            where TResult : struct
            where TSecond : struct, ISequence<TSecondElement>
            where TResultSelector : struct, IFunc<T, TSecondElement, TResult>
        {
            var seq = ZipSequence<T, TSecondElement>.New(source.Source, second.Source, resultSelector);
            return ValueSequence<TResult>.New(seq);
        }
    }

    public struct ZipSequence<T, TSource, TSecondElement, TSecond, TResult, TResultSelector> : ISequence<TResult>
        where T : struct
        where TSource : struct, ISequence<T>
        where TSecondElement : struct
        where TSecond : struct, ISequence<TSecondElement>
        where TResult : struct
        where TResultSelector : struct, IFunc<T, TSecondElement, TResult>
    {
        readonly TSource source;
        readonly TSecond second;
        readonly ValueFunc<T, TSecondElement, TResult>.Struct<TResultSelector> resultSelector;

        public ZipSequence(TSource source, TSecond second, ValueFunc<T, TSecondElement, TResult>.Struct<TResultSelector> resultSelector)
        {
            this.source = source;
            this.second = second;
            this.resultSelector = resultSelector;
        }

        public TResult Current => resultSelector.Invoke(source.Current, second.Current);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            source.Dispose();
            second.Dispose();
        }

        public bool MoveNext()
        {
            return source.MoveNext() && second.MoveNext();
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<TResult> ToList()
        {
            using (var sourceList = source.ToList())
            using (var secondList = second.ToList())
            {
                var length = math.min(sourceList.Length, secondList.Length);
                var result = new NativeList<TResult>(length, Allocator.Temp);
                for (var i = 0; i < length; i++)
                    result.AddNoResize(resultSelector.Invoke(sourceList[i], secondList[i]));
                return result;
            }
        }
    }

    public static class ZipSequence<T, TSecondElement>
        where T : struct
        where TSecondElement : struct
    {
        public static ZipSequence<T, TSource, TSecondElement, TSecond, TResult, TResultSelector> New<TSource, TSecond, TResult, TResultSelector>(
            TSource source,
            TSecond second,
            ValueFunc<T, TSecondElement, TResult>.Struct<TResultSelector> resultSelector
            )
            where TSource : struct, ISequence<T>
            where TResult : struct
            where TSecond : struct, ISequence<TSecondElement>
            where TResultSelector : struct, IFunc<T, TSecondElement, TResult>
        {
            return new ZipSequence<T, TSource, TSecondElement, TSecond, TResult, TResultSelector>(source, second, resultSelector);
        }
    }
}
