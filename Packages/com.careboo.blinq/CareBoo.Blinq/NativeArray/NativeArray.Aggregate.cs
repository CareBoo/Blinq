using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static TResult Aggregate<T, TAccumulate, TResult>(
            this ref NativeArray<T> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate> func,
            ValueFunc<TAccumulate, TResult> resultSelector
            )
            where T : struct
            where TAccumulate : struct
            where TResult : struct
        {
            for (var i = 0; i < source.Length; i++)
                seed = func.Invoke(seed, source[i]);
            return resultSelector.Invoke(seed);
        }

        public static TAccumulate Aggregate<T, TAccumulate>(
            this ref NativeArray<T> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate> func
            )
            where T : struct
            where TAccumulate : struct
        {
            for (var i = 0; i < source.Length; i++)
                seed = func.Invoke(seed, source[i]);
            return seed;
        }

        public static T Aggregate<T>(this ref NativeArray<T> source, ValueFunc<T, T, T> func)
            where T : struct
        {
            if (source.Length == 0) throw Error.NoElements();
            var seed = source[0];
            for (var i = 1; i < source.Length; i++)
                seed = func.Invoke(seed, source[i]);
            return seed;
        }
    }
}
