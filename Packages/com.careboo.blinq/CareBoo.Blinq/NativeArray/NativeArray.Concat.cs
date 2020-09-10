using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<T, ValueSequence<T, NativeArraySequence<T>>.ConcatSequence<TSecond>> Concat<T, TSecond>(
            this ref NativeArray<T> source,
            TSecond second
            )
            where T : unmanaged, IEquatable<T>
            where TSecond : struct, ISequence<T>
        {
            return source.ToValueSequence().Concat(second);
        }
    }
}
