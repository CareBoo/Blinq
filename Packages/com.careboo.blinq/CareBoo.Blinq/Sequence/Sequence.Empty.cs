using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, EmptySequence<T>> Empty<T>()
            where T : struct
        {
            var seq = new EmptySequence<T>();
            return ValueSequence<T>.New(ref seq);
        }
    }

    public struct EmptySequence<T> : ISequence<T>
        where T : struct
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

        public NativeList<T> ToList()
        {
            return new NativeList<T>(Allocator.Temp);
        }
    }
}
