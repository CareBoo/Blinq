using Unity.Collections;
using CareBoo.Burst.Delegates;
using System.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<TResult, SelectIndexSequence<T, TSource, TResult, TSelector>> Select<T, TSource, TResult, TSelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, int, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TResult : struct
            where TSelector : struct, IFunc<T, int, TResult>
        {
            var seq = new SelectIndexSequence<T, TSource, TResult, TSelector> { Source = source.Source, Selector = selector };
            return ValueSequence<TResult>.New(seq);
        }

        public static ValueSequence<TResult, SelectSequence<T, TSource, TResult, TSelector>> Select<T, TSource, TResult, TSelector>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, TResult>.Struct<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TResult : struct
            where TSelector : struct, IFunc<T, TResult>
        {
            var seq = new SelectSequence<T, TSource, TResult, TSelector> { Source = source.Source, Selector = selector };
            return ValueSequence<TResult>.New(seq);
        }
    }

    public struct SelectIndexSequence<T, TSource, TResult, TSelector> : ISequence<TResult>
        where T : struct
        where TSource : struct, ISequence<T>
        where TResult : struct
        where TSelector : struct, IFunc<T, int, TResult>
    {
        public TSource Source;
        public ValueFunc<T, int, TResult>.Struct<TSelector> Selector;

        private int currentIndex;

        public TResult Current => Selector.Invoke(Source.Current, currentIndex - 1);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            Source.Dispose();
        }

        public bool MoveNext()
        {
            currentIndex += 1;
            return Source.MoveNext();
        }

        public void Reset()
        {
            currentIndex = 0;
            Source.Reset();
        }

        public NativeList<TResult> ToList()
        {
            var sourceList = Source.ToList();

            var newList = new NativeList<TResult>(sourceList.Length, Allocator.Temp);
            for (var i = 0; i < sourceList.Length; i++)
            {
                newList.AddNoResize(Selector.Invoke(sourceList[i], i));
            }
            sourceList.Dispose();
            return newList;
        }
    }

    public struct SelectSequence<T, TSource, TResult, TSelector> : ISequence<TResult>
        where T : struct
        where TSource : struct, ISequence<T>
        where TResult : struct
        where TSelector : struct, IFunc<T, TResult>
    {
        public TSource Source;
        public ValueFunc<T, TResult>.Struct<TSelector> Selector;

        public TResult Current => Selector.Invoke(Source.Current);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            Source.Dispose();
        }

        public bool MoveNext()
        {
            return Source.MoveNext();
        }

        public void Reset()
        {
            Source.Reset();
        }

        public NativeList<TResult> ToList()
        {
            var sourceList = Source.ToList();

            var newList = new NativeList<TResult>(sourceList.Length, Allocator.Temp);
            for (var i = 0; i < sourceList.Length; i++)
            {
                newList.AddNoResize(Selector.Invoke(sourceList[i]));
            }
            sourceList.Dispose();
            return newList;
        }
    }
}
