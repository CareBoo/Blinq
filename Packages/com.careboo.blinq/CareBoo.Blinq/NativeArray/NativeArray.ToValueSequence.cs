using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public struct NativeArraySequence<T> : ISequence<T>
        where T : struct, IEquatable<T>
    {
        public NativeArray<T> Source;

        public NativeList<T> Execute()
        {
            var list = new NativeList<T>(Source.Length, Allocator.Temp);
            for (var i = 0; i < Source.Length; i++)
                list.AddNoResize(Source[i]);
            return list;
        }
    }

    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<T, NativeArraySequence<T>> ToValueSequence<T>(this ref NativeArray<T> nativeArray)
            where T : struct, IEquatable<T>
        {
            var newSequence = new NativeArraySequence<T> { Source = nativeArray };
            return new ValueSequence<T, NativeArraySequence<T>>(newSequence);
        }
    }
}
