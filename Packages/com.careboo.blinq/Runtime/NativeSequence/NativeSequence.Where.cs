using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        public NativeSequence<T> Where(BurstCompiledFunc<T, int, bool> predicate = default)
        {
            var output = new NativeList<T>(input.Length, Allocator.Persistent);
            var job = new WhereWithIndexJob { Input = input, Predicate = predicate, Output = output };
            return new NativeSequence<T>(
                output.AsDeferredJobArray(),
                job.Schedule(dependsOn)
            );
        }

        public NativeSequence<T> Where(BurstCompiledFunc<T, bool> predicate = default)
        {
            var output = new NativeList<T>(input.Length, Allocator.Persistent);
            var job = new WhereJob { Input = input, Predicate = predicate, Output = output };
            return new NativeSequence<T>(
                output.AsDeferredJobArray(),
                job.Schedule(dependsOn)
            );
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct WhereWithIndexJob : IJob
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<T> Input;

            [ReadOnly]
            public BurstCompiledFunc<T, int, bool> Predicate;

            [WriteOnly]
            public NativeList<T> Output;

            public void Execute()
            {
                for (var i = 0; i < Input.Length; i++)
                    if (Predicate.Invoke(Input[i], i))
                        Output.AddNoResize(Input[i]);
            }
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct WhereJob : IJob
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<T> Input;

            [ReadOnly]
            public BurstCompiledFunc<T, bool> Predicate;

            [WriteOnly]
            public NativeList<T> Output;

            public void Execute()
            {
                for (var i = 0; i < Input.Length; i++)
                    if (Predicate.Invoke(Input[i]))
                        Output.AddNoResize(Input[i]);
            }
        }
    }
}
