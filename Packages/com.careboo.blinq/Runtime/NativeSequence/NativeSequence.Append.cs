using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        public NativeSequence<T> Append(T item)
        {
            var output = new NativeList<T>(input.Length + 1, Allocator.Persistent);
            var job = new AppendJob { Input = input, Item = item, Output = output };
            return new NativeSequence<T>(
                output.AsDeferredJobArray(),
                job.Schedule(dependsOn)
            );
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct AppendJob : IJob
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<T> Input;

            [ReadOnly]
            public T Item;

            [WriteOnly]
            public NativeList<T> Output;

            public void Execute()
            {
                for (var i = 0; i < Input.Length; i++)
                    Output.AddNoResize(Input[i]);
                Output.AddNoResize(Item);
            }
        }
    }
}
