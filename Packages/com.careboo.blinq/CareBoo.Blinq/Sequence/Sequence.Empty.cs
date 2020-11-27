using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, EmptySequence<T>, EmptySequence<T>.Enumerator> Empty<T>()
            where T : struct
        {
            var seq = new EmptySequence<T>();
            return ValueSequence<T, EmptySequence<T>.Enumerator>.New(in seq);
        }
    }

    public struct EmptySequence<T>
        : ISequence<T, EmptySequence<T>.Enumerator>
        where T : struct
    {
        public struct Enumerator : IEnumerator<T>
        {
            public T Current => default;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                return false;
            }

            public void Reset()
            {
            }

        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            return new NativeList<T>(allocator);
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator();
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
