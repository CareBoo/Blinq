using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public struct NativeArraySequence<T> : ISequence<T, NativeArray<T>.Enumerator>
        where T : struct
    {
        readonly NativeArray<T> source;

        public NativeArraySequence(in NativeArray<T> source)
        {
            this.source = source;
        }

        public NativeArray<T>.Enumerator GetEnumerator()
        {
            return source.GetEnumerator();
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var list = new NativeList<T>(source.Length, allocator);
            for (var i = 0; i < source.Length; i++)
                list.AddNoResize(source[i]);
            return list;
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

    public static partial class Sequence
    {
        public static ValueSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator> ToValueSequence<T>(this in NativeArray<T> nativeArray)
            where T : struct
        {
            var newSequence = new NativeArraySequence<T>(in nativeArray);
            return ValueSequence<T, NativeArray<T>.Enumerator>.New(in newSequence);
        }
    }
}
