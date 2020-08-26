using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        public NativeSequence<T> Where<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var output = new NativeList<T>(input.Length, Allocator.Persistent);
            var job = new WhereJob<TPredicate> { Input = input, Predicate = predicate, Output = output };
            dependsOn = job.Schedule(dependsOn);
            input = output.AsDeferredJobArray();
            return this;
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct WhereJob<TPredicate> : IJob
            where TPredicate : struct, IFunc<T, int, bool>
        {
            [ReadOnly]
            [DeallocateOnJobCompletion]
            public NativeArray<T> Input;

            [ReadOnly]
            public TPredicate Predicate;

            [WriteOnly]
            public NativeList<T> Output;

            public void Execute()
            {
                for (var i = 0; i < Input.Length; i++)
                    if (Predicate.Invoke(Input[i], i))
                        Output.AddNoResize(Input[i]);
            }
        }
    }
}
