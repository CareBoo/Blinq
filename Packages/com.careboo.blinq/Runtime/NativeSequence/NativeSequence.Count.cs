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
            var job = new CountJob<TPredicate> { Source = source, Predicate = predicate, Output = output };
            job.Schedule(source.Length, 64, dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }

        public int Count()
        {
            dependsOn.Complete();
            return source.Length;
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct CountJob<TPredicate> : IJobParallelFor
            where TPredicate : struct, IFunc<T, bool>
        {
            [ReadOnly]
            public NativeList<T> Source;

            [ReadOnly]
            public TPredicate Predicate;

            [NativeDisableParallelForRestriction]
            public NativeArray<int> Output;

            public void Execute(int index)
            {
                if (Predicate.Invoke(Source[index]))
                    Output[0]++;
            }
        }
    }
}
