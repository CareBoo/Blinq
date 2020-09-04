using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        : INativeDisposable
        , IEnumerable<T>
        , IEquatable<NativeSequence<T>>
        where T : struct
    {
        private NativeList<T> source;

        private JobHandle dependsOn;

        public NativeSequence(NativeList<T> source, JobHandle dependsOn = default)
        {
            this.source = source;
            this.dependsOn = dependsOn;
        }

        public NativeSequence(T[] source, Allocator allocator)
            : this(source.ToNativeList(allocator))
        {
        }

        public NativeSequence(NativeArray<T> source, Allocator allocator)
            : this(source.ToNativeList(allocator))
        {
        }

        public void Dispose()
        {
            dependsOn.Complete();
            source.Dispose();
        }

        public void Complete()
        {
            dependsOn.Complete();
        }

        public JobHandle Dispose(JobHandle inputDeps)
        {
            dependsOn = JobHandle.CombineDependencies(dependsOn, inputDeps);
            return source.Dispose(dependsOn);
        }


        public bool Equals(NativeSequence<T> other)
        {
            return source.Equals(other.source);
        }

        public NativeArray<T>.Enumerator GetEnumerator()
        {
            dependsOn.Complete();
            var enumerator = source.GetEnumerator();
            return enumerator;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator NativeList<T>(NativeSequence<T> from)
        {
            return from.source;
        }
    }
}
