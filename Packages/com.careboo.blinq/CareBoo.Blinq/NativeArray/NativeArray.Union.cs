using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<T, UnionSequence<T, NativeArraySequence<T>, TSecond>> Union<T, TSecond>(
           this ref NativeArray<T> source,
           ValueSequence<T, TSecond> second
           )
           where T : unmanaged, IEquatable<T>
           where TSecond : struct, ISequence<T>
        {
            var seq = source.ToValueSequence();
            return seq.Union(second);
        }

        public static ValueSequence<T, UnionSequence<T, NativeArraySequence<T>, NativeArraySequence<T>>> Union<T>(
           this ref NativeArray<T> source,
           NativeArray<T> second
           )
           where T : unmanaged, IEquatable<T>
        {
            var seq = source.ToValueSequence();
            return seq.Union(second);
        }

    }
}
