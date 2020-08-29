using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        public TResult Aggregate<TAccumulate, TResult>(TAccumulate seed, Func<TAccumulate, T, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
            where TAccumulate : struct
            where TResult : struct
        {
            throw Error.NotCodeGenerated();
        }

        public TAccumulate Aggregate<TAccumulate>(TAccumulate seed, Func<TAccumulate, T, TAccumulate> func)
            where TAccumulate : struct
        {
            throw Error.NotCodeGenerated();
        }

        public T Aggregate(Func<T, T, T> func)
        {
            throw Error.NotCodeGenerated();
        }

        public TResult Aggregate<TAccumulate, TResult>(TAccumulate seed, BurstCompiledFunc<TAccumulate, T, TAccumulate> func, BurstCompiledFunc<TAccumulate, TResult> resultSelector)
            where TAccumulate : struct
            where TResult : struct
        {
            return Aggregate_CodeGeneratedFunc<
                TAccumulate,
                TResult,
                BurstCompiledFunc<TAccumulate, T, TAccumulate>,
                BurstCompiledFunc<TAccumulate, TResult>>
                (seed, func, resultSelector);
        }

        public TAccumulate Aggregate<TAccumulate>(TAccumulate seed, BurstCompiledFunc<TAccumulate, T, TAccumulate> func)
            where TAccumulate : struct
        {
            return Aggregate_CodeGeneratedFunc(seed, func);
        }

        public T Aggregate(BurstCompiledFunc<T, T, T> func)
        {
            return Aggregate(default, func);
        }

        public TResult Aggregate_CodeGeneratedFunc<TAccumulate, TResult, TFunc, TResultSelector>(
            TAccumulate seed,
            TFunc func,
            TResultSelector resultSelector
            )
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            var output = new NativeArray<TResult>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            var job = new AggregateJob<TAccumulate, TResult, TFunc, TResultSelector> { Input = input, Seed = seed, Func = func, ResultSelector = resultSelector, Output = output };
            job.Schedule(dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }

        public TAccumulate Aggregate_CodeGeneratedFunc<TAccumulate, TFunc>(
            TAccumulate seed,
            TFunc func
            )
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            var output = new NativeArray<TAccumulate>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            var job = new AggregateJob<TAccumulate, TFunc> { Input = input, Seed = seed, Func = func, Output = output };
            job.Schedule(dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct AggregateJob<TAccumulate, TFunc> : IJob
            where TAccumulate : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<T> Input;

            public TAccumulate Seed;

            [ReadOnly]
            public TFunc Func;

            [WriteOnly]
            public NativeArray<TAccumulate> Output;

            public void Execute()
            {
                for (var i = 0; i < Input.Length; i++)
                    Seed = Func.Invoke(Seed, Input[i]);
                Output[0] = Seed;
            }
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct AggregateJob<TAccumulate, TResult, TFunc, TResultSelector> : IJob
            where TAccumulate : struct
            where TResult : struct
            where TFunc : struct, IFunc<TAccumulate, T, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            [ReadOnly]
            public NativeArray<T> Input;

            public TAccumulate Seed;

            [ReadOnly]
            public TFunc Func;

            [ReadOnly]
            public TResultSelector ResultSelector;

            [WriteOnly]
            public NativeArray<TResult> Output;

            public void Execute()
            {
                for (var i = 0; i < Input.Length; i++)
                    Seed = Func.Invoke(Seed, Input[i]);
                Output[0] = ResultSelector.Invoke(Seed);
            }
        }
    }
}
