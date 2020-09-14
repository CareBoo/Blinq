using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<T, ConcatSequence<T, NativeArraySequence<T>, TSecond>> Concat<T, TSecond>(
            this ref NativeArray<T> source,
            TSecond second
            )
            where T : struct
            where TSecond : struct, ISequence<T>
        {
            var seq = source.ToValueSequence();
            return seq.Concat(second);
        }
    }
}
