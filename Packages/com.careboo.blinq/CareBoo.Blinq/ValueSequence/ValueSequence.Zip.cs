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
            this in ValueSequence<T, TSource> source,
            in ValueSequence<TSecondElement, TSecond> second,
            in ValueFunc<T, TSecondElement, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TSecondElement : struct
            where TResult : struct
            where TSecond : struct, ISequence<TSecondElement>
            where TResultSelector : struct, IFunc<T, TSecondElement, TResult>
        {
            var sourceSeq = source.GetEnumerator();
            var secondSeq = second.GetEnumerator();
            var seq = ZipSequence<T, TSecondElement>.New(ref sourceSeq, ref secondSeq, in resultSelector);
            return ValueSequence<TResult>.New(ref seq);
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
        TSource source;
        TSecond second;
        readonly ValueFunc<T, TSecondElement, TResult>.Struct<TResultSelector> resultSelector;

        public ZipSequence(
            ref TSource source,
            ref TSecond second,
            in ValueFunc<T, TSecondElement, TResult>.Struct<TResultSelector> resultSelector
            )
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
    }

    public static class ZipSequence<T, TSecondElement>
        where T : struct
        where TSecondElement : struct
    {
        public static ZipSequence<T, TSource, TSecondElement, TSecond, TResult, TResultSelector> New<TSource, TSecond, TResult, TResultSelector>(
            ref TSource source,
            ref TSecond second,
            in ValueFunc<T, TSecondElement, TResult>.Struct<TResultSelector> resultSelector
            )
            where TSource : struct, ISequence<T>
            where TResult : struct
            where TSecond : struct, ISequence<TSecondElement>
            where TResultSelector : struct, IFunc<T, TSecondElement, TResult>
        {
            return new ZipSequence<T, TSource, TSecondElement, TSecond, TResult, TResultSelector>(ref source, ref second, in resultSelector);
        }
    }
}
