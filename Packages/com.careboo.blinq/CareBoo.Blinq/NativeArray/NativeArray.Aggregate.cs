using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static TResult Aggregate<T, TAccumulate, TResult, TFunc, TResultSelector>(
            this ref NativeArray<T> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Impl<TFunc> func,
            ValueFunc<TAccumulate, TResult>.Impl<TResultSelector> resultSelector
            )
            where T : struct
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            for (var i = 0; i < source.Length; i++)
                seed = func.Invoke(seed, source[i]);
            return resultSelector.Invoke(seed);
        }

        public static TAccumulate Aggregate<T, TAccumulate, TFunc>(
            this ref NativeArray<T> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Impl<TFunc> func
            )
            where T : struct
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            for (var i = 0; i < source.Length; i++)
                seed = func.Invoke(seed, source[i]);
            return seed;
        }

        public static T Aggregate<T, TFunc>(
            this ref NativeArray<T> source,
            ValueFunc<T, T, T>.Impl<TFunc> func
            )
            where T : struct
            where TFunc : struct, IFunc<T, T, T>
        {
            if (source.Length == 0) throw Error.NoElements();
            var seed = source[0];
            for (var i = 1; i < source.Length; i++)
                seed = func.Invoke(seed, source[i]);
            return seed;
        }
    }
}
