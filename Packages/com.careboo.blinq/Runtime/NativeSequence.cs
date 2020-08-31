using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T>
        : IDisposable
        , IEnumerable<T>
        , IEquatable<NativeSequence<T>>
        where T : struct
    {
        private NativeList<T> source;

        private JobHandle dependsOn;

        public JobHandle DependsOn => dependsOn;

        private NativeSequence(NativeList<T> source, JobHandle dependsOn = default)
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

        public static implicit operator NativeArray<T>(NativeSequence<T> from)
        {
            return from.source;
        }
    }
}
