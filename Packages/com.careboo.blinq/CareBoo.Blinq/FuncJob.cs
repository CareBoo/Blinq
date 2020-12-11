using System;
using CareBoo.Burst.Delegates;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    [BurstCompile]
    public struct FuncJob<T, TResult, TFunc> : IJob
        where T : struct
        where TResult : struct
        where TFunc : struct, IFunc<T, TResult>
    {
        [ReadOnly]
        readonly T arg1;

        [ReadOnly]
        readonly ValueFunc<T, TResult>.Struct<TFunc> func;

        [WriteOnly]
        NativeArray<TResult> output;

        internal FuncJob(
            in T arg1,
            ValueFunc<T, TResult>.Struct<TFunc> func,
            ref NativeArray<TResult> output
            )
        {
            this.arg1 = arg1;
            this.func = func;
            this.output = output;
        }

        public void Execute()
        {
            output[0] = func.Invoke(arg1);
        }
    }

    public struct JobHandle<TResult> : IDisposable
        where TResult : struct
    {
        JobHandle jobHandle;
        NativeArray<TResult> output;

        public bool IsCompleted => jobHandle.IsCompleted;

        internal JobHandle(in JobHandle jobHandle, ref NativeArray<TResult> output)
        {
            this.jobHandle = jobHandle;
            this.output = output;
        }

        public TResult Complete()
        {
            jobHandle.Complete();
            var result = output[0];
            Dispose();
            return result;
        }

        public void Dispose()
        {
            if (output.IsCreated)
                output.Dispose();
        }
    }
}
