using System;
using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, RepeatSequence<T>> Repeat<T>(T element, int count)
            where T : struct
        {
            var seq = new RepeatSequence<T>(element, count);
            return ValueSequence<T>.New(ref seq);
        }
    }

    public struct RepeatSequence<T> : ISequence<T>
        where T : struct
    {
        readonly T element;
        int count;

        public RepeatSequence(in T element, int count)
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

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var list = new NativeList<T>(count, allocator);
            for (var i = 0; i < count; i++)
                list.AddNoResize(element);
            return list;
        }
    }
}
