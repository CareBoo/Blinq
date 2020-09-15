using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ConcatSequence<T, NativeArraySequence<T>, TSecond>> Concat<T, TSecond>(
            this ref NativeArray<T> source,
            TSecond second
            )
            where T : struct
            where TSecond : struct, ISequence<T>
        {
            return source.ToValueSequence().Concat(second);
        }

        public static ValueSequence<T, ConcatSequence<T, NativeArraySequence<T>, NativeArraySequence<T>>> Concat<T>(
            this ref NativeArray<T> source,
            NativeArray<T> second
            )
            where T : struct
        {
            return source.ToValueSequence().Concat(second);
        }
    }
}
