using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq.NativeArray
{
    [BurstCompile(CompileSynchronously = true)]
    public struct AllJob<TSource, TPredicate> : IJob
        where TSource : unmanaged
        where TPredicate : IPredicate<TSource>
    {
        [ReadOnly]
        public NativeArray<TSource> Input;

        [ReadOnly]
        public TPredicate Predicate;

        [WriteOnly]
        public NativeArray<bool> Output;

        public void Execute()
        {
            var result = true;
            for (var i = 0; i < Input.Length; i++)
                result &= Predicate.Invoke(Input[i]);
            Output[0] = result;
        }
    }

    public static partial class BlinqExtensions
    {
        public static bool All<TSource, TPredicate>(this ref NativeArray<TSource> source, JobHandle dependsOn = default)
            where TSource : unmanaged
            where TPredicate : IPredicate<TSource>
        {
            var output = new NativeArray<bool>(1, Allocator.Persistent);
            var job = new AllJob<TSource, TPredicate> { Input = source, Predicate = default, Output = output };
            job.Schedule(dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }

        public static bool All<TSource, TPredicate>(this TSource[] source)
            where TSource : unmanaged
            where TPredicate : IPredicate<TSource>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var input = new NativeArray<TSource>(source, Allocator.Persistent);
            var result = input.All<TSource, TPredicate>();
            input.Dispose();
            return result;
        }

        public static bool All<TSource, TPredicate>(this List<TSource> source)
            where TSource : unmanaged
            where TPredicate : IPredicate<TSource>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var input = source.ToNativeArray(Allocator.Persistent);
            var result = input.All<TSource, TPredicate>();
            input.Dispose();
            return result;
        }
    }
}
