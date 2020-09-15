namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static TResult Aggregate<T, TSource, TAccumulate, TResult, TFunc, TResultSelector>(
            this ValueSequence<T, TSource> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Impl<TFunc> func,
            ValueFunc<TAccumulate, TResult>.Impl<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                seed = func.Invoke(seed, sourceList[i]);
            return resultSelector.Invoke(seed);
        }

        public static TAccumulate Aggregate<T, TSource, TAccumulate, TFunc>(
            this ValueSequence<T, TSource> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Impl<TFunc> func
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                seed = func.Invoke(seed, sourceList[i]);
            return seed;
        }

        public static T Aggregate<T, TSource, TFunc>(
            this ValueSequence<T, TSource> source,
            ValueFunc<T, T, T>.Impl<TFunc> func
            )
            where T : struct
            where TSource : struct, ISequence<T>
            where TFunc : struct, IFunc<T, T, T>
        {
            var sourceList = source.Execute();
            if (sourceList.Length == 0) throw Error.NoElements();
            var seed = sourceList[0];
            for (var i = 1; i < sourceList.Length; i++)
                seed = func.Invoke(seed, sourceList[i]);
            return seed;
        }
    }
}
