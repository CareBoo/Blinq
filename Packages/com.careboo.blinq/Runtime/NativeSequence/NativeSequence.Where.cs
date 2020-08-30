using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        where T : struct
    {
        [CodeGenSourceApi("0beab518-4e77-4166-8c32-d191679c2aa2")]
        public NativeSequence<T> Where(Func<T, int, bool> predicate)
        {
            throw Error.NotCodeGenerated();
        }

        [CodeGenTargetApi("0beab518-4e77-4166-8c32-d191679c2aa2")]
        public NativeSequence<T> WhereWithIndex<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IFunc<T, int, bool>
        {
            var output = new NativeList<T>(input.Length, Allocator.Persistent);
            var job = new WhereWithIndexJob<TPredicate> { Input = input, Predicate = predicate, Output = output };
            return new NativeSequence<T>(
                output.AsDeferredJobArray(),
                job.Schedule(dependsOn)
            );
        }

        [CodeGenSourceApi("a5d240e9-7f53-4bd7-9061-d8289bdf224f")]
        public NativeSequence<T> Where(Func<T, bool> predicate)
        {
            throw Error.NotCodeGenerated();
        }

        [CodeGenTargetApi("a5d240e9-7f53-4bd7-9061-d8289bdf224f")]
        public NativeSequence<T> Where<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IFunc<T, bool>
        {
            var output = new NativeList<T>(input.Length, Allocator.Persistent);
            var job = new WhereJob<TPredicate> { Input = input, Predicate = predicate, Output = output };
            return new NativeSequence<T>(
                output.AsDeferredJobArray(),
                job.Schedule(dependsOn)
            );
        }

        [BurstCompile(CompileSynchronously = true)]
        public struct WhereWithIndexJob<TPredicate> : IJob
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

        [BurstCompile(CompileSynchronously = true)]
        public struct WhereJob<TPredicate> : IJob
            where TPredicate : struct, IFunc<T, bool>
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
                    if (Predicate.Invoke(Input[i]))
                        Output.AddNoResize(Input[i]);
            }
        }
    }
}
