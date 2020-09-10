using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<T, ValueSequence<T, NativeArraySequence<T>>.ConcatSequence<TSequence>> Concat<T, TSequence>(
            this ref NativeArray<T> source,
            TSequence second
            )
            where T : unmanaged, IEquatable<T>
            where TSequence : struct, ISequence<T>
        {
            return source.ToValueSequence().Concat(second);
        }
    }
}
