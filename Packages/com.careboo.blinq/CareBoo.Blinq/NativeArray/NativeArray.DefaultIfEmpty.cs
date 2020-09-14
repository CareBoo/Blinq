using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<T, DefaultIfEmptySequence<T, NativeArraySequence<T>>> DefaultIfEmpty<T>(
            this ref NativeArray<T> source,
            T defaultVal = default
            )
            where T : struct
        {
            var seq = source.ToValueSequence();
            return seq.DefaultIfEmpty(defaultVal);
        }
    }
}
