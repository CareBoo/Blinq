using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class ValueSequenceExtensions
    {
        public static ValueSequence<TResult, SelectIndexSequence<T, TSource, TResult, TSelector>> Select<T, TSource, TResult, TSelector>(
            this ref ValueSequence<T, TSource> source,
            ValueFunc<T, int, TResult>.Impl<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TResult : struct
            where TSelector : struct, IFunc<T, int, TResult>
        {
            var seq = new SelectIndexSequence<T, TSource, TResult, TSelector> { Source = source.Source, Selector = selector };
            return new ValueSequence<TResult, SelectIndexSequence<T, TSource, TResult, TSelector>>(seq);
        }

        public static ValueSequence<TResult, SelectSequence<T, TSource, TResult, TSelector>> Select<T, TSource, TResult, TSelector>(
            this ref ValueSequence<T, TSource> source,
            ValueFunc<T, TResult>.Impl<TSelector> selector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TResult : struct
            where TSelector : struct, IFunc<T, TResult>
        {
            var seq = new SelectSequence<T, TSource, TResult, TSelector> { Source = source.Source, Selector = selector };
            return new ValueSequence<TResult, SelectSequence<T, TSource, TResult, TSelector>>(seq);
        }
    }

    public struct SelectIndexSequence<T, TSource, TResult, TSelector> : ISequence<TResult>
        where T : struct
        where TSource : struct, ISequence<T>
        where TResult : struct
        where TSelector : struct, IFunc<T, int, TResult>
    {
        public TSource Source;
        public ValueFunc<T, int, TResult>.Impl<TSelector> Selector;

        public NativeList<TResult> Execute()
        {
            var sourceList = Source.Execute();

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
        public ValueFunc<T, TResult>.Impl<TSelector> Selector;

        public NativeList<TResult> Execute()
        {
            var sourceList = Source.Execute();

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
