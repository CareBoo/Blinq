using System;
using System.Collections.Generic;
using CareBoo.Burst.Delegates;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static FuncJob<ValueSequence<T, TSource, TSourceEnumerator>, TResult, TFunc> NewJob<T, TSource, TSourceEnumerator, TResult, TFunc>(
            this in ValueSequence<T, TSource, TSourceEnumerator> seq,
            ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>.Struct<TFunc> func,
            ref NativeArray<TResult> output
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct
            where TFunc : struct, IFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>
        {
            return new FuncJob<ValueSequence<T, TSource, TSourceEnumerator>, TResult, TFunc>(in seq, func, ref output);
        }

        public static TResult Run<T, TSource, TSourceEnumerator, TResult, TFunc>(
            this in ValueSequence<T, TSource, TSourceEnumerator> seq,
            ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>.Struct<TFunc> func
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct
            where TFunc : struct, IFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>
        {
            var output = new NativeArray<TResult>(1, Allocator.Persistent);
            NewJob(in seq, func, ref output).Run();
            var result = output[0];
            output.Dispose();
            return result;
        }

        public static JobHandle Schedule<T, TSource, TSourceEnumerator, TResult, TFunc>(
            this in ValueSequence<T, TSource, TSourceEnumerator> seq,
            ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>.Struct<TFunc> func,
            ref NativeArray<TResult> output
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct
            where TFunc : struct, IFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>
        {
            return NewJob(in seq, func, ref output).Schedule();
        }

        public static JobHandle<TResult> Schedule<T, TSource, TSourceEnumerator, TResult, TFunc>(
            this in ValueSequence<T, TSource, TSourceEnumerator> seq,
            ValueFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>.Struct<TFunc> func
            )
            where T : struct
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
            where TResult : struct
            where TFunc : struct, IFunc<ValueSequence<T, TSource, TSourceEnumerator>, TResult>
        {
            var output = new NativeArray<TResult>(1, Allocator.Persistent);
            var jobHandle = NewJob(in seq, func, ref output).Schedule();
            return new JobHandle<TResult>(in jobHandle, ref output);
        }
    }
}
