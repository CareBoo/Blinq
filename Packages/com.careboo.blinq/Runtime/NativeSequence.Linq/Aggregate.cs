using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        public TResult Aggregate<TAccumulate, TResult, TAccumulator, TResultSelector>(TAccumulate seed = default, TAccumulator accumulator = default, TResultSelector resultSelector = default)
            where TAccumulate : struct
            where TResult : struct
            where TAccumulator : struct, IFunc<T, TAccumulate, TAccumulate>
            where TResultSelector : struct, IFunc<TAccumulate, TResult>
        {
            var output = new NativeArray<TResult>(1, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            var job = new AggregateJob<TAccumulate, TResult, TAccumulator, TResultSelector> { Input = input, Seed = seed, Accumulator = accumulator, ResultSelector = resultSelector, Output = output };
            job.Schedule(dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }

        public TResult Aggregate<TResult, TAccumulator, TResultSelector>(T seed = default, TAccumulator accumulator = default, TResultSelector resultSelector = default)
            where TResult : struct
            where TAccumulator : struct, IFunc<T, T, T>
            where TResultSelector : struct, IFunc<T, TResult>
        {
            return Aggregate<T, TResult, TAccumulator, TResultSelector>(seed, accumulator, resultSelector);
        }

        public TAccumulate Aggregate<TAccumulate, TAccumulator>(TAccumulate seed = default, TAccumulator accumulator = default)
            where TAccumulate : struct
            where TAccumulator : struct, IFunc<T, TAccumulate, TAccumulate>
        {
            var output = new NativeArray<TAccumulate>(1, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            var job = new AggregateJob<TAccumulate, TAccumulator> { Input = input, Seed = seed, Accumulator = accumulator };
            job.Schedule(dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }

        public T Aggregate<TAccumulator>(T seed = default, TAccumulator accumulator = default)
            where TAccumulator : struct, IFunc<T, T, T>
        {
            return Aggregate<T, TAccumulator>(seed, accumulator);
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct AggregateJob<TAccumulate, TAccumulator> : IJob
            where TAccumulate : struct
            where TAccumulator : struct, IFunc<T, TAccumulate, TAccumulate>
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<T> Input;

            public TAccumulate Seed;

            [ReadOnly]
            public TAccumulator Accumulator;

            [WriteOnly]
            public NativeArray<TAccumulate> Output;

            public void Execute()
            {
                for (var i = 0; i < Input.Length; i++)
                    Seed = Accumulator.Invoke(Input[i], Seed);
                Output[0] = Seed;
            }
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct AggregateJob<TAccumulate, TResult, TAccumulator, TResultSelector> : IJob
            where TAccumulate : struct
            where TResult : struct
            where TAccumulator : IFunc<T, TAccumulate, TAccumulate>
            where TResultSelector : IFunc<TAccumulate, TResult>
        {
            [ReadOnly]
            public NativeArray<T> Input;

            public TAccumulate Seed;

            [ReadOnly]
            public TAccumulator Accumulator;

            [ReadOnly]
            public TResultSelector ResultSelector;

            [WriteOnly]
            public NativeArray<TResult> Output;

            public void Execute()
            {
                for (var i = 0; i < Input.Length; i++)
                    Seed = Accumulator.Invoke(Input[i], Seed);
                Output[0] = ResultSelector.Invoke(Seed);
            }
        }
    }
}
