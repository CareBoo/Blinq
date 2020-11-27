using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<
            T,
            DistinctSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator>,
            DistinctSequence<T, NativeArraySequence<T>, NativeArray<T>.Enumerator>.Enumerator>
        Distinct<T>(
            this in NativeArray<T> source
            )
            where T : unmanaged, IEquatable<T>
        {
            return source.ToValueSequence().Distinct();
        }
    }
}
