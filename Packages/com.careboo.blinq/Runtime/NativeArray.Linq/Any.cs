using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    [BurstCompile(CompileSynchronously = true)]
    public struct AnyJob<TSource, TPredicate> : IJob
        where TSource : unmanaged
        where TPredicate : unmanaged, IPredicate<TSource>
    {
        [ReadOnly]
        public NativeArray<TSource> Input;

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

    [BurstCompile(CompileSynchronously = true)]
    public struct SomeJob : IJob
    {
        [ReadOnly]
        public NativeArray<int> Input;

        [WriteOnly]
        public NativeArray<int> Output;

        public void Execute()
        {
            Output[0] = Input[0] + Input[1];
        }
    }

    public static partial class Enumerable
    {
        [BurstCompile]
        public static bool Any<TSource, TPredicate>(this NativeArray<TSource> source, JobHandle dependsOn = default)
            where TSource : unmanaged
            where TPredicate : unmanaged, IPredicate<TSource>
        {
            var output = new NativeArray<bool>(1, Allocator.Persistent);
            var job = new AnyJob<TSource, TPredicate> { Input = source, Output = output };
            job.Schedule(dependsOn).Complete();
            var result = output[0];
            output.Dispose();
            return result;
        }

        [BurstCompile]
        public static bool Any<TSource>(this NativeArray<TSource> source, JobHandle dependsOn = default)
            where TSource : unmanaged
        {
            dependsOn.Complete();
            return source.Length > 0;
        }

        // public static bool Any<TSource, TPredicate>(this TSource[] source)
        //     where TSource : unmanaged
        //     where TPredicate : unmanaged, IPredicate<TSource>
        // {
        //     if (source == null) throw new ArgumentNullException(nameof(source));

        //     var input = new NativeArray<TSource>(source, Allocator.Persistent);
        //     var result = input.Any<TSource, TPredicate>();
        //     input.Dispose();
        //     return result;
        // }

        // public static bool Any<TSource>(this TSource[] source)
        // {
        //     if (source == null) throw new ArgumentNullException(nameof(source));

        //     return source.Length > 0;
        // }

        // public static bool Any<TSource, TPredicate>(this List<TSource> source)
        //     where TSource : unmanaged
        //     where TPredicate : unmanaged, IPredicate<TSource>
        // {
        //     if (source == null) throw new ArgumentNullException(nameof(source));

        //     var input = source.ToNativeArray(Allocator.Persistent);
        //     var result = input.Any<TSource, TPredicate>();
        //     input.Dispose();
        //     return result;
        // }

        // public static bool Any<TSource>(this List<TSource> source)
        // {
        //     if (source == null) throw new ArgumentNullException(nameof(source));

        //     return source.Count > 0;
        // }
    }
}
