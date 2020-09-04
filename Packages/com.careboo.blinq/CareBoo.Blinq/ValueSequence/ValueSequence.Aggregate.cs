using System.Collections.Generic;

namespace CareBoo.Blinq
{
    public partial struct ValueSequence<T, TQuery>
        : IEnumerable<T>
        where T : struct
        where TQuery : struct, IQuery<T>
    {
        public TResult Aggregate<TAccumulate, TResult, TFunc, TResultSelector>(
            TAccumulate seed,
            TFunc func = default,
            TResultSelector resultSelector = default
            )
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IValueFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IValueFunc<TAccumulate, TResult>
        {
            var sourceList = query.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                seed = func.Invoke(seed, sourceList[i]);
            return resultSelector.Invoke(seed);
        }

        public TAccumulate Aggregate<TAccumulate, TFunc>(TAccumulate seed, TFunc func = default)
            where TAccumulate : struct
            where TFunc : struct, IValueFunc<TAccumulate, T, TAccumulate>
        {
            var sourceList = query.Execute();
            for (var i = 0; i < sourceList.Length; i++)
                seed = func.Invoke(seed, sourceList[i]);
            return seed;
        }

        public T Aggregate<TFunc>(TFunc func = default)
            where TFunc : struct, IValueFunc<T, T, T>
        {
            var sourceList = query.Execute();
            if (sourceList.Length == 0) throw Error.NoElements();
            var seed = sourceList[0];
            for (var i = 1; i < sourceList.Length; i++)
                seed = func.Invoke(seed, sourceList[i]);
            return seed;
        }
    }
}
