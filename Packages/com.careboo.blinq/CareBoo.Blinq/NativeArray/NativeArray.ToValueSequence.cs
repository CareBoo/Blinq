using System;
using System.Collections;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public struct NativeArraySequence<T> : ISequence<T>
        where T : struct
    {
        readonly NativeArray<T> source;
        readonly NativeArray<T>.Enumerator sourceEnum;

        public NativeArraySequence(NativeArray<T> source)
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

        public NativeList<T> ToList()
        {
            var list = new NativeList<T>(source.Length, Allocator.Temp);
            for (var i = 0; i < source.Length; i++)
                list.AddNoResize(source[i]);
            return list;
        }
    }

    public static partial class Sequence
    {
        public static ValueSequence<T, NativeArraySequence<T>> ToValueSequence<T>(this ref NativeArray<T> nativeArray)
            where T : struct
        {
            var newSequence = new NativeArraySequence<T>(nativeArray);
            return ValueSequence<T>.New(newSequence);
        }
    }
}
