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
            var job = new ConcatJob { Source = source, Second = second.source };
            dependsOn = job.Schedule();
            return this;
        }

        [BurstCompile(CompileSynchronously = true)]
        internal struct ConcatJob : IJob
        {
            public NativeList<T> Source;

            [ReadOnly]
            public NativeList<T> Second;

            public void Execute()
            {
                Source.AddRange(Second);
            }
        }
    }
}
