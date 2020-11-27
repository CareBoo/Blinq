using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            T,
            ExceptSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TSecond, TSecondEnumerator>,
            ExceptSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TSecond, TSecondEnumerator>.Enumerator>
                Except<T, TSecond, TSecondEnumerator>(
            this in NativeArray<T> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : unmanaged, IEquatable<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            return source.ToValueSequence().Except(second);
        }

        public static ValueSequence<
            T,
            ExceptSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, NativeArraySequence<T>, NativeArray<T>.Enumerator>,
            ExceptSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, NativeArraySequence<T>, NativeArray<T>.Enumerator>.Enumerator>
                Except<T>(
            this in NativeArray<T> source,
            in NativeArray<T> second
            )
            where T : unmanaged, IEquatable<T>
        {
            return source.ToValueSequence().Except(second);
        }
    }
}
