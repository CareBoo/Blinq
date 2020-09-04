using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static TResult Aggregate<T, TAccumulate, TResult, TFunc, TResultSelector>(
            this ref NativeArray<T> source,
            TAccumulate seed,
            TFunc func = default,
            TResultSelector resultSelector = default
            )
                where T : struct
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IValueFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IValueFunc<TAccumulate, TResult>
        {
            for (var i = 0; i < source.Length; i++)
                seed = func.Invoke(seed, source[i]);
            return resultSelector.Invoke(seed);
        }

        public static TAccumulate Aggregate<T, TAccumulate, TFunc>(
            this ref NativeArray<T> source,
            TAccumulate seed,
            TFunc func = default
            )
            where T : struct
            where TAccumulate : struct
            where TFunc : struct, IValueFunc<TAccumulate, T, TAccumulate>
        {
            for (var i = 0; i < source.Length; i++)
                seed = func.Invoke(seed, source[i]);
            return seed;
        }

        public static T Aggregate<T, TFunc>(this ref NativeArray<T> source, TFunc func = default)
            where T : struct
            where TFunc : struct, IValueFunc<T, T, T>
        {
            if (source.Length == 0) throw Error.NoElements();
            var seed = source[0];
            for (var i = 1; i < source.Length; i++)
                seed = func.Invoke(seed, source[i]);
            return seed;
        }
    }
}
