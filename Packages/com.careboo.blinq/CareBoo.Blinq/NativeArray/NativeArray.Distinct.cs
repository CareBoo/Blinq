using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, DistinctSequence<T, NativeArraySequence<T>>> Distinct<T>(
            this ref NativeArray<T> source
            )
            where T : unmanaged, IEquatable<T>
        {
            var seq = source.ToValueSequence();
            return seq.Distinct();
        }
    }
}
