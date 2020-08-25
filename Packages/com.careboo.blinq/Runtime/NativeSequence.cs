using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;

namespace CareBoo.Blinq
{
    public partial struct NativeSequence<T> : IDisposable, IEnumerable<T>, IEnumerable, IEquatable<NativeSequence<T>>
        where T : struct
    {
        private NativeList<T> input;
        private JobHandle dependsOn;

        public NativeSequence(NativeList<T> input, JobHandle dependsOn = default)
        {
            this.input = input;
            this.dependsOn = dependsOn;
        }

        public NativeSequence(NativeArray<T> input, Allocator allocator)
            : this(new NativeList<T>(input.Length, allocator))
        {
            this.input.CopyFrom(input);
        }

        public NativeSequence(T[] input, Allocator allocator)
            : this(new NativeList<T>(input.Length, allocator))
        {
            this.input.CopyFrom(input);
        }

        public void Dispose()
        {
            dependsOn.Complete();
            input.Dispose();
        }

        public bool Equals(NativeSequence<T> other)
        {
            return input.Equals(other.input);
        }

        public NativeArray<T>.Enumerator GetEnumerator()
        {
            dependsOn.Complete();
            return input.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
