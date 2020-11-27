using System.Collections.Generic;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            T,
            ConcatSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TSecond, TSecondEnumerator>,
            ConcatSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, TSecond, TSecondEnumerator>.Enumerator>
        Concat<T, TSecond, TSecondEnumerator>(
            this in NativeArray<T> source,
            in ValueSequence<T, TSecond, TSecondEnumerator> second
            )
            where T : struct
            where TSecond : struct, ISequence<T, TSecondEnumerator>
            where TSecondEnumerator : struct, IEnumerator<T>
        {
            return source.ToValueSequence().Concat(second);
        }

        public static ValueSequence<
            T,
            ConcatSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, NativeArraySequence<T>, NativeArray<T>.Enumerator>,
            ConcatSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator, NativeArraySequence<T>, NativeArray<T>.Enumerator>.Enumerator>
        Concat<T>(
            this in NativeArray<T> source,
            in NativeArray<T> second
            )
            where T : struct
        {
            return source.ToValueSequence().Concat(second);
        }
    }
}
