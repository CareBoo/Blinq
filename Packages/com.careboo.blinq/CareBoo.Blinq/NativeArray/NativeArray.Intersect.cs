using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            T,
            IntersectSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TSecond, TSecondEnumerator>,
            IntersectSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TSecond, TSecondEnumerator>.Enumerator>
        Intersect<T, TSecond, TSecondEnumerator>(
            this in NativeArray<T> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : unmanaged, IEquatable<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            return source.ToValueSequence().Intersect(in second);
        }

        public static ValueSequence<
            T,
            IntersectSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, NativeArraySequence<T>, NativeArray<T>.Enumerator>,
            IntersectSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, NativeArraySequence<T>, NativeArray<T>.Enumerator>.Enumerator>
        Intersect<T>(
            this in NativeArray<T> source,
            in NativeArray<T> second
            )
            where T : unmanaged, IEquatable<T>
        {
            return source.ToValueSequence().Intersect(in second);
        }
    }
}
