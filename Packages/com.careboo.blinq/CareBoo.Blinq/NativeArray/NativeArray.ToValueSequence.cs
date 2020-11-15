using System;
using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public struct NativeArraySequence<T> : ISequence<T>
        where T : struct
    {
        readonly NativeArray<T> source;
        NativeArray<T>.Enumerator sourceEnum;

        public NativeArraySequence(in NativeArray<T> source)
        {
            this.source = source;
            sourceEnum = source.GetEnumerator();
        }

        public T Current => sourceEnum.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            return sourceEnum.MoveNext();
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public NativeList<T> ToNativeList(Allocator allocator)
        {
            var list = new NativeList<T>(source.Length, allocator);
            for (var i = 0; i < source.Length; i++)
                list.AddNoResize(source[i]);
            return list;
        }
    }

    public static partial class Sequence
    {
        public static ValueSequence<T, NativeArraySequence<T>> ToValueSequence<T>(this in NativeArray<T> nativeArray)
            where T : struct
        {
            var newSequence = new NativeArraySequence<T>(in nativeArray);
            return ValueSequence<T>.New(ref newSequence);
        }
    }
}
