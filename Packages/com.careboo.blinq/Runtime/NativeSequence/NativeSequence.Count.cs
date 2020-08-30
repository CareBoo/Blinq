using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        [CodeGenSourceApi("ce70eaef-6b62-4f4b-b9e4-eff91ab0d629")]
        public int Count(Func<T, bool> predicate)
        {
            throw Error.NotCodeGenerated();
        }

        [CodeGenTargetApi("ce70eaef-6b62-4f4b-b9e4-eff91ab0d629")]
        public int Count<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunc<T, bool>
        {
            var output = new NativeArray<int>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            var job = new CountJob<TPredicate> { Input = input, Predicate = predicate, Output = output };
            job.Schedule(input.Length, 64, dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }

        public int Count()
        {
            return Length;
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct CountJob<TPredicate> : IJobParallelFor
            where TPredicate : struct, IFunc<T, bool>
        {
            [ReadOnly]
            public NativeArray<T> Input;

            [ReadOnly]
            public TPredicate Predicate;

            [NativeDisableParallelForRestriction]
            public NativeArray<int> Output;

            public void Execute(int index)
            {
                if (Predicate.Invoke(Input[index]))
                    Output[0]++;
            }
        }
    }
}
