using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ExceptSequence<T, NativeArraySequence<T>, TSecond>> Except<T, TSecond>(
            this ref NativeArray<T> source,
            ValueSequence<T, TSecond> second
            )
            where T : unmanaged, IEquatable<T>
            where TSecond : struct, ISequence<T>
        {
            return source.ToValueSequence().Except(second);
        }

        public static ValueSequence<T, ExceptSequence<T, NativeArraySequence<T>, NativeArraySequence<T>>> Except<T>(
            this ref NativeArray<T> source,
            NativeArray<T> second
            )
            where T : unmanaged, IEquatable<T>
        {
            return source.ToValueSequence().Except(second);
        }
    }
}
