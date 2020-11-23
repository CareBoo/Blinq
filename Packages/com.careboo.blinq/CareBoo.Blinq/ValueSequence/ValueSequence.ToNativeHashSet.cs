using System;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public struct ToNativeHashSetJob<T, TSource> : IJob
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            [ReadOnly]
            ValueSequence<T, TSource> source;

            [WriteOnly]
            NativeHashSet<T> hashSet;

            public ToNativeHashSetJob(ValueSequence<T, TSource> source, ref NativeHashSet<T> hashSet)
            {
                this.source = source;
                this.hashSet = hashSet;
            }

            public void Execute()
            {
                source.ToNativeHashSet(ref hashSet);
            }
        }

        public static NativeHashSet<T> ToNativeHashSet<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in Allocator allocator
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            var result = new NativeHashSet<T>(0, allocator);
            return ToNativeHashSet(source, ref result);
        }

        public static NativeHashSet<T> RunToNativeHashSet<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in Allocator allocator
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            var result = new NativeHashSet<T>(0, allocator);
            return source.RunToNativeHashSet(ref result);
        }

        public static CollectionJobHandle<NativeHashSet<T>> ScheduleToNativeHashSet<T, TSource>(
            this in ValueSequence<T, TSource> source,
            in Allocator allocator
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            var result = new NativeHashSet<T>(0, allocator);
            var jobHandle = new ToNativeHashSetJob<T, TSource>(source, ref result).Schedule();
            return new CollectionJobHandle<NativeHashSet<T>>(jobHandle, result);
        }

        public static NativeHashSet<T> ToNativeHashSet<T, TSource>(
            this in ValueSequence<T, TSource> source,
            ref NativeHashSet<T> hashSet
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            var list = source.ToNativeList(Allocator.Temp);
            hashSet.Capacity = list.Length;
            for (var i = 0; i < list.Length; i++)
                hashSet.Add(list[i]);
            list.Dispose();
            return hashSet;
        }

        public static NativeHashSet<T> RunToNativeHashSet<T, TSource>(
            this in ValueSequence<T, TSource> source,
            ref NativeHashSet<T> hashSet
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            new ToNativeHashSetJob<T, TSource>(source, ref hashSet).Run();
            return hashSet;
        }

        public static JobHandle ScheduleToNativeHashSet<T, TSource>(
            this in ValueSequence<T, TSource> source,
            ref NativeHashSet<T> hashSet
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T>
        {
            return new ToNativeHashSetJob<T, TSource>(source, ref hashSet).Schedule();
        }
    }
}
