﻿using System;
using Unity.Collections;

namespace CareBoo.Blinq
{
    public static partial class NativeArrayExtensions
    {
        public static ValueSequence<T, AppendSequence<T, NativeArraySequence<T>>> Append<T>(this ref NativeArray<T> source, T item)
            where T : struct
        {
            var seq = source.ToValueSequence();
            return seq.Append(item);
        }
    }
}
