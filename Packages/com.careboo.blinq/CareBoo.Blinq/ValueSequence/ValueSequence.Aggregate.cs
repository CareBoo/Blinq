namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TSource>
    {
        public TResult Aggregate<TAccumulate, TResult>(
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate> func,
            ValueFunc<TAccumulate, TResult> resultSelector
            )
            where TAccumulate : struct
            where TResult : struct
        {
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                seed = func.Invoke(seed, sourceList[i]);
            return resultSelector.Invoke(seed);
        }

        public TAccumulate Aggregate<TAccumulate>(
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate> func
            )
            where TAccumulate : struct
        {
            var sourceList = source.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                seed = func.Invoke(seed, sourceList[i]);
            return seed;
        }

        public T Aggregate(ValueFunc<T, T, T> func)
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
