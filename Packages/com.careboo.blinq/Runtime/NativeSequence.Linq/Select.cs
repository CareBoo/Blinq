using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        public NativeSequence<TResult> Select<TResult, TSelector>(TSelector selector = default)
            where TResult : struct
            where TSelector : struct, IFunc<T, int, TResult>
        {
            var output = new NativeArray<TResult>(input.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            var job = new SelectJob<TResult, TSelector> { Input = input, Selector = selector, Output = output };
            return new NativeSequence<TResult>(
                output,
                job.Schedule(input.Length, 32, dependsOn)
            );
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct SelectJob<TResult, TSelector> : IJobParallelFor
            where TResult : struct
            where TSelector : struct, IFunc<T, int, TResult>
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<T> Input;

            [ReadOnly]
            public TSelector Selector;

            [WriteOnly]
            public NativeArray<TResult> Output;

            public void Execute(int index)
            {
                Output[index] = Selector.Invoke(Input[index], index);
            }
        }
    }
}
