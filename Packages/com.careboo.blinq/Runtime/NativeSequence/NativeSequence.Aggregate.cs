using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        public TResult Aggregate<TAccumulate, TResult>(TAccumulate seed, BurstCompiledFunc<TAccumulate, T, TAccumulate> func, BurstCompiledFunc<TAccumulate, TResult> resultSelector)
            where TAccumulate : struct
            where TResult : struct
        {
            var output = new NativeArray<TResult>(1, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            var job = new AggregateJob<TAccumulate, TResult> { Input = input, Seed = seed, Func = func, ResultSelector = resultSelector, Output = output };
            job.Schedule(dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }

        public TAccumulate Aggregate<TAccumulate>(TAccumulate seed, BurstCompiledFunc<TAccumulate, T, TAccumulate> func)
            where TAccumulate : struct
        {
            var output = new NativeArray<TAccumulate>(1, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            var job = new AggregateJob<TAccumulate> { Input = input, Seed = seed, Func = func };
            job.Schedule(dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }

        public T Aggregate(BurstCompiledFunc<T, T, T> func)
        {
            return Aggregate(default, func);
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct AggregateJob<TAccumulate> : IJob
            where TAccumulate : struct
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<T> Input;

            public TAccumulate Seed;

            [ReadOnly]
            public BurstCompiledFunc<TAccumulate, T, TAccumulate> Func;

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
        public struct AggregateJob<TAccumulate, TResult> : IJob
            where TAccumulate : struct
            where TResult : struct
        {
            [ReadOnly]
            public NativeArray<T> Input;

            public TAccumulate Seed;

            [ReadOnly]
            public BurstCompiledFunc<TAccumulate, T, TAccumulate> Func;

            [ReadOnly]
            public BurstCompiledFunc<TAccumulate, TResult> ResultSelector;

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
