using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        public int Count(BurstCompiledFunc<T, bool> predicate)
        {
            var output = new NativeArray<int>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            var job = new CountJob { Input = input, Predicate = predicate, Output = output };
            job.Schedule(input.Length, 64, dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }

        public int Count()
        {
            return Length;
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct CountJob : IJobParallelFor
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<T> Input;

            [ReadOnly]
            public BurstCompiledFunc<T, bool> Predicate;

            [NativeDisableParallelForRestriction]
            public NativeArray<int> Output;

            public void Execute(int index)
            {
                if (Predicate.Invoke(Input[index]))
                    Output[0]++;
            }
        }
    }
}
