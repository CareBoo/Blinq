using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        [BurstCompile]
        public struct SequenceToNativeHashSetJob<T, TSource, TSourceEnumerator> : IJob
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            [ReadOnly]
            ValueSequence<T, TSource, TSourceEnumerator> source;

            [WriteOnly]
            NativeHashSet<T> hashSet;

            public SequenceToNativeHashSetJob(ValueSequence<T, TSource, TSourceEnumerator> source, ref NativeHashSet<T> hashSet)
            {
                this.source = source;
                this.hashSet = hashSet;
            }

            public void Execute()
            {
                source.ToNativeHashSet(ref hashSet);
            }
        }

        public static NativeHashSet<T> ToNativeHashSet<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            Allocator allocator
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var result = new NativeHashSet<T>(0, allocator);
            return ToNativeHashSet(source, ref result);
        }

        public static NativeHashSet<T> RunToNativeHashSet<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            Allocator allocator
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var result = new NativeHashSet<T>(0, allocator);
            return source.RunToNativeHashSet(ref result);
        }

        public static CollectionJobHandle<NativeHashSet<T>> ScheduleToNativeHashSet<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            Allocator allocator
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var result = new NativeHashSet<T>(0, allocator);
            var jobHandle = new SequenceToNativeHashSetJob<T, TSource, TSourceEnumerator>(source, ref result).Schedule();
            return new CollectionJobHandle<NativeHashSet<T>>(jobHandle, result);
        }

        public static NativeHashSet<T> ToNativeHashSet<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeHashSet<T> hashSet
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            var list = source.ToNativeList(Allocator.Temp);
            hashSet.Capacity = list.Length;
            for (var i = 0; i < list.Length; i++)
                hashSet.Add(list[i]);
            list.Dispose();
            return hashSet;
        }

        public static NativeHashSet<T> RunToNativeHashSet<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeHashSet<T> hashSet
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            new SequenceToNativeHashSetJob<T, TSource, TSourceEnumerator>(source, ref hashSet).Run();
            return hashSet;
        }

        public static JobHandle ScheduleToNativeHashSet<T, TSource, TSourceEnumerator>(
            this in ValueSequence<T, TSource, TSourceEnumerator> source,
            ref NativeHashSet<T> hashSet
            )
            where T : unmanaged, IEquatable<T>
            where TSource : struct, ISequence<T, TSourceEnumerator>
            where TSourceEnumerator : struct, IEnumerator<T>
        {
            return new SequenceToNativeHashSetJob<T, TSource, TSourceEnumerator>(source, ref hashSet).Schedule();
        }
    }
}
