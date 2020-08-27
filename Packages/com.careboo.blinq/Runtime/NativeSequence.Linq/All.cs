using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        public bool All(BurstCompiledFunc<T, bool> predicate)
        {
            var output = new NativeArray<bool>(1, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            var job = new AllJob { Input = input, Predicate = predicate, Output = output };
            job.Schedule(dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct AllJob : IJob
        {
            [ReadOnly]
            public NativeArray<T> Input;

            [ReadOnly]
            public BurstCompiledFunc<T, bool> Predicate;

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
    }
}
