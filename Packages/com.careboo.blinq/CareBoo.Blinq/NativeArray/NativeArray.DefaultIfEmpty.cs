using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<T, ValueSequence<T, NativeArraySequence<T>>.DefaultIfEmptySequence> DefaultIfEmpty<T>(
            this ref NativeArray<T> source,
            T defaultVal = default
            )
            where T : unmanaged, IEquatable<T>
        {
            return source.ToValueSequence().DefaultIfEmpty(defaultVal);
        }
    }
}
