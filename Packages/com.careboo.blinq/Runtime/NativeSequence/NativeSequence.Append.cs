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
            var job = new AppendJob { Source = source, Item = item };
            dependsOn = job.Schedule(dependsOn);
            return this;
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct AppendJob : IJob
        {
            public NativeList<T> Source;

            [ReadOnly]
            public T Item;

            public void Execute()
            {
                Source.Add(Item);
            }
        }
    }
}
