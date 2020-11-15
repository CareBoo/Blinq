using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public struct ValueSequence<T, TSource>
        : IEnumerable<T>
        where T : struct
        where TSource : struct, ISequence<T>
    {
        TSource source;

        public ValueSequence(ref TSource source)
        {
            this.source = source;
        }

        public TSource GetEnumerator()
        {
            return source;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            return source.ToNativeList(allocator);
        }
    }

    public static class ValueSequence<T>
        where T : struct
    {
        public static ValueSequence<T, TSource> New<TSource>(ref TSource source)
            where TSource : struct, ISequence<T>
        {
            return new ValueSequence<T, TSource>(ref source);
        }
    }
}
