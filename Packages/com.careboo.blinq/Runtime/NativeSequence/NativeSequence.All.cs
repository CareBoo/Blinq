using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        [CodeGenSourceApi("927b1d67-8862-43a8-b4f4-99e90e9e5b30")]
        public bool All(Func<T, bool> predicate)
        {
            throw Error.NotCodeGenerated();
        }

        [CodeGenTargetApi("927b1d67-8862-43a8-b4f4-99e90e9e5b30")]
        public bool All<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IFunc<T, bool>
        {
            var output = new NativeArray<bool>(1, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            var job = new AllJob<TPredicate> { Input = source, Predicate = predicate, Output = output };
            job.Schedule(dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct AllJob<TPredicate> : IJob
            where TPredicate : struct, IFunc<T, bool>
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
    }
}
