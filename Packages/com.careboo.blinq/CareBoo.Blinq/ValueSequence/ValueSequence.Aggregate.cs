namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        public TResult Aggregate<TAccumulate, TResult, TFunc, TResultSelector>(
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Reference<TFunc> func,
            ValueFunc<TAccumulate, TResult>.Reference<TResultSelector> resultSelector
            )
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

        public TAccumulate Aggregate<TAccumulate, TFunc>(
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Reference<TFunc> func
            )
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                seed = func.Invoke(seed, sourceList[i]);
            return seed;
        }

        public T Aggregate<TFunc>(ValueFunc<T, T, T>.Reference<TFunc> func)
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
