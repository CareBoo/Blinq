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
        public NativeSequence<T> Where(PureFunc<T, int, bool> predicate)
        {
            throw Error.NotCodeGenerated();
        }

        [CodeGenTargetApi("0beab518-4e77-4166-8c32-d191679c2aa2")]
        public NativeSequence<T> WhereWithIndex<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IValueFunc<T, int, bool>
        {
            var job = new WhereWithIndexJob<TPredicate> { Source = source, Predicate = predicate };
            dependsOn = job.Schedule(dependsOn);
            return this;
        }

        [CodeGenSourceApi("a5d240e9-7f53-4bd7-9061-d8289bdf224f")]
        public NativeSequence<T> Where(PureFunc<T, bool> predicate)
        {
            throw Error.NotCodeGenerated();
        }

        [CodeGenTargetApi("a5d240e9-7f53-4bd7-9061-d8289bdf224f")]
        public NativeSequence<T> Where<TPredicate>(TPredicate predicate = default)
            where TPredicate : struct, IValueFunc<T, bool>
        {
            var job = new WhereJob<TPredicate> { Source = source, Predicate = predicate };
            dependsOn = job.Schedule(dependsOn);
            return this;
        }

        [BurstCompile(CompileSynchronously = true)]
        internal struct WhereWithIndexJob<TPredicate> : IJob
            where TPredicate : struct, IValueFunc<T, int, bool>
        {
            public NativeList<T> Source;

            [ReadOnly]
            public TPredicate Predicate;

            public void Execute()
            {
                var length = Source.Length;
                for (var i = 0; i < length; i++)
                {
                    if (!Predicate.Invoke(Source[i], i))
                    {
                        Source.RemoveAt(i);
                        i--;
                        length--;
                    }
                }
            }
        }

        [BurstCompile(CompileSynchronously = true)]
        internal struct WhereJob<TPredicate> : IJob
            where TPredicate : struct, IValueFunc<T, bool>
        {
            public NativeList<T> Source;

            [ReadOnly]
            public TPredicate Predicate;

            public void Execute()
            {
                var length = Source.Length;
                for (var i = 0; i < length; i++)
                {
                    if (!Predicate.Invoke(Source[i]))
                    {
                        Source.RemoveAt(i);
                        i--;
                        length--;
                    }
                }
            }
        }
    }
}
