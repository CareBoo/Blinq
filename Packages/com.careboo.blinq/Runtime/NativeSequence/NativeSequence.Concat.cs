using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        public NativeSequence<T> Concat(NativeSequence<T> second)
        {
            var output = new NativeList<T>(Length + second.Length, Allocator.Persistent);
            var job = new ConcatJob { First = input, Second = second, Output = output };
            return new NativeSequence<T>(
                output.AsDeferredJobArray(),
                job.Schedule(dependsOn)
            );
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct ConcatJob : IJob
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<T> First;

            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<T> Second;

            [WriteOnly]
            public NativeList<T> Output;

            public void Execute()
            {
                for (var i = 0; i < First.Length; i++)
                    Output.AddNoResize(First[i]);
                for (var i = 0; i < Second.Length; i++)
                    Output.AddNoResize(Second[i]);
            }
        }
    }
}
