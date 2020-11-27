using System;
using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            T,
            UnionSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TSecond, TSecondEnumerator>,
            UnionSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TSecond, TSecondEnumerator>.Enumerator>
        Union<T, TSecond, TSecondEnumerator>(
            this in NativeArray<T> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : unmanaged, IEquatable<T>
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Union(in second);
        }

        public static ValueSequence<
            T,
            UnionSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, NativeArraySequence<T>, NativeArray<T>.Enumerator>,
            UnionSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, NativeArraySequence<T>, NativeArray<T>.Enumerator>.Enumerator>
        Union<T>(
            this in NativeArray<T> source,
            in NativeArray<T> second
            )
            where T : unmanaged, IEquatable<T>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Union(in second);
        }
    }
}
