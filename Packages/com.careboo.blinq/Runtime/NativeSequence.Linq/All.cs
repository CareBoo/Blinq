using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        [BurstCompile(CompileSynchronously = true)]
        public struct AllJob<TPredicate> : IJob
            where TPredicate : struct, IPredicate<T>
        {
            [ReadOnly]
            public NativeArray<T> Input;

            [ReadOnly]
            public TPredicate Predicate;

            [WriteOnly]
            public NativeArray<bool> Output;

            public void Execute()
            {
                for (var i = 0; i < Input.Length; i++)
                    if (!Predicate.Invoke(Input[i]))
                    {
                        Output[0] = false;
                        return;
                    }
                Output[0] = true;
            }
        }

        public bool All<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IPredicate<T>
        {
            var output = new NativeArray<bool>(1, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            var job = new AllJob<TPredicate> { Input = input, Predicate = predicate, Output = output };
            job.Schedule(dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }
    }
}
