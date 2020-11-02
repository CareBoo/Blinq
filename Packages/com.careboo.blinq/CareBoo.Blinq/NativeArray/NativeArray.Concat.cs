using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, ConcatSequence<T, NativeArraySequence<T>, TSecond>> Concat<T, TSecond>(
            this ref NativeArray<T> source,
            ValueSequence<T, TSecond> second
            )
            where T : struct
            where TSecond : struct, ISequence<T>
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Concat(second);
        }

        public static ValueSequence<T, ConcatSequence<T, NativeArraySequence<T>, NativeArraySequence<T>>> Concat<T>(
            this ref NativeArray<T> source,
            NativeArray<T> second
            )
            where T : struct
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.Concat(second);
        }
    }
}
