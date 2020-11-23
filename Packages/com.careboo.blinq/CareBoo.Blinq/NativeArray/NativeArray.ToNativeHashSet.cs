using System;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public struct ToNativeHashSetJob<T> : IJob
            where T : unmanaged, IEquatable<T>
        {
            [ReadOnly]
            NativeArray<T> source;

            [WriteOnly]
            NativeHashSet<T> hashSet;

            public ToNativeHashSetJob(NativeArray<T> source, ref NativeHashSet<T> hashSet)
            {
                this.source = source;
                this.hashSet = hashSet;
            }

            public void Execute()
            {
                source.ToNativeHashSet(ref hashSet);
            }
        }

        public static NativeHashSet<T> ToNativeHashSet<T>(
            this in NativeArray<T> source,
            in Allocator allocator
            )
            where T : unmanaged, IEquatable<T>
        {
            var result = new NativeHashSet<T>(source.Length, allocator);
            return source.ToNativeHashSet(ref result);
        }

        public static NativeHashSet<T> RunToNativeHashSet<T>(
            this in NativeArray<T> source,
            in Allocator allocator
            )
            where T : unmanaged, IEquatable<T>
        {
            var result = new NativeHashSet<T>(0, allocator);
            return source.RunToNativeHashSet(ref result);
        }

        public static CollectionJobHandle<NativeHashSet<T>> ScheduleToNativeHashSet<T>(
            this in NativeArray<T> source,
            in Allocator allocator
            )
            where T : unmanaged, IEquatable<T>
        {
            var result = new NativeHashSet<T>(0, allocator);
            var jobHandle = new ToNativeHashSetJob<T>(source, ref result).Schedule();
            return new CollectionJobHandle<NativeHashSet<T>>(jobHandle, result);
        }

        public static NativeHashSet<T> ToNativeHashSet<T>(
            this in NativeArray<T> source,
            ref NativeHashSet<T> hashSet
            )
            where T : unmanaged, IEquatable<T>
        {
            hashSet.Capacity = source.Length;
            for (var i = 0; i < source.Length; i++)
                hashSet.Add(source[i]);
            return hashSet;
        }

        public static NativeHashSet<T> RunToNativeHashSet<T>(
            this in NativeArray<T> source,
            ref NativeHashSet<T> hashSet
            )
            where T : unmanaged, IEquatable<T>
        {
            new ToNativeHashSetJob<T>(source, ref hashSet).Run();
            return hashSet;
        }

        public static JobHandle ScheduleToNativeHashSet<T>(
            this in NativeArray<T> source,
            ref NativeHashSet<T> hashSet
            )
            where T : unmanaged, IEquatable<T>
        {
            return new ToNativeHashSetJob<T>(source, ref hashSet).Schedule();
        }
    }
}
