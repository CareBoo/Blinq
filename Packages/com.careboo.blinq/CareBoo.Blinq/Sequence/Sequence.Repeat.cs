using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, RepeatSequence<T>, RepeatSequence<T>.Enumerator> Repeat<T>(in T element, int count)
            where T : struct
        {
            var seq = new RepeatSequence<T>(in element, count);
            return ValueSequence<T, RepeatSequence<T>.Enumerator>.New(in seq);
        }
    }

    public struct RepeatSequence<T>
        : ISequence<T, RepeatSequence<T>.Enumerator>
        where T : struct
    {
        public struct Enumerator : IEnumerator<T>
        {
            readonly T element;
            int count;

            public Enumerator(in T element, int count)
            {
                this.element = element;
                this.count = count;
            }

            public T Current => element;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                count -= 1;
                return count >= 0;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        readonly T element;

        readonly int count;

        public RepeatSequence(in T element, int count)
        {
            this.element = element;
            this.count = count;
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var list = new NativeList<T>(count, allocator);
            for (var i = 0; i < count; i++)
                list.AddNoResize(element);
            return list;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(in element, count);
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
