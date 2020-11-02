﻿using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class Sequence
    {
        public static ValueSequence<T, DefaultIfEmptySequence<T, NativeArraySequence<T>>> DefaultIfEmpty<T>(
            this ref NativeArray<T> source,
            T defaultVal = default
            )
            where T : struct
        {
            var sourceSeq = source.ToValueSequence();
            return sourceSeq.DefaultIfEmpty(defaultVal);
        }
    }
}
