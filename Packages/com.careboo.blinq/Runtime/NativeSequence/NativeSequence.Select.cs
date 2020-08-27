using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        public NativeSequence<TResult> Select<TResult>(BurstCompiledFunc<T, int, TResult> selector)
            where TResult : struct
        {
            var output = new NativeArray<TResult>(input.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            var job = new SelectWithIndexJob<TResult> { Input = input, Selector = selector, Output = output };
            return new NativeSequence<TResult>(
                output,
                job.Schedule(input.Length, 32, dependsOn)
            );
        }

        public NativeSequence<TResult> Select<TResult>(BurstCompiledFunc<T, TResult> selector)
            where TResult : struct
        {
            var output = new NativeArray<TResult>(input.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            var job = new SelectJob<TResult> { Input = input, Selector = selector, Output = output };
            return new NativeSequence<TResult>(
                output,
                job.Schedule(input.Length, 32, dependsOn)
            );
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct SelectWithIndexJob<TResult> : IJobParallelFor
            where TResult : struct
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<T> Input;

            [ReadOnly]
            public BurstCompiledFunc<T, int, TResult> Selector;

            [WriteOnly]
            public NativeArray<TResult> Output;

            public void Execute(int index)
            {
                Output[index] = Selector.Invoke(Input[index], index);
            }
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct SelectJob<TResult> : IJobParallelFor
            where TResult : struct
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<T> Input;

            [ReadOnly]
            public BurstCompiledFunc<T, TResult> Selector;

            [WriteOnly]
            public NativeArray<TResult> Output;

            public void Execute(int index)
            {
                Output[index] = Selector.Invoke(Input[index]);
            }
        }
    }
}
