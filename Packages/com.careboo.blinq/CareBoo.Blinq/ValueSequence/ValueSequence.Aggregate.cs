using System.Collections.Generic;
using CareBoo.Burst.Delegates;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static TResult Aggregate<T, TSource, TSourceEnumerator, TAccumulate, TResult, TFunc, TResultSelector>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func,
            ValueFunc<TAccumulate, TResult>.Struct<TResultSelector> resultSelector
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            for (var i = 0; i < sourceList.Length; i++)
                seed = func.Invoke(seed, sourceList[i]);
            var result = resultSelector.Invoke(seed);
            sourceList.Dispose();
            return result;
        }

        public static TAccumulate Aggregate<T, TSource, TSourceEnumerator, TAccumulate, TFunc>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            TAccumulate seed,
            ValueFunc<TAccumulate, T, TAccumulate>.Struct<TFunc> func
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            for (var i = 0; i < sourceList.Length; i++)
                seed = func.Invoke(seed, sourceList[i]);
            sourceList.Dispose();
            return seed;
        }

        public static T Aggregate<T, TSource, TSourceEnumerator, TFunc>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ValueFunc<T, T, T>.Struct<TFunc> func
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TFunc : struct, IFunc<T, T, T>
        {
            var sourceList = source.ToNativeList(Allocator.Temp);
            if (sourceList.Length == 0)
            {
                sourceList.Dispose();
                throw Error.NoElements();
            }
            var seed = sourceList[0];
            for (var i = 1; i < sourceList.Length; i++)
                seed = func.Invoke(seed, sourceList[i]);
            sourceList.Dispose();
            return seed;
        }
    }
}
