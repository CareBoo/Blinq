using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        [CodeGenSourceApi("156435a9-2bbf-4a7e-a395-a5e697a81e85")]
        public bool Any(PureFunc<T, bool> predicate)
        {
            throw Error.NotCodeGenerated();
        }

        [CodeGenTargetApi("156435a9-2bbf-4a7e-a395-a5e697a81e85")]
        public bool Any<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IValueFunc<T, bool>
        {
            var output = new NativeArray<bool>(1, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            var job = new AnyJob<TPredicate> { Input = source, Predicate = predicate, Output = output };
            job.Schedule(dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }

        public bool Any()
        {
            dependsOn.Complete();
            var result = source.Length > 0;

            return result;
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct AnyJob<TPredicate> : IJob
            where TPredicate : struct, IValueFunc<T, bool>
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
                    if (Predicate.Invoke(Input[i]))
                    {
                        Output[0] = true;
                        return;
                    }
                Output[0] = false;
            }
        }
    }
}
