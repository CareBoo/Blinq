using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public struct ValueSequence<T, TSource, TEnumerator>
        : ISequence<T, TEnumerator>
        where T : struct
        where TSource : struct, ISequence<T, TEnumerator>
        where TEnumerator : struct, IEnumerator<T>
    {
        public readonly TSource Source;

        public ValueSequence(in TSource source)
        {
            Source = source;
        }

        public TEnumerator GetEnumerator()
        {
            return Source.GetEnumerator();
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
            return Source.ToNativeList(allocator);
        }
    }

    public static class ValueSequence<T, TEnumerator>
        where T : struct
        where TEnumerator : struct, IEnumerator<T>
    {
        public static ValueSequence<T, TSource, TEnumerator> New<TSource>(in TSource source)
            where TSource : struct, ISequence<T, TEnumerator>
        {
            return new ValueSequence<T, TSource, TEnumerator>(in source);
        }
    }
}
