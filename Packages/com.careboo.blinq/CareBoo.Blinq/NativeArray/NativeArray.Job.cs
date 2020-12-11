using CareBoo.Burst.Delegates;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static FuncJob<NativeArray<T>, TResult, TFunc> NewJob<T, TResult, TFunc>(
            this in NativeArray<T> source,
            ValueFunc<NativeArray<T>, TResult>.Struct<TFunc> func,
            ref NativeArray<TResult> output
            )
            where T : struct
            where TResult : struct
            where TFunc : struct, IFunc<NativeArray<T>, TResult>
        {
            return new FuncJob<NativeArray<T>, TResult, TFunc>(in source, func, ref output);
        }

        public static TResult Run<T, TResult, TFunc>(
            this in NativeArray<T> source,
            ValueFunc<NativeArray<T>, TResult>.Struct<TFunc> func
            )
            where T : struct
            where TResult : struct
            where TFunc : struct, IFunc<NativeArray<T>, TResult>
        {
            var output = new NativeArray<TResult>(1, Allocator.Persistent);
            NewJob(in source, func, ref output).Run();
            var result = output[0];
            output.Dispose();
            return result;
        }

        public static JobHandle Schedule<T, TResult, TFunc>(
            this in NativeArray<T> source,
            ValueFunc<NativeArray<T>, TResult>.Struct<TFunc> func,
            ref NativeArray<TResult> output
            )
            where T : struct
            where TResult : struct
            where TFunc : struct, IFunc<NativeArray<T>, TResult>
        {
            return NewJob(in source, func, ref output).Schedule();
        }

        public static JobHandle<TResult> Schedule<T, TResult, TFunc>(
            this in NativeArray<T> source,
            ValueFunc<NativeArray<T>, TResult>.Struct<TFunc> func
            )
            where T : struct
            where TResult : struct
            where TFunc : struct, IFunc<NativeArray<T>, TResult>
        {
            var output = new NativeArray<TResult>(1, Allocator.Persistent);
            var jobHandle = NewJob(in source, func, ref output).Schedule();
            return new JobHandle<TResult>(in jobHandle, ref output);
        }
    }
}
